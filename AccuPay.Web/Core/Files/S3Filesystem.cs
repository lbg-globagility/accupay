using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;

//using Notisphere.Attachments.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Files
{
    /// <summary>
    /// File service used to connect to AWS S3 service.
    /// </summary>
    public class S3Filesystem : IFilesystem
    {
        private readonly Amazon.RegionEndpoint _region = Amazon.RegionEndpoint.USEast2;

        private readonly string _bucketName;

        public S3Filesystem(string bucketName)
        {
            _bucketName = bucketName;
        }

        public async Task<string> Move(IFormFile file, string target)
        {
            ValidatePath(target);

            using var stream = file.OpenReadStream();
            return await Move(stream, target);
        }

        public async Task<string> Move(Stream stream, string target)
        {
            ValidatePath(target);

            using var s3 = new AmazonS3Client(_region);
            using var fileTransferUtility = new TransferUtility(s3);
            await fileTransferUtility.UploadAsync(stream, _bucketName, target);

            return target;
        }

        public async Task<Stream> Get(string location)
        {
            ValidatePath(location);

            using var s3 = new AmazonS3Client(_region);
            // Get S3 object from Amazon S3 using the `location` as the key
            var objectKey = location;
            var s3Object = await s3.GetObjectAsync(_bucketName, objectKey);

            // Return the responseStream, don't close as the caller will be responsible for
            // closing the stream.
            return s3Object.ResponseStream;
        }

        public async Task Remove(string location)
        {
            ValidatePath(location);

            using var s3 = new AmazonS3Client(_region);
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = location
            };

            _ = await s3.DeleteObjectAsync(request);
        }

        private void ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"{nameof(path)} cannot be empty.");
            }
        }
    }
}
