using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Component.FileUpload.Interfaces;
using Component.ImageResize.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;

namespace Admin.Controllers.Upload
{
    [Route("{area}/Upload/{controller}/{action}")]
    public class ContentController : Controller
    {
        private readonly IFileUploadManager _fileUploadManager;
        private readonly IImageResizer _imageResizer;
        private readonly UploadSettings _uploadSettings;
        private readonly ImageResizeSettings _imageResizeSettings;

        public ContentController(
            IFileUploadManager fileUploadManager,
            IImageResizer imageResizer,
            IOptions<UploadSettings> uploadSettings,
            IOptions<ImageResizeSettings> imageResizeSettings)
        {
            _fileUploadManager = fileUploadManager;
            _imageResizer = imageResizer;
            _uploadSettings = uploadSettings.Value;
            _imageResizeSettings = imageResizeSettings.Value;
        }

        public IActionResult Index()
        {
            return Content("Hi");
        }

        public async Task<IActionResult> SaveImage()
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file is null)
            {
                return BadRequest(new
                {
                    Message = "فایلی پیدا نشد"
                });
            }

            var fileUploadResult = await _fileUploadManager.Save(file, _uploadSettings.ContentImage);


            if (!fileUploadResult.Succeeded)
            {
                return BadRequest(new
                {
                    Message = fileUploadResult.ErrorMessage
                });
            }

            await _imageResizer.ResizeImage(fileUploadResult.FilePath, _imageResizeSettings.Content);

            return Ok(fileUploadResult.FileUrl);
        }
    }
}
