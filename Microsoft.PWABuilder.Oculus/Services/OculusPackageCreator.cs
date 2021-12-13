using Microsoft.Extensions.Options;
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
        private readonly TempDirectory temp;
        private readonly AppSettings appSettings;
        private readonly ILogger<OculusPackageCreator> logger;

        public OculusPackageCreator(
            OculusCliWrapper oculusCli,
            TempDirectory temp, 
            IOptions<AppSettings> appSettings,
            ILogger<OculusPackageCreator> logger)
        {
            this.oculusCli = oculusCli;
            this.temp = temp;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a zip file containing the APK (app package file) and a next steps readme document that shows the developer how to publish the package to the Oculus store.
        /// </summary>
        /// <param name="packageOptions">The app packaging options.</param>
        /// <returns></returns>
        public async Task<string> Create(OculusAppPackageOptions.Validated packageOptions)
        {
            // Create our temporary output directory.
            var outputDirectory = temp.CreateDirectory();

            var filePath = await CreateManifestFile(packageOptions, outputDirectory);

            // Run the Oculus CLI.
            var oculusCliResult = await oculusCli.CreateApk(packageOptions, outputDirectory, filePath);

            // Zip up the APK and the readme doc.
            var zipFilePath = CreateZipPackage(oculusCliResult.ApkFilePath, this.appSettings.ReadMePath, outputDirectory);

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

        private string CreateZipPackage(string apkFilePath, string readmeFilePath, string outputDirectory)
        {
            var zipPath = Path.Combine(outputDirectory, "output.zip");
            using var zipFile = File.Create(zipPath);
            using var zipArchive = new ZipArchive(zipFile, ZipArchiveMode.Create);
            zipArchive.CreateEntryFromFile(apkFilePath, "output.apk");
            zipArchive.CreateEntryFromFile(readmeFilePath, "readme.md");

            // TODO: Create a new zip file containing the APK and our readme
            return zipPath;
        }
    }
}
