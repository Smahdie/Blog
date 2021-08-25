using Core.Dtos.PageDtos;
using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.Pages.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IPageQuery _pageQueryProvider;

        public DetailsModel(IPageQuery pageQueryProvider)
        {
            _pageQueryProvider = pageQueryProvider;
        }

        public PageDetailsDto Data { get; set; }

        public async Task<IActionResult> OnGet(string keyword)
        {
            var language = HttpContext.CurrentLanguage();
            Data = await _pageQueryProvider.GetDetailsAsync(keyword, language);
            return Page();
        }
    }
}
