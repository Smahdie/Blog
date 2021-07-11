namespace Component.FileUpload.Dtos
{
    public class FileUploadResultDto
    {
        public bool Succeeded { get; protected set; }

        public string ErrorMessage { get; set; }

        public string FileUrl { get; set; }

        public string FilePath { get; set; }

        public static FileUploadResultDto Success(string fileUrl, string filePath) => new FileUploadResultDto()
        {
            FileUrl = fileUrl,
            FilePath = filePath,
            Succeeded = true
        };

        public static FileUploadResultDto InvalidFileSize => Error("حجم فایل مجاز نیست");

        public static FileUploadResultDto InvalidFileExtension => Error("فرمت فایل مجاز نیست");

        private static FileUploadResultDto Error(string error) => new FileUploadResultDto()
        {
            ErrorMessage = error
        };
    }
}
