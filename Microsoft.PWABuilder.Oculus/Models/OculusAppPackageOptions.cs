namespace Microsoft.PWABuilder.Oculus.Models
{
    /// <summary>
    /// Options for generating an Oculus app package.
    /// </summary>
    public class OculusAppPackageOptions
    {
        /// <summary>
        /// The Oculus app ID. 
        /// </summary>
        public string? AppId { get; set; }

        /// <summary>
        /// The app name. 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The URL of the PWA.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// The app version.
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// The URL of the PWA's web app manifest.
        /// </summary>
        public string? ManifestUrl { get; set; }

        /// <summary>
        /// The contents of the web app manifest.
        /// </summary>
        public WebAppManifest? Manifest { get; set; }

        /// <summary>
        /// Validates the package options. If valid, a <see cref="Validated"/> instance is returned. Otherwise, an exception is thrown.
        /// </summary>
        /// <returns>A validated options instance.</returns>
        public Validated Validate()
        {
            if (string.IsNullOrWhiteSpace(AppId))
            {
                throw new ArgumentNullException(nameof(AppId));
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentNullException(nameof(Name));
            }

            if (Name.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(Name), Name.Length, "Name must be at least 3 characters in length.");
            }

            if (!System.Version.TryParse(Version, out var version))
            {
                throw new ArgumentException("Version must be a valid version string, e.g. '1.0.0.0'", nameof(Version));
            }

            if (!Uri.TryCreate(Url, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("Url must be a valid, absolute URL", nameof(Url));
            }

            if (!Uri.TryCreate(ManifestUrl, UriKind.Absolute, out var manifestUri))
            {
                throw new ArgumentException("Manifest URL must be a valid, absolute URL", nameof(ManifestUrl));
            }

            ArgumentNullException.ThrowIfNull(Manifest);

            return new Validated(AppId, Name, uri, version, manifestUri, Manifest);
        }

        public record Validated(
            string AppId, 
            string Name, 
            Uri Uri, 
            Version Version, 
            Uri manifestUri, 
            WebAppManifest Manifest);
    }
}
