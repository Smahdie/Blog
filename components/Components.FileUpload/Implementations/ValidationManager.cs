using Microsoft.AspNetCore.Http;
using Component.FileUpload.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Component.FileUpload.Implementations
{
    public class ValidationManager : IValidationManager
    {
        private readonly IFileSignatureManager _fileSignatureManager;

        public ValidationManager(IFileSignatureManager fileSignatureManager)
        {
            _fileSignatureManager = fileSignatureManager;
        }

        public bool IsExtensionAllowed(string fileName, List<string> extensions)
        {
            var extension = Path.GetExtension(fileName).Replace(".", string.Empty);
            return extensions.Any(a => a.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsFileSizeValid(long totalFileSize, long maxKiloBytes)
        {
            var maxAllowedSizeInBytes = maxKiloBytes * 1024;
            return totalFileSize <= maxAllowedSizeInBytes;
        }

        public Task<bool> IsSignatureValid(IFormFile file)
        {
            return IsSignatureValid(file, file.FileName);
        }

        public Task<bool> IsSignatureValid(IFormFile file, string fileName)
        {
            return _fileSignatureManager.Check(file, fileName);
        }
    }
}
