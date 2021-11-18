namespace Microsoft.PWABuilder.Oculus.Models
{
    public class OculusAppPackageOptions
    {

        /// <summary>
        /// Represents the name of the web application as it is usually displayed to the user
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Determines the developers’ preferred display mode for the website. Only "standalone" or "minimal-ui" are accepted. (Setting standalone as default)
        /// </summary>
        public string Display { get; set; } = "standalone";

        /// <summary>
        /// Represents the name of the web application displayed to the user if there is not enough space to display name.
        /// </summary>
        public string? ShortName { get; set; }

        /// <summary>
        /// Defines the navigation scope of this web application's application context. It restricts what web pages can be viewed while the manifest is applied. If the user navigates outside the scope, it reverts to a normal web page inside a browser tab or window.
        /// </summary>
        public string? Scope { get; set; }

        /// <summary>
        /// Represents the start URL of the web application — the preferred URL that should be loaded when the user launches the web application.
        /// </summary>
        public string? StartUrl { get; set; }

        /// <summary>
        /// If true, this boolean field will give your PWA a tab bar similar to Oculus Browser
        /// </summary>
        public bool? OvrMultiTabEnabled { get; set; }

        /// <summary>
        /// Allows your PWA to include more web pages within the scope of your web application. Consists of a JSON dictionary containing extension URLs or wildcard patterns.
        /// </summary>
        public string? OvrScopeExtensions { get; set; }

    }
}
