using Canducci.Pagination;
using Core.Dtos.ContentDtos;
using Core.Dtos.Settings;
using Core.Interfaces.ContentProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace WebApp.Pages.Blog
{
    public class SearchModel : PageModel
    {
        private readonly IContentQueryProvider _contentQueryProvider;
        private readonly WebAppSettings _appSettings;

        public SearchModel(
            IContentQueryProvider contentQueryProvider,
            IOptions<WebAppSettings> appSettings)
        {
            _contentQueryProvider = contentQueryProvider;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<ContentListDto> Contents { get; set; }
        public string RequestQuery { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string query, int pageNumber)
        {
            RequestQuery = query.Trim();
            if (query.Length < 2)
            {
                ErrorMessage = "برای جستجو حداقل دو حرف وارد کنید.";
                return;
            }
            ViewData["Title"] = $"بلاگ - جستجو برای «{query}» - صفحه {pageNumber}";
            var (Items, TotalCount) = await _contentQueryProvider.SearchAsync(new ContentSearchRequestDto { Type = ContentType.Article, PageIndex = pageNumber, Keyword = query });
            Contents = new StaticPaginated<ContentListDto>(Items, pageNumber, _appSettings.PageSize, TotalCount);
            if(Contents.Count == 0)
            {
                ErrorMessage = "جستجو برای این عبارت نتیجه ای در بر نداشت.";
            }
        }
    }
}
