using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class FeaturedCardViewComponent : ViewComponent
    {
        private readonly IPageQuery _pageQueryProvider; 

        public FeaturedCardViewComponent(IPageQuery pageQueryProvider)
        {
            _pageQueryProvider = pageQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(string keyword)
        {
            var language = HttpContext.CurrentLanguage();
            var data = await _pageQueryProvider.GetDetailsAsync(keyword, language);
            return View(data);
        }
    }
}
