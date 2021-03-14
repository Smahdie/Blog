using Core.Dtos.MenuDtos;
using Core.Dtos.CommonDtos;
using Core.Interfaces.MenuProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.CategoryProviders;
using Core.Interfaces.PageProviders;

namespace Admin.Pages.Menus
{
    public class CreateModel : BasePageModel
    {
        private readonly IMenuManager _menuManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IPageManager _pageManager;

        public CreateModel(
            IMenuManager menuManager,
            ICategoryManager categoryManager,
            IPageManager pageManager)
        {
            _menuManager = menuManager;
            _categoryManager = categoryManager;
            _pageManager = pageManager;
        }

        [BindProperty]
        public MenuCreateDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            var result = await _menuManager.CreateAsync(Input);
            if (result.Success) 
            {
                result.Url = Url.Page("Index");
            }
            
            return new JsonResult(result);
        }

        public async Task<JsonResult> OnGetCategoryListAsync()
        {
            var result = await _categoryManager.GetSelectListAsync();
            return new JsonResult(result);
        }

        public async Task<JsonResult> OnGetPageListAsync()
        {
            var result = await _pageManager.GetSelectListAsync();
            return new JsonResult(result);
        }
    }
}
