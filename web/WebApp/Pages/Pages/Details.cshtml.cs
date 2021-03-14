using Core.Dtos.PageDtos;
using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebApp.Pages.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IPageQueryProvider _pageQueryProvider;

        public DetailsModel(IPageQueryProvider pageQueryProvider)
        {
            _pageQueryProvider = pageQueryProvider;
        }

        public PageDetailsDto Data { get; set; }

        public async Task<IActionResult> OnGet(string keyword)
        {
            Data = await _pageQueryProvider.GetDetailsAsync(keyword);
            return Page();
        }
    }
}
