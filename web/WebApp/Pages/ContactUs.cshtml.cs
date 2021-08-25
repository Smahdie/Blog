using Core.Dtos.CommonDtos;
using Core.Dtos.ContactInfoDtos;
using Core.Interfaces.CaptchaProviders;
using Core.Interfaces.ContactInfoProviders;
using Core.Interfaces.MessageProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Pages
{
    public class ContactUsModel : PageModel
    {
        private readonly IMessageCommand _messageCommandProvider;
        private readonly IContactInfoQuery _contactInfoQueryProvider;
        private readonly ICaptchaManager _captchaManager;

        public ContactUsModel(
            IMessageCommand messageCommandProvider,
            IContactInfoQuery contactInfoQueryProvider,
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
            var language = HttpContext.CurrentLanguage();
            Contacts = await _contactInfoQueryProvider.GetAllAsync(language);
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

            var dto = new Core.Dtos.MessageDtos.MessageSendDto 
            {
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                PhoneNumber = Input.PhoneNumber,
                Text = Input.Text
            };

            var sendResult = await _messageCommandProvider.SendAsync(dto);
            return new JsonResult(sendResult);
        }
    }
}
