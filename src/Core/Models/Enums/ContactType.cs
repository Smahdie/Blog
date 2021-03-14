using System.ComponentModel.DataAnnotations;

namespace Core.Models.Enums
{
    public enum ContactType
    {
        [Display(Name = "آدرس")]
        Address = 1,

        [Display(Name = "ایمیل")]
        Email = 2,

        [Display(Name = "تلفن")]
        Phone = 3,

        [Display(Name = "فکس")]
        Fax = 4,

        [Display(Name = "اینستاگرام")]
        Instagram = 5,

        [Display(Name = "توییتر")]
        Twitter = 6,

        [Display(Name = "فیسبوک")]
        Facebook = 7    ,

        [Display(Name = "اسکایپ")]
        Skype = 8,

        [Display(Name = "لینکدین")]
        LinkedIn = 9,

        [Display(Name = "موقعیت روی نقشه")]
        Location  =10
    }
}
