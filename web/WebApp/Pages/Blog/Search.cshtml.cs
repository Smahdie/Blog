using Canducci.Pagination;
using Core.Dtos.ContentDtos;
using Core.Dtos.Settings;
using Core.Interfaces.ContentProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.Pages.Blog
{
    public class SearchModel : PageModel
    {
        private readonly IContentQuery _contentQueryProvider;
        private readonly WebAppSettings _appSettings;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public SearchModel(
            IContentQuery contentQueryProvider,
            IOptions<WebAppSettings> appSettings,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _contentQueryProvider = contentQueryProvider;
            _appSettings = appSettings.Value;
            _sharedLocalizer = sharedLocalizer;
        }

        public StaticPaginated<ContentListDto> Contents { get; set; }
        public string RequestQuery { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string query, int pageNumber)
        {
            RequestQuery = query.Trim();
            if (query.Length < 2)
            {
                ErrorMessage = _sharedLocalizer["Search Length Limit"];
                return;
            }
            ViewData["Title"] = string.Format(_sharedLocalizer["Search Title"],query, pageNumber);

            var language = HttpContext.CurrentLanguage();

            var (Items, TotalCount) = await _contentQueryProvider.SearchAsync(new ContentSearchRequestDto { Language = language, Type = ContentType.Article, PageIndex = pageNumber, Keyword = query });
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);
            if(Contents.Count == 0)
            {
                ErrorMessage = _sharedLocalizer["Search No Result"];
            }
        }
    }
}
