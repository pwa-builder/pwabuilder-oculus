using Microsoft.AspNetCore.Mvc;
using Microsoft.PWABuilder.Oculus.Models;

namespace Microsoft.PWABuilder.Oculus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APKController : ControllerBase
    {
        [HttpPost]
        public string GenerateZip(OculusAppPackageOptions options)
        {
            return CreateAppPackage(options);
        }

        private string CreateAppPackage(OculusAppPackageOptions options)
        {
            return "This API was called";
        }
    }
       
}