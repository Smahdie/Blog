using Canducci.Pagination;
using Core.Dtos.PageDtos;
using Core.Dtos.Settings;
using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly IPageManager _pageManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            IPageManager pageManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _pageManager = pageManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<PageGridDto> Categories { get; set; }

        public async Task OnGetAsync()
        {
            var searchDto = new PageGridDto ();
            var (Items, TotalCount) = await _pageManager.GetAllAsync(searchDto);
            Categories = new StaticPaginated<PageGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(PageGridDto searchDto)
        {
            var (Items, TotalCount) = await _pageManager.GetAllAsync(searchDto);
            Categories = new StaticPaginated<PageGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _pageManager.DeleteAsync(id);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id)
        {
            var result = await _pageManager.ChangeStatusAsync(id);
            return new JsonResult(result);
        }
    }
}
