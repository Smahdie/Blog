using Core.Dtos.ContactInfoDtos;
using Core.Dtos.CommonDtos;
using Core.Interfaces.ContactInfoProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.ContactInfos
{
    public class EditModel : BasePageModel
    {
        private readonly IContactInfoManager _contactInfoManager;

        public EditModel(IContactInfoManager contactInfoManager)
        {
            _contactInfoManager = contactInfoManager;
        }

        [BindProperty]
        public ContactInfoUpdateDto Input { get; set; }

        public async Task OnGetAsync(int id)
        {
            Input = await _contactInfoManager.GetAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _contactInfoManager.UpdateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }
    }
}
