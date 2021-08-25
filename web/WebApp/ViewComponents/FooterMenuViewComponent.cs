using Core.Interfaces.MenuProviders;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class FooterMenuViewComponent : ViewComponent
    {
        private readonly IMenuQuery _menuQueryProvider;

        public FooterMenuViewComponent(IMenuQuery menuQueryProvider)
        {
            _menuQueryProvider = menuQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(string keyword)
        {
            var language = HttpContext.CurrentLanguage();

            var menu = await _menuQueryProvider.GetByKeywordAsync(keyword, language);
            return View(menu);
        }
    }
}
