using Canducci.Pagination;
using Core.Dtos.ContentDtos;
using Core.Dtos.Settings;
using Core.Interfaces.CategoryProviders;
using Core.Interfaces.ContentProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace WebApp.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly IContentQueryProvider _contentQueryProvider;
        private readonly ICategoryQueryProvider _categoryQueryProvider;
        private readonly WebAppSettings _appSettings;

        public IndexModel(
            IContentQueryProvider contentQueryProvider,
            ICategoryQueryProvider categoryQueryProvider,
            IOptions<WebAppSettings> appSettings)
        {
            _contentQueryProvider = contentQueryProvider;
            _categoryQueryProvider = categoryQueryProvider;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<ContentListDto> Contents { get; set; }
        public int? RequestId { get; set; }
        public string RequestSlug { get; set; }
        public string CategoryName { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNumber, int id, string slug)
        {
            var category = await _categoryQueryProvider.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            if (slug == null || category.Slug != slug)
            {
                return RedirectToPage("Index", new { pageNumber, category.Id, category.Slug });
            }
            CategoryName = category.Name;

            ViewData["Title"] = $"دسته {category.Name} - صفحه {pageNumber}";

            var (Items, TotalCount) = await _contentQueryProvider.GetAllAsync(new ContentListRequestDto { Type = ContentType.Article, PageIndex = pageNumber, CategoryId = category.Id});
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);
            return Page();
        }
    }
}
