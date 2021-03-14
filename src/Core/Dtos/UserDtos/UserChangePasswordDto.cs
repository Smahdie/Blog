using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.UserDtos
{
    public class UserChangePasswordDto
    {
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "رمز عبور فعلی")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "رمز عبور جدید")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "تکرار رمز عبور جدید")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید و تکرار آن برابر نیستند")]
        public string ConfirmNewPassword { get; set; }
    }
}
