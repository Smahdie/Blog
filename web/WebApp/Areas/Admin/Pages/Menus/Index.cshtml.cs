using Canducci.Pagination;
using Core.Dtos.MenuDtos;
using Core.Dtos.Settings;
using Core.Interfaces.MenuProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.Menus
{
    public class IndexModel : BasePageModel
    {
        private readonly IMenuManager _menuManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            IMenuManager menuManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _menuManager = menuManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<MenuGridDto> Menus { get; set; }

        public async Task OnGetAsync()
        {
            var searchDto = new MenuGridDto();
            var (Items, TotalCount) = await _menuManager.GetAllAsync(searchDto);
            Menus = new StaticPaginated<MenuGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(MenuGridDto searchDto)
        {
            var (Items, TotalCount) = await _menuManager.GetAllAsync(searchDto);
            Menus = new StaticPaginated<MenuGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }
        
    }
}
