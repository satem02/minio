using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class FileManagerController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public FileManagerController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(IFormFile file)
        {
            var response = new ServiceResult();
            response.Data = await _fileUploadService.UploadFile(file);
            return Ok(new
            {
                response
            });
        }

        [HttpGet("{fileName}")]
        public async Task<ActionResult> Get(string fileName)
        {
            var result = await _fileUploadService.DownloadFile(fileName);
            if (result.IsSuccess)
                return File(result.MemoryStream, result.ContentType, result.FileName);
            return Ok(new
            {
                result.Message,
                result.IsSuccess
            });
        }
    }
}
