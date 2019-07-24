using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio.Exceptions;
using mvc.Models;
using mvc.Services;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileManager _fileManager;
        private readonly string _bucketName = "deneme";

        public HomeController(IFileManager fileManager)
        {
            _fileManager = fileManager;
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
            string objectName = Guid.NewGuid().ToString().Substring(0, 7) + Path.GetExtension(file.FileName);
            string contentType = file.ContentType;

            try
            {
                //Bu isimde bir bucket olup olmadığını kontrol edelim.
                bool isFound = await _fileManager.BucketExistsAsync(_bucketName);
                if (!isFound)
                {
                    //Bu isimde bir bucket yoksa oluşturalım.
                    await _fileManager.MakeBucketAsync(_bucketName);
                }

                await _fileManager.PutObjectAsync(_bucketName, objectName, stream, stream.Length, contentType);
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

            MemoryStream memoryStream = new MemoryStream();
            try
            {
                //Eğer ilgili bucket altında ismi verilen object yer almıyorsa bu metod bize hata fırlatacaktır.
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
