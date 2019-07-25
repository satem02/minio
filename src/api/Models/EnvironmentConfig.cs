using System;

namespace api.Models
{
    public class EnvironmentConfig
    {
        public string X_BACKEND_SERVER { get; set; }
        public string MINIO_ENDPOINT { get; set; }
        public string MINIO_ACCESS_KEY { get; set; }
        public string MINIO_SECRET_KEY { get; set; }
    }
}
