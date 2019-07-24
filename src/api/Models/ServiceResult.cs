using System;

namespace api.Models
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; } = true;

        public string Message { get; set; } = string.Empty;

        public int StatusCode { get; set; } = 200;

        public object Data { get; set; }
    }
}
