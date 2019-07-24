using System;
using System.IO;

namespace api.Models
{
    public class FileResponseModel
    {
        public MemoryStream MemoryStream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "Islem Basarili.";

    }
}
