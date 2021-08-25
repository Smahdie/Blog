using Microsoft.Extensions.Localization;
using System.Reflection;

namespace WebApp
{
    public class SharedLocalizationService
    {
        private readonly IStringLocalizer localizer;
        public SharedLocalizationService(IStringLocalizerFactory factory)
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            localizer = factory.Create(nameof(SharedResource), assemblyName.Name);
        }

        public string Get(string key)
        {
            return localizer[key];
        }
    }
}
