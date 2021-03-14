using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class FeaturedCardViewComponent : ViewComponent
    {
        private readonly IPageQueryProvider _pageQueryProvider; 
        public FeaturedCardViewComponent(IPageQueryProvider pageQueryProvider)
        {
            _pageQueryProvider = pageQueryProvider;
        }
        public async Task<IViewComponentResult> InvokeAsync(string keyword)
        {
            var data = await _pageQueryProvider.GetDetailsAsync(keyword);
            return View(data);
        }
    }
}
