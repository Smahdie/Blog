using Core.Interfaces.MenuProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IMenuQuery _menuQueryProvider;

        public HeaderViewComponent(IMenuQuery menuQueryProvider)
        {
            _menuQueryProvider = menuQueryProvider;
        }
        public async Task <IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.CurrentLanguage();
            var menu = await _menuQueryProvider.GetByKeywordAsync("header", language, true);
            return View(menu);
        }
    }
}
