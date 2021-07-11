using Canducci.Pagination;
using Core.Dtos.CommonDtos;
using Core.Dtos.ContentDtos;
using Core.Dtos.Settings;
using Core.Interfaces.CategoryProviders;
using Core.Interfaces.ContentProviders;
using Core.Interfaces.TagProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Pages.Blog
{
    public class ListModel : PageModel
    {
        private readonly IContentQueryProvider _contentQueryProvider;
        private readonly ITagQueryProvider _tagQueryProvider;
        private readonly ICategoryQueryProvider _categoryQueryProvider;
        private readonly WebAppSettings _appSettings;

        public ListModel(
            IContentQueryProvider contentQueryProvider,
            ITagQueryProvider tagQueryProvider,
            ICategoryQueryProvider categoryQueryProvider,
            IOptions<WebAppSettings> appSettings)
        {
            _contentQueryProvider = contentQueryProvider;
            _tagQueryProvider = tagQueryProvider;
            _categoryQueryProvider = categoryQueryProvider;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<ContentListDto> Contents { get; set; }
        public string RequestHandler { get; set; }
        public int? RequestId { get; set; }
        public string RequestSlug { get; set; }

        public async Task OnGetAllAsync(int pageNumber)
        {
            ViewData["Title"] = $"بلاگ - صفحه {pageNumber}";
            RequestHandler = "All";
            ViewData["BreadCrumbTitle"] = ViewData["PageHeading"] = "بلاگ";
            var (Items, TotalCount) = await _contentQueryProvider.GetAllAsync(new ContentListRequestDto { Type = ContentType.Article, PageIndex = pageNumber});
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);
        }

        public async Task<IActionResult> OnGetTagAsync(int pageNumber, int id,string slug)
        {
            RequestHandler = "Tag";
            RequestId = id;
            RequestSlug = slug;

            var tag = await _tagQueryProvider.GetByIdAsync(id);
            if(tag == null)
            {
                return NotFound();
            }
            if (slug == null || tag.Slug != slug)
            {
                return RedirectToPage("List", new { handler = RequestHandler, pageNumber, tag.Id, tag.Slug });
            }
            ViewData["Title"] = $"بلاگ - برچسب {tag.Name} - صفحه {pageNumber}";
            ViewData["BreadCrumbTitle"] = ViewData["PageHeading"] = tag.Name;
            var (Items, TotalCount) = await _contentQueryProvider.GetAllAsync(new ContentListRequestDto { Type = ContentType.Article, PageIndex = pageNumber, TagId = id });
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);
            return Page();
        }

        public async Task<IActionResult> OnGetCategoryAsync(int pageNumber, int id, string slug)
        {
            RequestHandler = "Category";
            RequestId = id;
            RequestSlug = slug;

            var category = await _categoryQueryProvider.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            if (slug == null || category.Slug != slug)
            {
                return RedirectToPage("List", new { handler = RequestHandler, pageNumber, category.Id, category.Slug });
            }
            
            ViewData["Title"] = $"بلاگ - دسته {category.Name} - صفحه {pageNumber}";
            
            ViewData["BreadCrumbTitle"] = ViewData["PageHeading"] = category.Name;

            ViewData["BreadCrumb"] = new List<BreadCrumbDto> { new BreadCrumbDto { Text = "بلاگ", Href = Url.Page("List", "all") } };

            var (Items, TotalCount) = await _contentQueryProvider.GetAllAsync(new ContentListRequestDto { Type = ContentType.Article, PageIndex = pageNumber, CategoryId = id });
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);

            return Page();
        }
    }
}
