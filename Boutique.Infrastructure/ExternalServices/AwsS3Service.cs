using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.ExternalServices {
    public class AwsS3Service {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public AwsS3Service(IAmazonS3 s3Client,IConfiguration configuration) {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:BucketName"];
        }

        public async Task<string> UploadFileAsync(Stream fileStream,string fileName,string contentType) {
            var putRequest = new PutObjectRequest {
                BucketName = "boutiqueawsbucket", // bucket adınızı buraya yazın
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType,
                AutoCloseStream = true
            };

            var response = await _s3Client.PutObjectAsync(putRequest);
            return $"https://{putRequest.BucketName}.s3.{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{fileName}";
        }
    }
}
