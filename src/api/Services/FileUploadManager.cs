using System;
using System.IO;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Minio.Exceptions;

namespace api.Services
{
    public class FileUploadManager : IFileUploadService
    {
        private readonly string _bucketName = "deneme";
        private readonly IFileService _fileManager;
        private readonly ILogger<FileUploadManager> _logger;

        public FileUploadManager(IFileService fileManager, ILogger<FileUploadManager> logger)
        {
            _logger = logger;
            _fileManager = fileManager;
        }

        public async Task<FileResponseModel> DownloadFile(string fileName)
        {
            var response = new FileResponseModel();
            var memoryStream = new MemoryStream();

            try
            {
                await _fileManager.StatObjectAsync(_bucketName, fileName);
                await _fileManager.GetObjectAsync(_bucketName, fileName,
                                    (stream) =>
                                    {
                                        stream.CopyTo(memoryStream);
                                    });
                memoryStream.Position = 0;

            }
            catch
            {
                response.IsSuccess = false;
                response.Message = "Dosya bulunamadi.";
            }

            response.ContentType = GetContentType(fileName);
            response.FileName = fileName;
            response.MemoryStream = memoryStream;
            return response;
        }

        public async Task<FileViewModel> UploadFile(IFormFile file)
        {
            var response = new FileViewModel();
            var stream = new MemoryStream();

            file.CopyTo(stream);
            stream.Position = 0;
            string objectName = Guid.NewGuid().ToString().Substring(0, 7) + Path.GetExtension(file.FileName);
            string contentType = file.ContentType;
            _logger.LogInformation($"UploadFileName : {objectName}");
            try
            {
                bool isFound = await _fileManager.BucketExistsAsync(_bucketName);
                if (!isFound)
                {
                    await _fileManager.MakeBucketAsync(_bucketName);
                }
                await _fileManager.PutObjectAsync(_bucketName, objectName, stream, stream.Length, contentType);
                response.FileName = objectName;
            }
            catch (MinioException m)
            {
                response.Message = m.message;
            }
            _logger.LogInformation($"UploadFileName Success : {objectName}");

            return response;
        }

        string GetContentType(string fileName)
        {
            if (fileName.Contains(".jpg"))
            {
                return "image/jpg";
            }
            else if (fileName.Contains(".jpeg"))
            {
                return "image/jpeg";
            }
            else if (fileName.Contains(".png"))
            {
                return "image/png";
            }
            else if (fileName.Contains(".gif"))
            {
                return "image/gif";
            }
            else if (fileName.Contains(".pdf"))
            {
                return "application/pdf";
            }
            else
            {
                return "application/octet-stream";
            }
        }
    }
}
