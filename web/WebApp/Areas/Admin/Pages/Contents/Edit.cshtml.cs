using Core.Dtos.CommonDtos;
using Core.Dtos.ContentDtos;
using Core.Interfaces.CategoryProviders;
using Core.Interfaces.ContentProviders;
using Core.Interfaces.TagProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Contents
{
    public class EditModel : BasePageModel
    {
        private readonly IContentManager _contentManager;
        private readonly ITagManager _tagManager;
        private readonly ICategoryManager _categoryManager;

        public EditModel(
            IContentManager contentManager,
            ITagManager tagManager,
            ICategoryManager categoryManager)
        {
            _contentManager = contentManager;
            _tagManager = tagManager;
            _categoryManager = categoryManager;
        }

        [BindProperty]
        public ContentUpdateDto Input { get; set; }

        public async Task OnGetAsync(int id)
        {
            Input = await _contentManager.GetAsync(id);
        }

        public async Task<JsonResult> OnGetTagsAsync(string search, int page)
        {
            if (page < 1)
                page = 1;
            var result = await _tagManager.GetSelectListAsync(search, page);
            return new JsonResult(result);
        }

        public async Task<JsonResult> OnGetCategoriesAsync()
        {
            var result = await _categoryManager.GetTreeAsync();
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _contentManager.UpdateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }
    }
}
