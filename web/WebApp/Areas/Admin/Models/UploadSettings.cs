using Pack.FileUpload.Dtos;

namespace Admin.Models
{
    public class UploadSettings
    {
        public FileUploadConfig ContentImage { get; set; }
        public FileUploadConfig SliderImage { get; set; }
    }
}
