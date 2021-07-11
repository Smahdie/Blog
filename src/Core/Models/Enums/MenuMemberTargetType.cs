using System.ComponentModel.DataAnnotations;

namespace Core.Models.Enums
{
    public enum MenuMemberTargetType : byte
    {
        [Display(Name = "صفحه اصلی")]
        Home = 1,

        [Display(Name = "تماس با ما")]
        ContactUs = 2,

        [Display(Name = "دسته")]
        Category = 4,

        [Display(Name = "صفحه")]
        Page = 5
    }
}
