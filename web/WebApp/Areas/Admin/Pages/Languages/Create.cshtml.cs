using Core.Dtos.CategoryDtos;
using Core.Dtos.CommonDtos;
using Core.Dtos.LanguageDtos;
using Core.Interfaces.CategoryProviders;
using Core.Interfaces.LanguageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Languages
{
    public class CreateModel : BasePageModel
    {
        private readonly ILanguageManager _languageManager;

        public CreateModel(ILanguageManager languageManager)
        {
            _languageManager = languageManager;
        }

        [BindProperty]
        public LanguageCreateDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _languageManager.CreateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }
    }
}
