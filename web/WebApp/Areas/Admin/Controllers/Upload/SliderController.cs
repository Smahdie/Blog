using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pack.FileUpload.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;

namespace Admin.Controllers.Upload
{
    [Route("{area}/Upload/{controller}/{action=Index}")]
    public class SliderController : Controller
    {
        private readonly IFileUploadManager _fileUploadManager;
        private readonly UploadSettings _uploadConfig;

        public SliderController(
            IFileUploadManager fileUploadManager,
            IOptions<UploadSettings> uploadConfig)
        {
            _fileUploadManager = fileUploadManager;
            _uploadConfig = uploadConfig.Value;
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

            var fileUploadResult = await _fileUploadManager.Save(file, _uploadConfig.SliderImage);


            if (!fileUploadResult.Succeeded)
            {
                return BadRequest(new
                {
                    Message = fileUploadResult.ErrorMessage
                });
            }

            return Ok(fileUploadResult.FileUrl);
        }
    }
}
