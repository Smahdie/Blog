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
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.Pages.Blog
{
    public class ListModel : PageModel
    {
        private readonly IContentQuery _contentQueryProvider;
        private readonly ITagQuery _tagQueryProvider;
        private readonly ICategoryQuery _categoryQueryProvider;
        private readonly WebAppSettings _appSettings;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public ListModel(
            IContentQuery contentQueryProvider,
            ITagQuery tagQueryProvider,
            ICategoryQuery categoryQueryProvider,
            IOptions<WebAppSettings> appSettings,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _contentQueryProvider = contentQueryProvider;
            _tagQueryProvider = tagQueryProvider;
            _categoryQueryProvider = categoryQueryProvider;
            _appSettings = appSettings.Value;
            _sharedLocalizer = sharedLocalizer;
        }

        public StaticPaginated<ContentListDto> Contents { get; set; }
        public string RequestHandler { get; set; }
        public int? RequestId { get; set; }
        public string RequestSlug { get; set; }

        public async Task OnGetAllAsync(int pageNumber)
        {
            var language = HttpContext.CurrentLanguage();
            ViewData["Title"] = string.Format(_sharedLocalizer["Content All Title"],pageNumber);
            RequestHandler = "All";
            ViewData["BreadCrumbTitle"] = ViewData["PageHeading"] = _sharedLocalizer["Blog"];
            var (Items, TotalCount) = await _contentQueryProvider.GetAllAsync(new ContentListRequestDto { Language = language, Type = ContentType.Article, PageIndex = pageNumber });
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);
        }

        public async Task<IActionResult> OnGetTagAsync(int pageNumber, int id, string slug)
        {
            RequestHandler = "Tag";
            RequestId = id;
            RequestSlug = slug;

            var tag = await _tagQueryProvider.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            if (slug == null || tag.Slug != slug)
            {
                return RedirectToPage("List", new { handler = RequestHandler, pageNumber, tag.Id, tag.Slug });
            }
            ViewData["Title"] = string.Format(_sharedLocalizer["Tag Title"], tag.Name, pageNumber);
            ViewData["BreadCrumbTitle"] = ViewData["PageHeading"] = tag.Name;
            ViewData["BreadCrumb"] = new List<BreadCrumbDto> { new BreadCrumbDto { Text = _sharedLocalizer["Blog"], Href = Url.Page("List", "all") } };
            var language = HttpContext.CurrentLanguage();
            var (Items, TotalCount) = await _contentQueryProvider.GetAllAsync(new ContentListRequestDto { Language = language, Type = ContentType.Article, PageIndex = pageNumber, TagId = id });
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);
            return Page();
        }

        public async Task<IActionResult> OnGetCategoryAsync(string culture, int pageNumber, int id, string slug)
        {
            var language = HttpContext.CurrentLanguage();

            RequestHandler = "Category";
            RequestId = id;
            RequestSlug = slug;

            var category = await _categoryQueryProvider.GetByIdAsync(id, language);
            if (category == null)
            {
                return NotFound();
            }
            if (slug == null || category.Slug != slug)
            {
                return RedirectToPage("List", new { handler = RequestHandler, pageNumber, category.Id, category.Slug });
            }

            ViewData["Title"] = string.Format(_sharedLocalizer["Category Title"], category.Name, pageNumber);

            ViewData["BreadCrumbTitle"] = ViewData["PageHeading"] = category.Name;

            ViewData["BreadCrumb"] = new List<BreadCrumbDto> { new BreadCrumbDto { Text = _sharedLocalizer["Blog"], Href = Url.Page("List", "all") } };

            
            var (Items, TotalCount) = await _contentQueryProvider.GetAllAsync(new ContentListRequestDto { Language = language, Type = ContentType.Article, PageIndex = pageNumber, CategoryId = id });
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);

            return Page();
        }
    }
}