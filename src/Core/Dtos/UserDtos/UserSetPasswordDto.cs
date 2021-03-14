using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.UserDtos
{
    public class UserSetPasswordDto
    {
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "شناسه", Prompt = "شناسه")]
        public string Id { get; set; }

        public string Email{get;set;}

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "طول {0} باید حداقل {2} و حداکثر {1} کاراکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید", Prompt = "رمز عبور جدید")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور جدید", Prompt = "تکرار رمز عبور جدید")]
        [Compare("Password", ErrorMessage = "رمز عبور جدید و تکرار آن برابر نیستند")]
        public string ConfirmPassword { get; set; }
    }
}
