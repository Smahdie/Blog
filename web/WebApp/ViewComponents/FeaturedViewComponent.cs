using Core.Interfaces.MenuProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class FeaturedViewComponent : ViewComponent
    {
        private readonly IMenuQueryProvider _menuQueryProvider; 
        public FeaturedViewComponent(IMenuQueryProvider menuQueryProvider)
        {
            _menuQueryProvider = menuQueryProvider;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _menuQueryProvider.GetByKeywordAsync("featured");
            return View(data);
        }
    }
}
