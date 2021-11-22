using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PWABuilder.Oculus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.PWABuilder.Oculus.Services
{
    /// <summary>
    /// Creates and tracks temporary files and directories and deletes them when CleanUp() is called.
    /// </summary>
    public class TempDirectory : IDisposable
    {
        private readonly List<string> directoriesToCleanUp = new();
        private readonly List<string> filesToCleanUp = new();
        private readonly ILogger<TempDirectory> logger;
        private readonly AppSettings appSettings;

        public TempDirectory(
            IOptions<AppSettings> appSettings,
            ILogger<TempDirectory> logger)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a temporary directory that will be cleaned up on disposal.
        /// </summary>
        /// <param name="dirName">The directory name. If null or empty, a guid will be used as the directory name.</param>
        /// <returns>The path to the temporary directory.</returns>
        public string CreateDirectory(string? dirName = null)
        {
            var expandedOutputDir = Environment.ExpandEnvironmentVariables(this.appSettings.TempDirectory);
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = Guid.NewGuid().ToString();
            }
            var outputFolder = Path.Combine(expandedOutputDir, dirName);
            Directory.CreateDirectory(outputFolder);
            directoriesToCleanUp.Add(outputFolder);
            return outputFolder;
        }

        /// <summary>
        /// Creates an empty temporary file that will be cleaned up on disposal.
        /// </summary>
        /// <returns>The path to the empty file.</returns>
        public string CreateFile()
        {
            // Make sure the output directory exists
            var expandedOutputDir = Environment.ExpandEnvironmentVariables(this.appSettings.TempDirectory);
            Directory.CreateDirectory(expandedOutputDir);

            var tempFileName = Path.Combine(expandedOutputDir, Guid.NewGuid().ToString() + ".tmp");
            this.filesToCleanUp.Add(tempFileName);

            // Create a zero-byte file.
            File.WriteAllBytes(tempFileName, Array.Empty<byte>());

            return tempFileName;
        }

        /// <summary>
        /// Deletes all the temporary files and directories create by this instance.
        /// This is called automatically during <see cref="Dispose"/>.
        /// </summary>
        public void CleanUp()
        {
            foreach (var file in this.filesToCleanUp)
            {
                if (!string.IsNullOrWhiteSpace(file))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception fileDeleteError)
                    {
                        logger.LogWarning(fileDeleteError, "Unable to cleanup {zipFile}", file);
                    }
                }
            }

            foreach (var directory in this.directoriesToCleanUp)
            {
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    try
                    {
                        Directory.Delete(directory, recursive: true);
                    }
                    catch (Exception directoryDeleteError)
                    {
                        logger.LogWarning(directoryDeleteError, "Unable to cleanup {directory}", directory);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes all the temporary files and directories created by this instance.
        /// </summary>
        public void Dispose()
        {
            CleanUp();
            GC.SuppressFinalize(this);
        }
    }
}
