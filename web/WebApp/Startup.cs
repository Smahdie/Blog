using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;

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

            services.Configure<RequestLocalizationOptions>(options =>
            {
                //Part 1: support English and Persian languages
                var supportedCultures = new[] {
                    new CultureInfo("fa"),
                    new CultureInfo("en")
                };
                options.DefaultRequestCulture = new RequestCulture("fa");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // Part 2: introduce a way to detect current culture
                var provider = new RouteValueRequestCultureProvider { Options = options };
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    provider
                };
            });

            //Part 3: Tell the app to look for resource files in the **Resources** folder
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddSingleton<SharedLocalizationService>();

            //Part 4: Add a constraint to routeOptions, to check the culture with **CultureRouteConstraint** class
            services.Configure<RouteOptions>(options => { options.ConstraintMap.Add("culture", typeof(CultureRouteConstraint)); });
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

            //Part 6
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapRazorPages();
               endpoints.MapControllerRoute(
               name: "default",
               pattern: "{controller}/{action}/{id?}");
            });
        }
    }

    
}
