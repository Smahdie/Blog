using Core.Dtos.CommonDtos;
using Core.Dtos.ContentDtos;
using Core.Interfaces.ContentProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IContentQueryProvider _contentQueryProvider;

        public IndexModel(
            IContentQueryProvider contentQueryProvider)
        {
            _contentQueryProvider = contentQueryProvider;
        }

        public ListHasMoreDto<ContentListDto> Contents { get; set; }

        public async Task OnGet()
        {
            Contents = await _contentQueryProvider.GetTopAsync(new TopContentRequestDto { Type= ContentType.Article });
        }
    }
}
