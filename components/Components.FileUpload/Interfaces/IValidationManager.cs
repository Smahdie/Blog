using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Component.FileUpload.Interfaces
{
    public interface IValidationManager
    {
        bool IsFileSizeValid(long totalFileSize, long maxMegaBytes);

        bool IsExtensionAllowed(string fileName, List<string> extensions);

        Task<bool> IsSignatureValid(IFormFile file);

        Task<bool> IsSignatureValid(IFormFile file, string fileName);
    }
}
