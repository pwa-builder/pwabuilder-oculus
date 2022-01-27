using System.IO.Compression;

namespace Microsoft.PWABuilder.Oculus.Common
{
    public static class ZipArchiveExtensions
    {
        /// <summary>
        /// Creates a new file in a zip and writes the contents to it.
        /// </summary>
        /// <param name="zip">The zip to add the file to.</param>
        /// <param name="fileContents">The string contents of the file.</param>
        /// <param name="entryName">The name of the entry in the zip file.</param>
        /// <returns>A new zip entry.</returns>
        public static async Task<ZipArchiveEntry> CreateEntryFromString(this ZipArchive zip, string fileContents, string entryName)
        {
            var entry = zip.CreateEntry(entryName);
            var encoding = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true); // necessary for the "install.ps1" powershell script to work with Unicode app names
            using (var installStream = new StreamWriter(entry.Open(), encoding))
            {
                await installStream.WriteAsync(fileContents);
            }

            return entry;
        }
    }
}
