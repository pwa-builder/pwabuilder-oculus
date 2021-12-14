namespace Microsoft.PWABuilder.Oculus.Models
{
    /// <summary>
    /// Contains information about an existing signing key used for signing the APK.
    /// </summary>
    public class SigningKeyInfo
    {
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
            if (string.IsNullOrWhiteSpace(KeyStoreFile))
            {
                throw new ArgumentNullException(nameof(KeyStoreFile));
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

            return new Validated(KeyStoreFile, StorePassword, Alias, Password);
        }

        /// <summary>
        /// A validated signing key info.
        /// </summary>
        /// <param name="KeyStoreFile">The base64-encoded key store file. If an empty string, a new keystore file will be generated and used to sign the APK.</param>
        /// <param name="StorePassword">The store password.</param>
        /// <param name="Alias">The alias of the key to use to sign the APK.</param>
        /// <param name="Password">The password for the key to use to sign the APK.</param>
        public record Validated(
            string KeyStoreFile, 
            string StorePassword, 
            string Alias, 
            string Password);
    }
}
