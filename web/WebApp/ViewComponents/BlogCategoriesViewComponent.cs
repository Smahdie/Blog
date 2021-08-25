using Core.Dtos.Settings;
using Core.Interfaces.CategoryProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class BlogCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryQuery _categoryQueryProvider;
        private readonly WebAppSettings _appSettings;

        public BlogCategoriesViewComponent(
            ICategoryQuery categoryQueryProvider,
            IOptions<WebAppSettings> appSettings)
        {
            _categoryQueryProvider = categoryQueryProvider;
            _appSettings = appSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.CurrentLanguage();
            var categories = await _categoryQueryProvider.GetListAsync(_appSettings.BlogCategoryId, language);
            return View(categories);
        }
    }
}