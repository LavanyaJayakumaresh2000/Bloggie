using System.Net;
using System.Threading.Tasks;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUploadRepository uploadRepository;

        public ImageController(IUploadRepository uploadRepository)
        {
            this.uploadRepository = uploadRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var imageUrl = await uploadRepository.UploadAsync(file);
            if (imageUrl == null)
            {
                return Problem("Something Went Wrong", HttpStatusCode.BadRequest.ToString());
            }
            return new JsonResult(imageUrl);
        }
    }
}
