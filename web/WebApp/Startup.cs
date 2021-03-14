using Admin.Services.Mail;
using Admin.Services.Profile;
using Core.Models.Manager;
using FileUpload;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pack.ImageResize;
using Admin.Models;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigurePanelServices(Environment, Configuration);

            services.ConfigureUploadServices(Environment, Configuration);
            
            services.AddWebAppServices(Configuration);

            services.AddEmailServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
               name: "default",
               pattern: "{controller}/{action}/{id?}");
            });
        }
    }

    public static class IServiceCollectionExtensions
    {
        public static void ConfigurePanelServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddIdentity<Manager, Role>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
                config.SignIn.RequireConfirmedEmail = false;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                 opt =>
                 {
                     opt.LoginPath = "/Admin/Account/Login";
                 });


            var mvcBuilder = services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Admin", "/");
                    options.Conventions.AllowAnonymousToPage("/Error");
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Account/AccessDenied");
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Account/ForgotPassword");
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Account/ForgotPasswordConfirmation");
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Account/Lockout");
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Account/Login");
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Account/ResetPassword");
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Account/ResetPasswordConfirmation");
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Account/SignedOut");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            if (environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.Configure<MailManagerOptions>(configuration.GetSection("Email"));

            if (configuration["Email:EmailProvider"] == "SendGrid")
            {
                services.Configure<SendGridAuthOptions>(configuration.GetSection("Email:SendGrid"));
                services.AddSingleton<IMailManager, SendGridMailManager>();
            }
            else
            {
                services.AddSingleton<IMailManager, EmptyMailManager>();
            }

            services.AddScoped<ProfileManager>();

            services.AddPanelServices(configuration);
        }

        public static void ConfigureUploadServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            services.Configure<UploadSettings>(configuration.GetSection("Upload"));
            services.Configure<ImageResizeSettings>(configuration.GetSection("ImageResize"));

            services.AddImageResizeServices();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(AllowSpecificOrigins,
            //        builder => builder.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>())
            //                          .AllowAnyHeader()
            //                          .AllowAnyMethod()
            //                          .AllowCredentials());
            //});

            //services.Configure<FormOptions>(x =>
            //{
            //    x.ValueLengthLimit = int.MaxValue;
            //    x.MultipartBodyLengthLimit = int.MaxValue;
            //    x.MultipartHeadersLengthLimit = int.MaxValue;
            //});

            services.AddFileUploadServices();

            //if (!environment.IsDevelopment())
            //{
            //    services.AddAuthentication(options =>
            //    {
            //        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    })
            //    .AddCookie(options =>
            //    {
            //        if (!environment.IsDevelopment())
            //        {
            //            configuration.GetSection("CookieAuthentication").Bind(options);
            //        }
            //    });
            //}
        }
    }
}
