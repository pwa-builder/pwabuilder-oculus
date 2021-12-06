using Microsoft.Extensions.Options;
using Microsoft.PWABuilder.Oculus.Models;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

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

        //Pass the manifest file

        public async Task<OculusCliResult> CreateApk(OculusAppPackageOptions.Validated packageOptions, string outputDirectory, string manifestFilePath)
        {
            // Run the Oculus CLI tool.
            // TODO: implement this. See Oculus PWA Getting Started.pdf.

            ProcessResult procResult;

            try
            {
                procResult = await procRunner.Run(appSettings.OculusCliPath, CreateCommandLineArgs(packageOptions, outputDirectory, manifestFilePath), TimeSpan.FromMinutes(5));
            }
            catch (Exception error)
            {
                var stdOut = (error as ProcessException)?.StandardOutput;
                var stdErr = (error as ProcessException)?.StandardError;
                //TODO Add Custom Exception class
                throw new Exception(stdErr);
            }
            return new OculusCliResult
            {
                ApkFilePath = Path.Combine(outputDirectory, "output.apk"), // TODO: implement this
            };
        }


        /// <summary>
        /// Creates command line arguments for the pwa_builder.exe command line tool from the specified options.
        /// </summary>
        /// <returns></returns>
        protected virtual string CreateCommandLineArgs(OculusAppPackageOptions.Validated options, string outputDirectory, string manifestFilePath)
        {
            var args = new Dictionary<string, string?>
            {
                { "create-pwa","" },
                { "out", Path.Combine(outputDirectory, "output.apk").ToString() },
                { "android-sdk", appSettings.AndroidSdkPath },
                { "manifest-content-file", manifestFilePath},
                { "web-manifest-url", options.manifestUri.ToString() },            
            };

            var builder = new StringBuilder();

            foreach (var arg in args)
            {
                if (!string.IsNullOrWhiteSpace(arg.Value))
                {
                    builder.Append($"--{arg.Key}=\"{arg.Value}\"");
                }
                else
                {
                    builder.Append(arg.Key);
                }
                builder.Append(' ');
            }
            return builder.ToString();
        }
    }
}
