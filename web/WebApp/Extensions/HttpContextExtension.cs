using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace WebApp.Extensions
{
    public static class HttpContextExtension
    {
        public static string CurrentLanguage(this HttpContext httpContext)
        {
            var feature = httpContext.Features.Get<IRequestCultureFeature>();
            return feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
        }
    }
}
