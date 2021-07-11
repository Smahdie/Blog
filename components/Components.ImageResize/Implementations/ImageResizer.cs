using Component.ImageResize.Dtos;
using Component.ImageResize.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Component.ImageResize.Implementations
{
    public class ImageResizer : IImageResizer
    {
        public async Task ResizeImage(string imagePath, List<ImageSizeDto> imageSizes)
        {
            var imageDirectory = Path.GetDirectoryName(imagePath);
            var imageName = Path.GetFileNameWithoutExtension(imagePath);
            var imageExtension = Path.GetExtension(imagePath);

            
            foreach (var size in imageSizes)
            {
                using var image = Image.Load(imagePath);

                var outputPath = Path.Combine(imageDirectory, $"{imageName}_{size.Width}x{size.Height}{imageExtension}");

                image.Mutate(x => x
                     .Resize(size.Width, size.Height));

                await image.SaveAsync(outputPath);
            }
        }
    }
}
