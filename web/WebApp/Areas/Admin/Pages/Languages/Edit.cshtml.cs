using Core.Dtos.CommonDtos;
using Core.Dtos.LanguageDtos;
using Core.Interfaces.LanguageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Languages
{
    public class EditModel : BasePageModel
    {
        private readonly ILanguageManager _languageManager;

        public EditModel(ILanguageManager languageManager)
        {
            _languageManager = languageManager;
        }

        [BindProperty]
        public LanguageUpdateDto Input { get; set; }

        public async Task OnGetAsync(int id)
        {
            Input = await _languageManager.GetAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _languageManager.UpdateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }
    }
}
