namespace Microsoft.PWABuilder.Oculus.Models
{
    /// <summary>
    /// Options for generating an Oculus app package.
    /// </summary>
    public class OculusAppPackageOptions
    {
        /// <summary>
        /// The Oculus app package ID. Usually a reverse-domain style string, e.g. com.myawesomepwa
        /// </summary>
        public string? PackageId { get; set; }

        /// <summary>
        /// The app name. 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The app version code.
        /// </summary>
        public int? VersionCode { get; set; }

        /// <summary>
        /// The URL of the PWA's web app manifest.
        /// </summary>
        public string? ManifestUrl { get; set; }

        /// <summary>
        /// The contents of the web app manifest.
        /// </summary>
        public WebAppManifest? Manifest { get; set; }

        /// <summary>
        /// The signing key info. If omitted, a new key will be generated and used to sign the APK.
        /// </summary>
        public SigningKeyInfo? SigningKey { get; set; }

        /// <summary>
        /// Validates the package options. If valid, a <see cref="Validated"/> instance is returned. Otherwise, an exception is thrown.
        /// </summary>
        /// <returns>A validated options instance.</returns>
        public Validated Validate()
        {
            if (string.IsNullOrWhiteSpace(PackageId))
            {
                throw new ArgumentNullException(nameof(PackageId));
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentNullException(nameof(Name));
            }

            if (Name.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(Name), Name.Length, "Name must be at least 3 characters in length.");
            }

            if (VersionCode <= 0 || VersionCode == null)
            {
                throw new ArgumentOutOfRangeException(nameof(VersionCode), VersionCode, "Version code must be greater than zero");
            }

            if (!Uri.TryCreate(ManifestUrl, UriKind.Absolute, out var manifestUri))
            {
                throw new ArgumentException("Manifest URL must be a valid, absolute URL", nameof(ManifestUrl));
            }

            ArgumentNullException.ThrowIfNull(Manifest);
            var validSigningKey = SigningKey?.Validate();
            return new Validated(PackageId, Name, VersionCode.Value, manifestUri, Manifest, validSigningKey);
        }

        public record Validated(
            string PackageId, 
            string Name, 
            int VersionCode, 
            Uri ManifestUri, 
            WebAppManifest Manifest,
            SigningKeyInfo.Validated? SigningKey);
    }
}
