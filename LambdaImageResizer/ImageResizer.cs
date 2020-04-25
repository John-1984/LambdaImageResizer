using System;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace LambdaImageResizer
{
    public static class ImageResizer
    {
        public enum DeviceType
        {
            Desktop = 1,
            Tablet = 2,
            Mobile = 3
        }

        public static Stream ResizeImage(DeviceType deviceType, Stream imageStream)
        {
            try
            {
                Console.WriteLine("Input Stream Seek: " + imageStream.CanSeek + ". Length: " + imageStream.Length);
                imageStream.Position = 0;
                MemoryStream memoryStream = new MemoryStream();

                Console.WriteLine("Memory Stream Seek: " + memoryStream.CanSeek + ". Length: " + memoryStream.Length);

                var deviceDimension = GetDeviceDimensions(deviceType);
                using (var image = Image.Load(imageStream, new PngDecoder()))
                {
                    image.Mutate(ctx => ctx.Resize(deviceDimension.Width, deviceDimension.Height));
                    image.Save(memoryStream, new PngEncoder());
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    Console.WriteLine("Memory Output Stream Seek: " + memoryStream.CanSeek + ". Length: " + memoryStream.Length);
                    return memoryStream;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when ResizeImage an object", e.Message);
            }
            return null;
        }

        private static DeviceDimension GetDeviceDimensions(DeviceType deviceType)
        {
            //Set the Values from AppSettings File
            DeviceDimension deviceDimension = null;
            switch (deviceType)
            {
                case DeviceType.Desktop:
                    deviceDimension = new DeviceDimension() { Height = 1080, Width = 1920, DeviceType = DeviceType.Desktop };
                    break;
                case DeviceType.Tablet:
                    deviceDimension = new DeviceDimension() { Height = 900, Width = 1440, DeviceType = DeviceType.Tablet };
                    break;
                default:
                    deviceDimension = new DeviceDimension() { Height = 768, Width = 1366, DeviceType = DeviceType.Mobile };
                    break;
            }

            return deviceDimension;
        }
    }
}
