using Core.Dtos.Settings;
using Core.Interfaces.CategoryProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class BlogCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryQueryProvider _categoryQueryProvider;
        private readonly WebAppSettings _appSettings;
        public BlogCategoriesViewComponent(
            ICategoryQueryProvider categoryQueryProvider,
            IOptions<WebAppSettings> appSettings)
        {
            _categoryQueryProvider = categoryQueryProvider;
            _appSettings = appSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _categoryQueryProvider.GetListAsync(_appSettings.BlogCategoryId);
            return View(tags);
        }
    }
}