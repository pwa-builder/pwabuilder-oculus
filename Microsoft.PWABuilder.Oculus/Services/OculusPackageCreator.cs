using Microsoft.Extensions.Options;
using Microsoft.PWABuilder.Oculus.Models;

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

            // Run the Oculus CLI.
            var oculusCliResult = await oculusCli.CreateApk(packageOptions, outputDirectory);

            // Zip up the APK and the readme doc.
            var zipFilePath = CreateZipPackage(oculusCliResult.ApkFilePath, this.appSettings.ReadMePath);

            return zipFilePath;
        }

        private string CreateZipPackage(string apkFilePath, string readmeFilePath)
        {
            // TODO: Create a new zip file containing the APK and our readme
            throw new NotImplementedException();
        }
    }
}
