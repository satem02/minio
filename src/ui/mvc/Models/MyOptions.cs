namespace mvc.Models
{
    public class MyOptions
    {
        public MinioOptions Minio { get; set; }
    }

    public class MinioOptions
    {
        public string Endpoint { get; set; }
        public string Accesskey { get; set; }
        public string Secretkey { get; set; }

    }
}
