using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.Exceptions;
using mvc.Models;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            FileViewModel viewModel = new FileViewModel();

            //Yüklenen dosyanın MemoryStream nesnesini oluşturalım
            MemoryStream stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            //MinIO Bağlantısı
            string endPoint = _config["Minio:Endpoint"];
            string accessKey = _config["Minio:Accesskey"];
            string secretKey = _config["Minio:SecretKey"];
            MinioClient minioClient = new MinioClient(endPoint, accessKey, secretKey);

            string bucketName = "medium";
            string objectName = Guid.NewGuid().ToString().Substring(0, 7) + Path.GetExtension(file.FileName);
            string contentType = file.ContentType;

            try
            {
                //Bu isimde bir bucket olup olmadığını kontrol edelim.
                bool isFound = await minioClient.BucketExistsAsync(bucketName);
                if (!isFound)
                {
                    //Bu isimde bir bucket yoksa oluşturalım.
                    await minioClient.MakeBucketAsync(bucketName);
                }

                await minioClient.PutObjectAsync(bucketName, objectName, stream, stream.Length, contentType);
                viewModel.FileName = objectName;
            }
            catch (MinioException m)
            {
                viewModel.Message = m.message;
            }

            return View(viewModel);
        }

        public async Task<FileResult> DownloadFile(string fileName)
        {
            //MinIO Bağlantısı
            string endPoint = _config["Minio:Endpoint"];
            string accessKey = _config["Minio:Accesskey"];
            string secretKey = _config["Minio:SecretKey"];
            MinioClient minioClient = new MinioClient(endPoint, accessKey, secretKey);

            string bucketName = "deneme";
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                //Eğer ilgili bucket altında ismi verilen object yer almıyorsa bu metod bize hata fırlatacaktır.
                await minioClient.StatObjectAsync(bucketName, fileName);

                await minioClient.GetObjectAsync(bucketName, fileName,
                                    (stream) =>
                                    {
                                        stream.CopyTo(memoryStream);
                                    });
                memoryStream.Position = 0;

            }
            catch
            {

            }

            return File(memoryStream, GetContentType(fileName), fileName);
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
