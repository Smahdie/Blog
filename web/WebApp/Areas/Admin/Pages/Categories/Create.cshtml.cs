using Core.Dtos.CategoryDtos;
using Core.Dtos.CommonDtos;
using Core.Interfaces.CategoryProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Categories
{
    public class CreateModel : BasePageModel
    {
        private readonly ICategoryManager _categoryManager;

        public CreateModel(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [BindProperty]
        public CategoryCreateDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _categoryManager.CreateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }

        public async Task<JsonResult> OnGetListAsync()
        {
            var result = await _categoryManager.GetSelectListAsync();
            return new JsonResult(result);
        }
    }
}
