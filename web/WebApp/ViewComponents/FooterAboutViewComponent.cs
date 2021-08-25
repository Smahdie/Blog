using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class FooterAboutViewComponent : ViewComponent
    {
        private readonly IPageQuery _pageQueryProvider;

        public FooterAboutViewComponent(IPageQuery pageQueryProvider)
        {
            _pageQueryProvider = pageQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.CurrentLanguage();
            var result = await _pageQueryProvider.GetDetailsAsync("about_us", language);
            return View(result);
        }
    }
}
