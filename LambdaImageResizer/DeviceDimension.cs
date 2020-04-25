using System;
using static LambdaImageResizer.ImageResizer;

namespace LambdaImageResizer
{
    public class DeviceDimension : IDeviceDimension
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public DeviceType DeviceType { get; set; }
    }

    interface IDeviceDimension
    {
        //Just for a supertype
    }
}
