using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class FooterAboutViewComponent : ViewComponent
    {
        private readonly IPageQueryProvider _pageQueryProvider;

        public FooterAboutViewComponent(IPageQueryProvider pageQueryProvider)
        {
            _pageQueryProvider = pageQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _pageQueryProvider.GetDetailsAsync("about_us");
            return View(result);
        }
    }
}
