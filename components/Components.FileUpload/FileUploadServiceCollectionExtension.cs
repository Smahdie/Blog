using Microsoft.Extensions.DependencyInjection;
using Component.FileUpload.Implementations;
using Component.FileUpload.Implementations.FileSignatures;
using Component.FileUpload.Interfaces;

namespace FileUpload
{
    public static class FileUploadServiceCollectionExtension
    {
        public static IServiceCollection AddFileUploadServices(this IServiceCollection services)
        {
            services.AddTransient<FileSignature, JpgSignature>();
            services.AddTransient<FileSignature, PngSignature>();

            services.AddTransient<IFileSignatureManager, FileSignatureManager>();
            services.AddTransient<IFileUploadManager, FileUploadManager>();
            services.AddTransient<IValidationManager, ValidationManager>();

            return services;
        }
    }
}
