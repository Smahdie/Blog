using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Component.FileUpload.Dtos
{
    public class FileUploadRequestDto
    {
        public int MaxKiloBytes { get; set; }
        public List<string> Extensions { get; set; }
        public Address Address { get; set; }
        public IFormFile File { get; set; }
        public string MetaData { get; set; }
    }
}
