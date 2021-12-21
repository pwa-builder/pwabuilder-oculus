using Microsoft.Extensions.Options;
using Microsoft.PWABuilder.Oculus.Models;

namespace Microsoft.PWABuilder.Oculus.Services
{
    /// <summary>
    /// Wraps the Java SDK's keytool.exe command line utility. Used for generating new keys.
    /// </summary>
    public class KeyToolWrapper
    {
        private readonly ProcessRunner procRunner;
        private readonly ILogger<KeyToolWrapper> logger;
        private readonly AppSettings appSettings;

        public KeyToolWrapper(ProcessRunner procRunner, IOptions<AppSettings> appSettings, ILogger<KeyToolWrapper> logger)
        {
            this.procRunner = procRunner;
            this.logger = logger;
            this.appSettings = appSettings.Value;
        }

        /// <summary>
        /// Runs the keytool.exe CLI and generates a .keystore file.
        /// </summary>
        /// <param name="args">The keytool.exe arguments.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <returns>File path to the created .keystore file.</returns>
        public async Task<KeystoreFile> CreateKeystore(OculusAppPackageOptions.Validated packageOptions, string outputDirectory)
        {
            var dName = CreateDNameFromPackageOptions(packageOptions);
            var keyPassword = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var storePassword = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var alias = "oculuspwa";
            var outputFilePath = Path.Combine(outputDirectory, $"signing-key.keystore");
            var keyToolCommand = $"-genkeypair -dname \"{dName}\" -alias {alias} -keypass {keyPassword} -keystore \"{outputFilePath}\" -storepass {storePassword} -validity 20000 -keyalg RSA";
            
            try
            {
                var keyToolProcOutput = await this.procRunner.Run(appSettings.KeyToolPath, keyToolCommand, TimeSpan.FromMinutes(1));

                // Log warning if there was any error information output by keytool.exe.
                if (!string.IsNullOrWhiteSpace(keyToolProcOutput.StandardError))
                {
                    logger.LogWarning("Keytool ran successfully using \"{cmd}\". However, it output error information: {error}.{newline}Standard output: {output}", keyToolCommand, keyToolProcOutput.StandardError, Environment.NewLine, keyToolProcOutput.StandardOutput);
                }
                else
                {
                    logger.LogInformation("Keytool successfully generated a new key using \"{cmd}\". Standard output:{newline}{output}", keyToolCommand, Environment.NewLine, keyToolProcOutput.StandardOutput);
                }
            }
            catch (Exception error)
            {
                logger.LogError(error, "Keytool was unable to generate a new keystore file.");
                throw;
            }

            if (!File.Exists(outputFilePath))
            {
                throw new FileNotFoundException("Keytool reported that it ran successfully and generated a new .keystore file, however, no such .keystore file was found on disk.", outputFilePath);
            }

            return new KeystoreFile(outputFilePath, alias, keyPassword, storePassword);
        }

        /// <summary>
        /// Commas in the common name field must be escaped so that "te,st" becomes "te\,st".
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string EscapeDName(string input)
        {
            return input.Replace(",", "\\,");
        }

        private static string CreateDNameFromPackageOptions(OculusAppPackageOptions.Validated options)
        {
            return $"cn={EscapeDName(options.ManifestUri.Host)}, " +
                "ou=Engineering, " +
                $"o={EscapeDName(options.Name)}, " +
                "c=US";
        }
    }
}
