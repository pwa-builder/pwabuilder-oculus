using Microsoft.ApplicationInsights;
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
        private readonly TelemetryClient telemetryClient;
        private readonly bool isAppInsightsEnabled;
        public Analytics(
            IOptions<AppSettings> settings,
            IHttpClientFactory httpClientFactory,
            ILogger<Analytics> logger,
            TelemetryClient telemetryClient)
        {
            this.settings = settings;
            this.http = httpClientFactory.CreateClient();
            this.logger = logger;
            this.telemetryClient = telemetryClient;
            this.isAppInsightsEnabled = !string.IsNullOrEmpty(this.settings.Value.ApplicationInsightsConnectionString);
        }

        /// <summary>
        /// Records an Oculus app package creation with the backend analytics service.
        /// </summary>
        /// <param name="uri">The URI of the app that was generated.</param>
        /// <param name="success">Whether the generation was successful.</param>
        /// <param name="error">The message of the error that occurred during Oculus app package generation.</param>
        public void Record(Uri uri, bool success, string? error, AnalyticsInfo? analyticsInfo, OculusAppPackageOptions.Validated? packageOptions)
        {
            //TODO: Code to remove in the future starts here
            if (!string.IsNullOrEmpty(this.settings.Value.AnalyticsUrl))
            {
                LogToRavenDB(uri, success, error);
            }
            else
            {
               this.logger.LogWarning("Skipping analytics event recording in RavenDB due to no analytics URL in app settings. For development, this should be expected.");
            }
            //Code to remove ends here

            if(!this.isAppInsightsEnabled)
            {
                this.logger.LogWarning("Skipping analytics event recording in App insights due to no connection string. For development, this should be expected.");
                return;
            }
            this.telemetryClient.Context.Operation.Id = analyticsInfo?.correlationId != null ? analyticsInfo.correlationId : System.Guid.NewGuid().ToString();

            Dictionary<string, string> record;
            var name = "";
            if (success && packageOptions != null)
            {
                record = new() { { "URL", uri.ToString() }, { "OculusPackageID", packageOptions.PackageId }, { "OculusAppName", packageOptions.Name } };
                name = "OculusPackageEvent";
            }
            else
            {
                record = new() { { "URL", uri.ToString() }, { "OculusPackageError", error ?? "" } };
                name = "OculusPackageFailureEvent";
            }
            if (analyticsInfo?.platformId != null)
            {
                record.Add("PlatformId", analyticsInfo.platformId);
                if (analyticsInfo?.platformIdVersion != null)
                {
                    record.Add("PlatformVersion", analyticsInfo.platformIdVersion);
                }
            }
            if(analyticsInfo?.referrer != null)
            {
                record.Add("Referrer", analyticsInfo.referrer);
            }
            telemetryClient.TrackEvent(name, record);
        }

        private void LogToRavenDB(Uri uri, bool success, string? error)
        {
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
