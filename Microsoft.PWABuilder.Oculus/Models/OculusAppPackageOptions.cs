namespace Microsoft.PWABuilder.Oculus.Models
{
    public class OculusAppPackageOptions
    {
        /// <summary>
        /// The package ID that uniquely identifies the app. Can only contain letters, numbers, hypen, and period. Typically, "MyCompany.MyApp". If unspecified, one will be created based on the URL.
        /// </summary>
        public string PackageId { get; set; } = string.Empty;

        /// <summary>
        /// Gets the display name of the app. If not supplied, the name will be fetched from the web manifest.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The URL of the PWA to generate an .msix file from.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// The version of the app.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Whether the generated MSIX package will be signable. If false, the app won't be able to be submitted to the Store.
        /// </summary>
        public bool AllowSigning { get; set; }

    }
}
