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
        /// The URL of the PWA.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// The version display name to use. This can be a proper version, such as "1.0.0.0", or can be a string of your choosing, e.g. "1.0beta2". 
        /// This is purely for display to end users.
        /// Oculus Store instead goes by <see cref="VersionCode"/> to determine the true version of your app.
        /// </summary>
        public string? VersionName { get; set; }

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
        /// The signing configuration, whether to sign the APK with a new signing key, an existing signing key, or to skip signing.
        /// </summary>
        public SigningMode SigningMode { get; set; }

        /// <summary>
        /// The existing signing key. If <see cref="SigningMode"/> is set to <see cref="SigningMode.Existing"/>, this must be the existing signing key details. Otherwise, this should be null.
        /// </summary>
        public ExistingSigningKeyInfo? ExistingSigningKey { get; set; }

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

            Uri.TryCreate(this.Url, UriKind.Absolute, out var uri);
            if (uri == null)
            {
                throw new ArgumentException("Url must be a valid, absolute URL", nameof(Url));
            }

            // Version name, used only for displaying to end users, is optional.
            // If omitted, we'll use a stringified VersionCode.
            var versionName = string.IsNullOrWhiteSpace(this.VersionName) ? $"{VersionCode}.0.0.0" : this.VersionName;

            if (!Uri.TryCreate(ManifestUrl, UriKind.Absolute, out var manifestUri))
            {
                throw new ArgumentException("Manifest URL must be a valid, absolute URL", nameof(ManifestUrl));
            }

            if (this.SigningMode == SigningMode.Existing && this.ExistingSigningKey == null)
            {
                throw new ArgumentNullException(nameof(ExistingSigningKey), "ExistingSigningKey must not be null when SigningMode is set to SigningMode.Existing");
            }

            ArgumentNullException.ThrowIfNull(Manifest);
            var validSigningKey = ExistingSigningKey?.Validate();
            return new Validated(
                PackageId, 
                Name, 
                uri,
                VersionCode.Value, 
                versionName,
                manifestUri, 
                Manifest, 
                this.SigningMode, 
                validSigningKey);
        }

        public record Validated(
            string PackageId, 
            string Name, 
            Uri Uri,
            int VersionCode, 
            string VersionName,
            Uri ManifestUri, 
            WebAppManifest Manifest,
            SigningMode SigningMode,
            ExistingSigningKeyInfo.Validated? ExistingSigningKey);
    }
}
