using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [ApiController]
    //[Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/files")]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider fileExtentionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtentionContentTypeProvider)
        {
            this.fileExtentionContentTypeProvider = fileExtentionContentTypeProvider?? throw new System.ArgumentNullException(nameof(fileExtentionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
       public ActionResult GetFile(string fileId)
        {
            string filePath = "develop-a-system-not-a-goal.pdf";
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            if (!fileExtentionContentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }
    }
}
