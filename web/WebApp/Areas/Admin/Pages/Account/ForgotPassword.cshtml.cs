using Admin.Extensions;
using Admin.Services.Mail;
using Core.Dtos.AccountDtos;
using Core.Models.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Admin.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Manager> _userManager;
        private readonly IMailManager _emailSender;

        public ForgotPasswordModel(UserManager<Manager> userManager, IMailManager emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public ForgotPasswordDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();                
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
            await _emailSender.SendResetPasswordAsync(Input.Email, callbackUrl);
            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}
