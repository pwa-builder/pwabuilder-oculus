using Microsoft.Extensions.Options;
using Microsoft.PWABuilder.Oculus.Models;

namespace Microsoft.PWABuilder.Oculus.Services
{
    /// <summary>
    /// Sends Oculus app package creation events to PWABuilder's backend analytics service.
    /// </summary>
    public class Analytics
    {
        private readonly IOptions<AppSettings> settings;
        private readonly ILogger<Analytics> logger;
        private readonly HttpClient http;

        public Analytics(
            IOptions<AppSettings> settings,
            IHttpClientFactory httpClientFactory,
            ILogger<Analytics> logger)
        {
            this.settings = settings;
            this.http = httpClientFactory.CreateClient();
            this.logger = logger;
        }

        /// <summary>
        /// Records an Oculus app package creation with the backend analytics service.
        /// </summary>
        /// <param name="uri">The URI of the app that was generated.</param>
        /// <param name="success">Whether the generation was successful.</param>
        /// <param name="error">The message of the error that occurred during Oculus app package generation.</param>
        public void Record(Uri uri, bool success, string? error)
        {
            if (string.IsNullOrEmpty(this.settings.Value.AnalyticsUrl))
            {
                this.logger.LogWarning("Skipping analytics event recording due to no analytics URL in app settings. For development, this should be expected.");
                return;
            }

            var args = System.Text.Json.JsonSerializer.Serialize(new
            {
                Url = uri,
                OculusPackage = success,
                OculusPackageError = error
            });
            this.http.PostAsync(this.settings.Value.AnalyticsUrl, new StringContent(args))
                .ContinueWith(_ => logger.LogInformation("Successfully recorded Oculus package generation for {url} to backend analytics service. Success = {success}, Error = {error}", uri, success, error), TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(task => logger.LogError(task.Exception ?? new Exception("Unable to record Oculus package generation to backend analytics service"), "Unable to record Oculus package generation for {url} to backend analytics service due to an error", uri), TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
