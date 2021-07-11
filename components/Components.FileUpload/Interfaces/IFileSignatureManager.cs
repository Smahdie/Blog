using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Component.FileUpload.Interfaces
{
    public interface IFileSignatureManager
    {
        Task<bool> Check(IFormFile file);
        Task<bool> Check(IFormFile file, string fileName);
    }
}