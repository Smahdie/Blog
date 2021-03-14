using Canducci.Pagination;
using Core.Dtos.CategoryDtos;
using Core.Dtos.Settings;
using Core.Interfaces.CategoryProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.Categories
{
    public class IndexModel : BasePageModel
    {
        private readonly ICategoryManager _categoryManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            ICategoryManager categoryManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _categoryManager = categoryManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<CategoryGridDto> Categories { get; set; }

        public async Task OnGetAsync()
        {
            var searchDto = new CategoryGridDto ();
            var (Items, TotalCount) = await _categoryManager.GetAllAsync(searchDto);
            Categories = new StaticPaginated<CategoryGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(CategoryGridDto searchDto)
        {
            var (Items, TotalCount) = await _categoryManager.GetAllAsync(searchDto);
            Categories = new StaticPaginated<CategoryGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _categoryManager.DeleteAsync(id);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id)
        {
            var result = await _categoryManager.ChangeStatusAsync(id);
            return new JsonResult(result);
        }
    }
}
