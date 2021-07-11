using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.LanguageDtos
{
    public class LanguageGridDto : BaseGridDto
    {
        [Display(Name = "شناسه")]
        public int? Id { get; set; }

        [Display(Name = "نام")]
        [Search]
        public string Name { get; set; }

        [Display(Name = "مخفف")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Search]
        public string Code { get; set; }

        [Display(Name = "زبان پیش فرض")]
        [Boolean]
        public bool IsDefault { get; set; }

        [Display(Name = "فعال")]
        [Boolean]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ درج")]
        public string CreatedOn { get; set; }
    }
}
