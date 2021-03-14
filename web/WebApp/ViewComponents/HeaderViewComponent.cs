using Core.Interfaces.MenuProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly IMenuQueryProvider _menuQueryProvider;
        public HeaderViewComponent(IMenuQueryProvider menuQueryProvider)
        {
            _menuQueryProvider = menuQueryProvider;
        }
        public async Task <IViewComponentResult> InvokeAsync()
        {
            var menu = await _menuQueryProvider.GetByKeywordAsync("header", true);
            return View(menu);
        }
    }
}
