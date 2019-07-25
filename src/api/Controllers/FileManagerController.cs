using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class FileManagerController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly EnvironmentConfig _configuration;

        public FileManagerController(IFileUploadService fileUploadService, IOptions<EnvironmentConfig> configuration)
        {
            _fileUploadService = fileUploadService;
            _configuration = configuration.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post(IFormFile file)
        {
            var response = new ServiceResult();
            response.Data = await _fileUploadService.UploadFile(file);
            return Ok(new
            {
                _configuration.X_BACKEND_SERVER,
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
                _configuration.X_BACKEND_SERVER,
                result.Message,
                result.IsSuccess
            });
        }
    }
}
