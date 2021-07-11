using Microsoft.AspNetCore.Http;
using Component.FileUpload.Dtos;
using System.Threading.Tasks;

namespace Component.FileUpload.Interfaces
{
    public interface IFileUploadManager
    {
        Task<FileUploadResultDto> Save(IFormFile file, FileUploadConfig config);

        Task<FileUploadResultDto> Save(FileUploadRequestDto request);
    }
}
