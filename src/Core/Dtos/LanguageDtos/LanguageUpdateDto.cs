using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.LanguageDtos
{
    public class LanguageUpdateDto
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "مخفف")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Code { get; set; }
    }
}
