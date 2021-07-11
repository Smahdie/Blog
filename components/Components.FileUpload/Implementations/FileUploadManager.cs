using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Component.FileUpload.Dtos;
using Component.FileUpload.Helpers;
using Component.FileUpload.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Component.FileUpload.Implementations
{
    internal class FileUploadManager : IFileUploadManager
    {
        private readonly IValidationManager _validationManager;
        private IWebHostEnvironment _environment;

        public FileUploadManager(
            IValidationManager validationManager,
            IWebHostEnvironment environment)
        {
            _validationManager = validationManager;
            _environment = environment;
        }

        public Task<FileUploadResultDto> Save(IFormFile file, FileUploadConfig config)
        {
            var fileUploadRequest = new FileUploadRequestDto
            {
                File = file,
                Address = config.Address,
                Extensions = config.Extensions,
                MaxKiloBytes = config.MaxKiloBytes
            };

            return Save(fileUploadRequest);
        }

        public async Task<FileUploadResultDto> Save(FileUploadRequestDto request)
        {
            if (!_validationManager.IsFileSizeValid(request.File.Length, request.MaxKiloBytes))
            {
                return FileUploadResultDto.InvalidFileSize;
            }

            if (!_validationManager.IsExtensionAllowed(request.File.FileName, request.Extensions))
            {
                return FileUploadResultDto.InvalidFileExtension;
            }

            if (!await _validationManager.IsSignatureValid(request.File))
            {
                return FileUploadResultDto.InvalidFileExtension;
            }

            (var relativePath, var fullPath) = FileHelper.CreateFilePath(_environment.WebRootPath, request.Address.Folder, request.File.FileName);

            await FileHelper.SaveFile(fullPath, request.File);

            return FileUploadResultDto.Success(relativePath.Replace('\\','/'), fullPath);
        }
    }
}