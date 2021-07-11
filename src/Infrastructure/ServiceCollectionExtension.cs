using Core.Dtos.EmailDtos;
using Core.Dtos.Settings;
using Core.Interfaces.CaptchaProviders;
using Core.Interfaces.CategoryProviders;
using Core.Interfaces.ContactInfoProviders;
using Core.Interfaces.ContentProviders;
using Core.Interfaces.EmailProviders;
using Core.Interfaces.LanguageProviders;
using Core.Interfaces.MenuProviders;
using Core.Interfaces.MessageProviders;
using Core.Interfaces.PageProviders;
using Core.Interfaces.SliderProviders;
using Core.Interfaces.TagProviders;
using Core.Interfaces.UserProviders;
using Infrastructure.Services.CaptchaProviders;
using Infrastructure.Services.CategoryProviders;
using Infrastructure.Services.ContactInfoProviders;
using Infrastructure.Services.ContentProviders;
using Infrastructure.Services.EmailProviders;
using Infrastructure.Services.LanguageProviders;
using Infrastructure.Services.MenuProviders;
using Infrastructure.Services.MessageProviders;
using Infrastructure.Services.PageProviders;
using Infrastructure.Services.SliderProviders;
using Infrastructure.Services.TagProviders;
using Infrastructure.Services.UserProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPanelServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpClient();

            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IContactInfoManager, ContactInfoManager>();
            services.AddScoped<IMessageManager, MessageManager>();
            services.AddScoped<IContentManager, ContentManager>();
            services.AddScoped<IMenuManager, MenuManager>();
            services.AddScoped<IPageManager, PageManager>();
            services.AddScoped<ISliderManager, SliderManager>();
            services.AddScoped<ITagManager, TagManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ILanguageManager, LanguageManager>();

            services.Configure<PanelAppSettings>(configuration.GetSection("PanelAppSetting"));
            services.Configure<UploadAddresses>(configuration.GetSection("UploadAddresses"));

            return services;
        }

        public static IServiceCollection AddWebAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMemoryCache();

            services.AddScoped<ICategoryQueryProvider, CategoryQueryProvider>();
            services.AddScoped<ICaptchaManager, CaptchaManager>();
            services.AddScoped<IContactInfoQueryProvider, ContactInfoQueryProvider>();
            services.AddScoped<IContentCommandProvider, ContentCommandProvider>();
            services.AddScoped<IContentQueryProvider, ContentQueryProvider>();
            services.AddScoped<IMenuQueryProvider, MenuQueryProvider>();
            services.AddScoped<IMessageCommandProvider, MessageCommandProvider>();
            services.AddScoped<IPageQueryProvider, PageQueryProvider>();
            services.AddScoped<ISliderQueryProvider, SliderQueryProvider>();
            services.AddScoped<ITagQueryProvider, TagQueryProvider>();            

            services.Configure<WebAppSettings>(configuration.GetSection("WebAppSettings"));
            services.Configure<MemoryCacheSettings>(configuration.GetSection("MemoryCache"));
            services.Configure<MailManagerSettings>(configuration.GetSection("MailManager"));
            services.Configure<GoogleReCaptchaSettings>(configuration.GetSection("GoogleReCaptcha"));            

            return services;
        }

        public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["Email:EmailProvider"] == "MailKit")
            {
                services.Configure<MailKitServerSettings>(configuration.GetSection("Email:MailKit"));
                services.AddSingleton<IEmailManager, MailKitEmailManager>();
            }
            else
            {
                services.AddSingleton<IEmailManager, EmptyEmailManager>();
            }

            return services;
        }

    }
}
