using Core.Interfaces.MenuProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class FeaturedViewComponent : ViewComponent
    {
        private readonly IMenuQuery _menuQueryProvider; 
        public FeaturedViewComponent(IMenuQuery menuQueryProvider)
        {
            _menuQueryProvider = menuQueryProvider;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.CurrentLanguage();
            var data = await _menuQueryProvider.GetByKeywordAsync("featured", language);
            return View(data);
        }
    }
}
