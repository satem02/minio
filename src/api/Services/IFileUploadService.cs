using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Http;

namespace api.Services
{
    public interface IFileUploadService
    {
        Task<FileResponseModel> DownloadFile(string fileName);
        Task<FileViewModel> UploadFile(IFormFile file);
    }
}
