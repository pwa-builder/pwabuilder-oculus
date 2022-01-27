namespace Microsoft.PWABuilder.Oculus.Models
{
    /// <summary>
    /// Specifies the signing mode.
    /// </summary>
    public enum SigningMode
    {
        /// <summary>
        /// No signing key. The APK will be unsigned. Developers will need to sign the APK manually before uploading to the Oculus store.
        /// </summary>
        None,
        /// <summary>
        /// Create a new signing key with which to sign the APK.
        /// </summary>
        New,
        /// <summary>
        /// Use an existing signing key to sign the APK.
        /// </summary>
        Existing
    }
}
