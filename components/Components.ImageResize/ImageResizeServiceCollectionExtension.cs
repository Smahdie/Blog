using Microsoft.Extensions.DependencyInjection;
using Component.ImageResize.Implementations;
using Component.ImageResize.Interfaces;

namespace Component.ImageResize
{
    public static class ImageResizeServiceCollectionExtension
    {
        public static IServiceCollection AddImageResizeServices(this IServiceCollection services)
        {
            services.AddTransient<IImageResizer, ImageResizer>();

            return services;
        }
    }
}
