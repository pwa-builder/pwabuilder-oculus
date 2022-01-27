namespace Microsoft.PWABuilder.Oculus.Models
{
    /// <summary>
    /// Contains information about a .keystore file on disk and details about the key inside it.
    /// </summary>
    /// <param name="KeystoreFilePath">The path to the new .keystore file.</param>
    /// <param name="Alias">The alias of the key.</param>
    /// <param name="KeyPassword">The password for the key.</param>
    /// <param name="StorePassword">The password for the .keystore file.</param>
    public record KeystoreFile(string Path, string Alias, string KeyPassword, string StorePassword);
}
