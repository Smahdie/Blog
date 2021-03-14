using Core.Dtos.CommonDtos;
using Core.Dtos.ContactInfoDtos;
using Core.Dtos.MessageDtos;
using Core.Interfaces.CaptchaProviders;
using Core.Interfaces.ContactInfoProviders;
using Core.Interfaces.MessageProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Pages
{
    public class ContactUsModel : PageModel
    {
        private readonly IMessageCommandProvider _messageCommandProvider;
        private readonly IContactInfoQueryProvider _contactInfoQueryProvider;
        private readonly ICaptchaManager _captchaManager;

        public ContactUsModel(
            IMessageCommandProvider messageCommandProvider,
            IContactInfoQueryProvider contactInfoQueryProvider,
            ICaptchaManager captchaManager)
        {
            _messageCommandProvider = messageCommandProvider;
            _contactInfoQueryProvider = contactInfoQueryProvider;
            _captchaManager = captchaManager;
        }

        [BindProperty]
        public MessageSendDto Input { get; set; }

        public List<ContactInfoListDto> Contacts { get; set; }

        public async Task OnGetAsync()
        {
            Contacts = await _contactInfoQueryProvider.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            var captchaResult = await _captchaManager.IsValidAsync(Input.Token);
            if (!captchaResult.Success)
            {
                return new JsonResult(captchaResult);
            }

            var sendResult = await _messageCommandProvider.SendAsync(Input);
            return new JsonResult(sendResult);
        }
    }
}
