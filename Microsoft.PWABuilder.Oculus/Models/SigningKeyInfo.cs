namespace Microsoft.PWABuilder.Oculus.Models
{
    /// <summary>
    /// Key signing details used for signing an Oculus APK file.
    /// </summary>
    public class SigningKeyInfo
    {
        /// <summary>
        /// Whether to skip signing. This can be useful if you want to generate an unsigned APK.
        /// </summary>
        public bool? SkipSigning { get; set; }

        /// <summary>
        /// Base64 encoded .keystore file.
        /// </summary>
        public string? KeyStoreFile { get; set; }

        /// <summary>
        /// The password for the keystore file.
        /// </summary>
        public string? StorePassword { get; set; }

        /// <summary>
        /// Alias of the key to retrieve from the keystore file.
        /// </summary>
        public string? Alias { get; set; }

        /// <summary>
        /// The password for the key specified by <see cref="Alias"/>.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Validates the key info and returns a <see cref="SigningKeyInfo.Validated"/> result.
        /// </summary>
        /// <returns>The validated signing key info.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Validated Validate()
        {
            // If we're configured to skip signing, we ignore all the other fields.
            if (SkipSigning == true)
            {
                return new Validated(true, KeyStoreFile ?? string.Empty, StorePassword ?? string.Empty, Alias ?? string.Empty, Password ?? string.Empty);
            }

            // If the key store file is null or empty, OK, we'll ignore all the other fields and just generate a signing key for them.
            if (string.IsNullOrWhiteSpace(KeyStoreFile))
            {
                return new Validated(false, string.Empty, string.Empty, string.Empty, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(StorePassword))
            {
                throw new ArgumentNullException(nameof(StorePassword));
            }

            if (string.IsNullOrWhiteSpace(Alias))
            {
                throw new ArgumentNullException(Alias);
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentNullException(Password);
            }

            return new Validated(SkipSigning ?? false, KeyStoreFile, StorePassword, Alias, Password);
        }

        /// <summary>
        /// A validated signing key info.
        /// </summary>
        /// <param name="SkipSigning">Whether to skip signing.</param>
        /// <param name="KeyStoreFile">The base64-encoded key store file. If an empty string, a new keystore file will be generated and used to sign the APK.</param>
        /// <param name="StorePassword">The store password.</param>
        /// <param name="Alias">The alias of the key to use to sign the APK.</param>
        /// <param name="Password">The password for the key to use to sign the APK.</param>
        public record Validated(
            bool SkipSigning, 
            string KeyStoreFile, 
            string StorePassword, 
            string Alias, 
            string Password);
    }
}
