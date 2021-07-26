using Core.Dtos.CategoryDtos;
using Core.Dtos.CommonDtos;
using Core.Interfaces.CategoryProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Categories
{
    public class EditModel : BasePageModel
    {
        private readonly ICategoryManager _categoryManager;

        public EditModel(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [BindProperty]
        public CategoryUpdateDto Input { get; set; }

        public async Task OnGetAsync(int id)
        {
            Input = await _categoryManager.GetAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            var result = await _categoryManager.UpdateAsync(Input);
            if (result.Success)
            {
                result.Url = Url.Page("Index");
            }
            return new JsonResult(result);
        }

        public async Task<JsonResult> OnGetListAsync(int id)
        {
            var result = await _categoryManager.GetSelectListAsync(id);
            return new JsonResult(result);
        }
    }
}
