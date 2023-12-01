using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PWABuilder.Oculus.Models;
using Microsoft.PWABuilder.Oculus.Services;

namespace Microsoft.PWABuilder.Oculus.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PackagesController : ControllerBase
    {
        private readonly OculusPackageCreator packageCreator;

        public PackagesController(OculusPackageCreator packageCreator)
        {
            this.packageCreator = packageCreator;
        }

        [HttpPost]
        public async Task<FileResult> Create(OculusAppPackageOptions options)
        {
            AnalyticsInfo analyticsInfo = new();
            
            if (HttpContext?.Request.Headers != null)
            {
                analyticsInfo.platformId = HttpContext.Request.Headers.TryGetValue("platform-identifier", out var id) ? id.ToString() : null;
                analyticsInfo.platformIdVersion = HttpContext.Request.Headers.TryGetValue("platform-identifier-version", out var version) ? version.ToString() : null;
                analyticsInfo.correlationId = HttpContext.Request.Headers.TryGetValue("correlation-id", out var corrId) ? corrId.ToString() : null;
                analyticsInfo.referrer = HttpContext.Request.Query.TryGetValue("ref", out var referrer) ? referrer.ToString() : null;
            }
            var validatedOptions = options.Validate();
            var zipFilePath = await packageCreator.Create(validatedOptions, analyticsInfo);
            var downloadFileName = $"{validatedOptions.Name}-Oculus-app.zip";
            return File(System.IO.File.OpenRead(zipFilePath), "application/zip", downloadFileName);
        }
    }
       
}