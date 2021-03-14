using Core.Dtos.CommonDtos;
using Core.Dtos.ContactInfoDtos;
using Core.Interfaces.ContactInfoProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.ContactInfos
{
    public class CreateModel : BasePageModel
    {
        private readonly IContactInfoManager _contactInfoManager;

        public CreateModel(IContactInfoManager contactInfoManager)
        {
            _contactInfoManager = contactInfoManager;
        }

        [BindProperty]
        public ContactInfoCreateDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _contactInfoManager.CreateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }
    }
}
