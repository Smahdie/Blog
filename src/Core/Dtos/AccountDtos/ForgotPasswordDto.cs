using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.AccountDtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نیست.")]
        [Display(Name = "ایمیل", Prompt = "ایمیل")]
        public string Email { get; set; }
    }
}
