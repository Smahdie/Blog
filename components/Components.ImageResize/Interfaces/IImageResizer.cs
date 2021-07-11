using Component.ImageResize.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Component.ImageResize.Interfaces
{
    public interface IImageResizer
    {
        Task ResizeImage(string imagePath, List<ImageSizeDto> imageSizes);
    }
}
