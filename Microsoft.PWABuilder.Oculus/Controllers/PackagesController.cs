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
            var validatedOptions = options.Validate();
            var zipFilePath = await packageCreator.Create(validatedOptions);
            var downloadFileName = $"{validatedOptions.Name}-Oculus-app.zip";
            return File(System.IO.File.OpenRead(zipFilePath), "application/zip", downloadFileName);
        }
    }
       
}