using Boutique.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boutique.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CloudStorageController:ControllerBase {
        private AwsS3Service _awsS3Service;

        public CloudStorageController(AwsS3Service awsS3Service) {
            _awsS3Service = awsS3Service;
        }

        [HttpPost("upload-file")]
        [Authorize]
        public async Task<IActionResult> FileUpload(IFormFile file) {
            if(file == null || file.Length == 0) {
                return BadRequest("File is not selected or empty.");
            }

            var fileName = file.FileName;
            var contentType = file.ContentType;

            using(var fileStream = file.OpenReadStream()) {
                var link = await _awsS3Service.UploadFileAsync(fileStream,fileName,contentType);
                return Ok(new { Link = link });
            }
        }
    }
}
