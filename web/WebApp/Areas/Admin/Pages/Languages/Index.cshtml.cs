using Canducci.Pagination;
using Core.Dtos.LanguageDtos;
using Core.Dtos.Settings;
using Core.Interfaces.LanguageProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.Languages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILanguageManager _languageManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            ILanguageManager languageManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _languageManager = languageManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<LanguageGridDto> Languages { get; set; }

        public async Task OnGetAsync()
        {
            var searchDto = new LanguageGridDto ();
            var (Items, TotalCount) = await _languageManager.GetAllAsync(searchDto);
            Languages = new StaticPaginated<LanguageGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(LanguageGridDto searchDto)
        {
            var (Items, TotalCount) = await _languageManager.GetAllAsync(searchDto);
            Languages = new StaticPaginated<LanguageGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostChangeDefaultAsync(int id)
        {
            var result = await _languageManager.ChangeDefaultAsync(id);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostChangeActiveAsync(int id)
        {
            var result = await _languageManager.ChangeActiveAsync(id);
            return new JsonResult(result);
        }
    }
}
