using Canducci.Pagination;
using Core.Dtos.Settings;
using Core.Dtos.UserDtos;
using Core.Interfaces.UserProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.Users
{
    public class IndexModel : BasePageModel
    {
        private readonly IUserManager _userManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            IUserManager userManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<UserGridDto> Users { get; set; }

        public async Task OnGetAsync()
        {
            var searchDto = new UserGridDto { PageOrderBy = "UserName", PageOrder = "asc" };
            var (Items, TotalCount) = await _userManager.GetAllAsync(searchDto);
            Users = new StaticPaginated<UserGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(UserGridDto searchDto)
        {
            var (Items, TotalCount) = await _userManager.GetAllAsync(searchDto);
            Users = new StaticPaginated<UserGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var result = await _userManager.DeleteAsync(id);
            return new JsonResult(result);

        }

        public async Task<IActionResult> OnPostChangeStatusAsync(string id)
        {
            var result = await _userManager.ChangeStatusAsync(id);
            return new JsonResult(result);
        }
    }
}
