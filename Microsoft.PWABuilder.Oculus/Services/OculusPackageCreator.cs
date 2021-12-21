using Microsoft.Extensions.Options;
using Microsoft.PWABuilder.Oculus.Common;
using Microsoft.PWABuilder.Oculus.Models;
using System.IO.Compression;
using System.Text.Json;

namespace Microsoft.PWABuilder.Oculus.Services
{
    /// <summary>
    /// Service that generates Oculus app packages.
    /// </summary>
    public class OculusPackageCreator
    {
        private readonly OculusCliWrapper oculusCli;
        private readonly KeyToolWrapper keyToolWrapper;
        private readonly TempDirectory temp;
        private readonly AppSettings appSettings;
        private readonly ILogger<OculusPackageCreator> logger;

        public OculusPackageCreator(
            OculusCliWrapper oculusCli,
            KeyToolWrapper keyTool,
            TempDirectory temp, 
            IOptions<AppSettings> appSettings,
            ILogger<OculusPackageCreator> logger)
        {
            this.oculusCli = oculusCli;
            this.keyToolWrapper = keyTool;
            this.temp = temp;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a zip file containing the APK (app package file) and a next steps readme document that shows the developer how to publish the package to the Oculus store.
        /// </summary>
        /// <param name="packageOptions">The app packaging options.</param>
        /// <returns>Path to the zip file containing the APK, documentation, and key information.</returns>
        public async Task<string> Create(OculusAppPackageOptions.Validated packageOptions)
        {
            // Create our temporary output directory.
            var outputDirectory = temp.CreateDirectory();

            // Write the manifest to disk
            var filePath = await CreateManifestFile(packageOptions, outputDirectory);

            // Save the signing key to disk if need be.
            var signingKeyDetails = await SaveSigningKeyFile(packageOptions, outputDirectory);

            // Run the Oculus CLI.
            var oculusCliResult = await oculusCli.CreateApk(packageOptions, signingKeyDetails, outputDirectory, filePath);

            // Zip up the APK and the readme doc.
            var zipFilePath = await CreateZipPackage(
                oculusCliResult.ApkFilePath, 
                this.appSettings.ReadMePath, 
                signingKeyDetails,
                packageOptions.Name, 
                outputDirectory);

            return zipFilePath;
        }

        /// <summary>
        /// Creates manifest file from specified options
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<string> CreateManifestFile(OculusAppPackageOptions.Validated options, string outputDirectory)
        {
            var jsonString = JsonSerializer.Serialize(options.Manifest);
            var filePath = Path.Combine(outputDirectory, "pwa.json");
            await File.WriteAllTextAsync(filePath, jsonString);
            return filePath;
        }

        private async Task<string> CreateZipPackage(string apkFilePath, string readmeFilePath, KeystoreFile? signingKeyDetails, string appName, string outputDirectory)
        {
            var zipPath = Path.Combine(outputDirectory, "output.zip");
            using var zipFile = File.Create(zipPath);
            using var zipArchive = new ZipArchive(zipFile, ZipArchiveMode.Create);
            zipArchive.CreateEntryFromFile(apkFilePath, $"{GetFileSystemSafeAppName(appName)}.apk");
            zipArchive.CreateEntryFromFile(readmeFilePath, "readme.html");

            // If we have a signing key, include that in the zip, along with a .txt file containing signing key information.
            if (signingKeyDetails != null)
            {
                zipArchive.CreateEntryFromFile(signingKeyDetails.Path, "signing.keystore");

                var keystoreReadme = $"Keep this file and signing.keystore in a safe place. You'll need these files if you want to upload future versions of your PWA to the Oculus Store.\r\n" +
                    "Key store file: signing.keystore" +
                    $"Keystore password: {signingKeyDetails.StorePassword}\r\n" +
                    $"Key alias: {signingKeyDetails.Alias}\r\n" +
                    $"Key password: {signingKeyDetails.KeyPassword}\r\n";
                await zipArchive.CreateEntryFromString(keystoreReadme, "signing-key-info.txt");
            }

            return zipPath;
        }

        private string GetFileSystemSafeAppName(string appName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var validAppNameChars = appName
                .Select(c => invalidChars.Contains(c) ? '_' : c)
                .ToArray();
            return new string(validAppNameChars);            
        }

        private async Task<KeystoreFile?> SaveSigningKeyFile(OculusAppPackageOptions.Validated packageOptions, string outputDirectory)
        {
            return packageOptions.SigningMode switch
            {
                SigningMode.None => null,
                SigningMode.Existing => await SaveExistingSigningKeyFile(packageOptions, outputDirectory),
                SigningMode.New => await keyToolWrapper.CreateKeystore(packageOptions, outputDirectory),
                _ => throw new NotImplementedException($"Unknown signing mode passed in: {packageOptions.SigningMode}")
            };
        }

        private async Task<KeystoreFile> SaveExistingSigningKeyFile(OculusAppPackageOptions.Validated packageOptions, string outputDirectory)
        {
            if (packageOptions.ExistingSigningKey == null)
            {
                throw new InvalidOperationException("Can't save existing signing key because packageOptions.ExistingSigningKey is null");
            }

            // If we're configured to sign with an existing key, write that key to disk.
            var keyStorePath = Path.Combine(outputDirectory, $"{Guid.NewGuid()}.keystore");
            try
            {
                var keyStoreBytes = Convert.FromBase64String(packageOptions.ExistingSigningKey.KeyStoreFile);
                await File.WriteAllBytesAsync(keyStorePath, keyStoreBytes);
                return new KeystoreFile(keyStorePath, packageOptions.ExistingSigningKey.Alias, packageOptions.ExistingSigningKey.Password, packageOptions.ExistingSigningKey.StorePassword);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Error creating key store file for PWA {name} at {url}", packageOptions.Name, packageOptions.ManifestUri);
                throw;
            }
        }
    }
}
