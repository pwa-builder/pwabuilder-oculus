using Microsoft.AspNetCore.Mvc;
using Microsoft.PWABuilder.Oculus.Models;

namespace Microsoft.PWABuilder.Oculus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APKController : ControllerBase
    {
        //private readonly ILogger<APKController> logger;
        [HttpPost]
        public string GenerateZip(OculusAppPackageOptions options)
        {
            return CreateAppPackage(options);
        }

        private string CreateAppPackage(OculusAppPackageOptions options)
        {
            //logger.LogInformation("Generating app package for {url} using {options}", options.Url, options);

            return "This API was called";
        }
    }
       
}