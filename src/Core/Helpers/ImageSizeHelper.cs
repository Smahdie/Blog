using System;
using System.IO;

namespace Core.Helpers
{
    public static class ImageSizeHelper
    {
        public static string GetSize(this string imagePath, string size)
        {            
            var imageName = Path.GetFileName(imagePath);
            var imageNameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
            var imageExtension = Path.GetExtension(imagePath);
            var sizedImageName = $"{imageNameWithoutExtension}_{size}{imageExtension}";
            var imageSizePath = imagePath.Replace(imageName, sizedImageName);
            return imageSizePath;
        }
    }
}
