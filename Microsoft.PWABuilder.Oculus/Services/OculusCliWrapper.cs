using Microsoft.Extensions.Options;
using Microsoft.PWABuilder.Oculus.Models;

namespace Microsoft.PWABuilder.Oculus.Services
{
    /// <summary>
    /// Wraps the Oculus CLI tool to allow for ease of execution.
    /// </summary>
    public class OculusCliWrapper
    {
        private readonly ProcessRunner procRunner;
        private readonly AppSettings appSettings;
        private readonly ILogger<OculusCliWrapper> logger;

        public OculusCliWrapper(
            ProcessRunner procRunner,
            IOptions<AppSettings> appSettings,
            ILogger<OculusCliWrapper> logger)
        {
            this.procRunner = procRunner;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public async Task<OculusCliResult> CreateApk(OculusAppPackageOptions.Validated packageOptions, string outputDirectory)
        {
            // Run the Oculus CLI tool.
            // TODO: implement this. See Oculus PWA Getting Started.pdf.
            var result = await procRunner.Run(appSettings.OculusCliPath, "Oculus command line args go here", TimeSpan.FromMinutes(5));

            return new OculusCliResult
            {
                ApkFilePath = "" // TODO: implement this
            };
        }
    }
}
