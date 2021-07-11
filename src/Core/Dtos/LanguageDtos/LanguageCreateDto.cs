using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.LanguageDtos
{
    public class LanguageCreateDto
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "مخفف")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Code { get; set; }

        [Display(Name = "زبان پیش فرض")]
        public bool IsDefault { get; set; }
    }
}
