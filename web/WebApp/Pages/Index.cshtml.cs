using Core.Dtos.CommonDtos;
using Core.Dtos.ContentDtos;
using Core.Interfaces.ContentProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IContentQuery _contentQueryProvider;

        public IndexModel(
            IContentQuery contentQueryProvider)
        {
            _contentQueryProvider = contentQueryProvider;
        }

        public ListHasMoreDto<ContentListDto> Contents { get; set; }

        public async Task OnGet()
        {
            var language = HttpContext.CurrentLanguage();
            Contents = await _contentQueryProvider.GetTopAsync(new TopContentRequestDto { Type = ContentType.Article, Language = language });
        }
    }
}
