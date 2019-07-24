using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using mvc.Models;

namespace mvc.Services
{
    public class FileManager : IFileManager
    {
        private readonly MinioClient _client;

        public FileManager(IOptions<MyOptions> options)
        {
            var minioOption = options.Value.Minio;
            _client = new MinioClient(minioOption.Endpoint, minioOption.Accesskey, minioOption.Secretkey);
        }

        public Task<bool> BucketExistsAsync(string bucketName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.BucketExistsAsync(bucketName);
        }

        public Task GetObjectAsync(string bucketName, string objectName, Action<Stream> cb, ServerSideEncryption sse = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.GetObjectAsync(bucketName, objectName, cb, sse);
        }

        public Task GetObjectAsync(string bucketName, string objectName, long offset, long length, Action<Stream> cb, ServerSideEncryption sse = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.GetObjectAsync(bucketName, objectName, offset, length, cb, sse, cancellationToken);
        }

        public Task GetObjectAsync(string bucketName, string objectName, string fileName, ServerSideEncryption sse = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.GetObjectAsync(bucketName, objectName, fileName, sse, cancellationToken);
        }

        public Task MakeBucketAsync(string bucketName, string location = "us-east-1", CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.MakeBucketAsync(bucketName, location, cancellationToken);
        }

        public Task PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType = null, Dictionary<string, string> metaData = null, ServerSideEncryption sse = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.PutObjectAsync(bucketName, objectName, data, size, contentType, metaData, sse, cancellationToken);
        }

        public Task PutObjectAsync(string bucketName, string objectName, string fileName, string contentType = null, Dictionary<string, string> metaData = null, ServerSideEncryption sse = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.PutObjectAsync(bucketName, objectName, fileName, contentType, metaData, sse, cancellationToken);
        }

        public Task<ObjectStat> StatObjectAsync(string bucketName, string objectName, ServerSideEncryption sse = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.StatObjectAsync(bucketName, objectName, sse, cancellationToken);
        }
    }
}
