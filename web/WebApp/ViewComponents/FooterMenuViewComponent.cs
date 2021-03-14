using Core.Interfaces.MenuProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class FooterMenuViewComponent : ViewComponent
    {
        private readonly IMenuQueryProvider _menuQueryProvider;
        public FooterMenuViewComponent(IMenuQueryProvider menuQueryProvider)
        {
            _menuQueryProvider = menuQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(string keyword)
        {
            var menu = await _menuQueryProvider.GetByKeywordAsync(keyword);
            return View(menu);
        }
    }
}
