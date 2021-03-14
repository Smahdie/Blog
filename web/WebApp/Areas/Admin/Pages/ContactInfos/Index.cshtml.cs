using Canducci.Pagination;
using Core.Dtos.ContactInfoDtos;
using Core.Dtos.Settings;
using Core.Interfaces.ContactInfoProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.ContactInfos
{
    public class IndexModel : BasePageModel
    {
        private readonly IContactInfoManager _contactInfoManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            IContactInfoManager contactInfoManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _contactInfoManager = contactInfoManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<ContactInfoGridDto> ContactInfos { get; set; }

        public async Task OnGetAsync()
        {
            var searchDto = new ContactInfoGridDto();
            var (Items, TotalCount) = await _contactInfoManager.GetAllAsync(searchDto);
            ContactInfos = new StaticPaginated<ContactInfoGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(ContactInfoGridDto searchDto)
        {
            var (Items, TotalCount) = await _contactInfoManager.GetAllAsync(searchDto);
            ContactInfos = new StaticPaginated<ContactInfoGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _contactInfoManager.DeleteAsync(id);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id)
        {
            var result = await _contactInfoManager.ChangeStatusAsync(id);
            return new JsonResult(result);
        }
    }
}
