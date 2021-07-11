using Microsoft.AspNetCore.Http;
using Component.FileUpload.Implementations.FileSignatures;
using Component.FileUpload.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Component.FileUpload.Implementations
{
    internal class FileSignatureManager : IFileSignatureManager
    {
        private readonly IEnumerable<FileSignature> _fileSignatures;

        public FileSignatureManager(IEnumerable<FileSignature> fileSignatures)
        {
            _fileSignatures = fileSignatures;
        }

        public Task<bool> Check(IFormFile file)
        {
            return Check(file, file.FileName);
        }

        public Task<bool> Check(IFormFile file, string fileName)
        {
            var extension = Path.GetExtension(fileName).Replace(".", string.Empty);

            var signature = _fileSignatures.FirstOrDefault(s => s.Extensions
                                           .Any(e => e.Equals(extension, StringComparison.OrdinalIgnoreCase)));

            if (signature is null)
            {
                return Task.FromResult(false);
            }

            return signature.Check(file);
        }
    }
}
