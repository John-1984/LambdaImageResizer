using System;
using System.IO;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;

namespace LambdaImageResizer
{
    /// <summary>
    /// Image loader.
    /// </summary>
    public class ImageLoader
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public void LoadImageToS3(S3Event s3Event, ILambdaContext lambdaContext)
        {
            try
            {
                Console.WriteLine("LoadImageToS3:" + s3Event.Records[0].EventName + " " + s3Event.Records[0].S3.Object.Key);
                if (s3Event.Records[0].EventName.ToString().Contains("ObjectCreated"))
                {
                    ObjectCreatedEventHandler(s3Event.Records[0].S3.Bucket.Name, s3Event.Records[0].S3.Object.Key);
                }
                else if (s3Event.Records[0].EventName.ToString().Contains("ObjectRemoved"))
                {
                    ObjectRemovedEventHandler(s3Event.Records[0].S3.Bucket.Name, s3Event.Records[0].S3.Object.Key);
                }
                Console.WriteLine("LoadImageToS3:" + " End of Image Loader.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in LoadImageToS3:" + ex.Message);
            }
        }

        private void ObjectRemovedEventHandler(string bucketName, string keyName) {
            try
            {
                var AWSS3Actions = new AWSS3Actions();
                AWSS3Actions.DeletingingObjectAsync(bucketName, keyName.Replace("base.Images", "d.Images")).Wait();
                AWSS3Actions.DeletingingObjectAsync(bucketName, keyName.Replace("base.Images", "t.Images")).Wait();
                AWSS3Actions.DeletingingObjectAsync(bucketName, keyName.Replace("base.Images", "m.Images")).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ObjectRemovedEventHandler:" + ex.Message);
            }
        }

        private void ObjectCreatedEventHandler(string bucketName, string keyName)
        {
            try
            {
                var AWSS3Actions = new AWSS3Actions();
                Stream imageStream = new MemoryStream();

                //Read Image from S3 Bucket
                Console.WriteLine("LoadImageToS3:" + " Reading Image.");
                using (imageStream = AWSS3Actions.ReadingObjectAsync(bucketName, keyName).Result)
                {
                    //Resize to Desktop and Upload
                    Console.WriteLine("LoadImageToS3:" + " Upload Resized Desktop Image.");
                    var desktopImageStream = ImageResizer.ResizeImage(ImageResizer.DeviceType.Desktop, imageStream);
                    AWSS3Actions.PuttingObjectAsync(bucketName, keyName.Replace("base.Images", "d.Images"), desktopImageStream).Wait();

                    //Resize to Tablet and Upload
                    Console.WriteLine("LoadImageToS3:" + " Upload Resized Tablet Image.");
                    var tabletImageStream = ImageResizer.ResizeImage(ImageResizer.DeviceType.Tablet, imageStream);
                    AWSS3Actions.PuttingObjectAsync(bucketName, keyName.Replace("base.Images", "t.Images"), tabletImageStream).Wait();

                    //Resize to Mobile and Upload
                    Console.WriteLine("LoadImageToS3:" + " Upload Resized Mobile Image.");
                    var mobileImageStream = ImageResizer.ResizeImage(ImageResizer.DeviceType.Mobile, imageStream);
                    AWSS3Actions.PuttingObjectAsync(bucketName, keyName.Replace("base.Images", "m.Images"), mobileImageStream).Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ObjectCreatedEventHandler:" + ex.Message);
            }
        }

        public void ImageTest(string bucketName, string keyName) {
            var AWSS3Actions = new AWSS3Actions();
            Stream imageStream = null;

            //Read Image from S3 Bucket
            using (imageStream = AWSS3Actions.ReadingObjectAsync(bucketName, keyName).Result)
            {
                Console.WriteLine("ImageStream Length:" + imageStream.Length.ToString());

                //Resize to Desktop and Upload
                var desktopImageStream = ImageResizer.ResizeImage(ImageResizer.DeviceType.Desktop, imageStream);
                AWSS3Actions.PuttingObjectAsync(bucketName, keyName.Replace("base.Images", "d.Images"), desktopImageStream).Wait();

                //Resize to Tablet and Upload
                var tabletImageStream = ImageResizer.ResizeImage(ImageResizer.DeviceType.Tablet, imageStream);
                AWSS3Actions.PuttingObjectAsync(bucketName, keyName.Replace("base.Images", "t.Images"), tabletImageStream).Wait();

                //Resize to Mobile and Upload
                var mobileImageStream = ImageResizer.ResizeImage(ImageResizer.DeviceType.Mobile, imageStream);
                AWSS3Actions.PuttingObjectAsync(bucketName, keyName.Replace("base.Images", "m.Images"), mobileImageStream).Wait();
            }
        }
    }
}
