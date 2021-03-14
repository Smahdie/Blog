using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.AccountDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نیست.")]
        [Display(Name = "ایمیل", Prompt = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "من را به خاطر داشته باش")]
        public bool RememberMe { get; set; }
    }
}
