using System.Collections.Generic;

namespace Component.FileUpload.Dtos
{
    public class FileUploadConfig
    {
        public int MaxKiloBytes { get; set; }
        public List<string> Extensions { get; set; }
        public Address Address { get; set; }
    }
}
