using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace LambdaImageResizer
{
    public class AWSS3Actions
    {
        /// <summary>
        /// Copyings the object async.
        /// </summary>
        /// <returns>The object async.</returns>
        /// <param name="sourceBucketName">Source bucket name.</param>
        /// <param name="sourceKey">Source key.</param>
        /// <param name="destinationBucketName">Destination bucket name.</param>
        /// <param name="destinationKey">Destination key.</param>
        public async Task CopyingObjectAsync(string sourceBucketName, string sourceKey, string destinationBucketName, string destinationKey)
        {
            try
            {
                using (var client = new AmazonS3Client(Amazon.RegionEndpoint.APSouth1))
                {
                    var request = new CopyObjectRequest
                    {
                        SourceBucket = sourceBucketName,
                        SourceKey = sourceKey,
                        DestinationBucket = destinationBucketName,
                        DestinationKey = destinationKey
                    };
                    var response = await client.CopyObjectAsync(request);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when CopyObjectAsync an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when CopyObjectAsync an object", e.Message);
            }
        }

        /// <summary>
        /// Puttings the object async.
        /// </summary>
        /// <returns>The object async.</returns>
        /// <param name="bucketName">Bucket name.</param>
        /// <param name="keyName">Key name.</param>
        /// <param name="imageStream">Image stream.</param>
        public async Task PuttingObjectAsync(string bucketName, string keyName, Stream imageStream)
        {
            try
            {
                using (var client = new AmazonS3Client(Amazon.RegionEndpoint.APSouth1))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                        InputStream = imageStream
                    };
                    var response = await client.PutObjectAsync(request);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when PuttingObjectAsync an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when PuttingObjectAsync an object", e.Message);
            }
        }

        /// <summary>
        /// Deletingings the object async.
        /// </summary>
        /// <returns>The object async.</returns>
        /// <param name="bucketName">Bucket name.</param>
        /// <param name="keyName">Key name.</param>
        public async Task DeletingingObjectAsync(string bucketName, string keyName)
        {
            try
            {
                using (var client = new AmazonS3Client(Amazon.RegionEndpoint.APSouth1))
                {
                    var request = new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                    };
                    var response = await client.DeleteObjectAsync(request);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when DeletingingObjectAsync an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when DeletingingObjectAsync an object", e.Message);
            }
        }

        /// <summary>
        /// Readings the object async.
        /// </summary>
        /// <returns>The object async.</returns>
        /// <param name="bucketName">Bucket name.</param>
        /// <param name="keyName">Key name.</param>
        public async Task<Stream> ReadingObjectAsync(string bucketName, string keyName)
        {
            MemoryStream imageStream = new MemoryStream();
            try
            {
                using (var client = new AmazonS3Client(Amazon.RegionEndpoint.APSouth1))
                {
                    GetObjectRequest request = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName
                    };
                    using (GetObjectResponse response = await client.GetObjectAsync(request))
                    {
                        var tempImageStream = response.ResponseStream;
                        tempImageStream.CopyTo(imageStream);
                        imageStream.Position = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when ReadObjectData an object", e.Message);
            }

            return imageStream;
        }
    }
}
