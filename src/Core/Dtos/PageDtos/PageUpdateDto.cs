using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.PageDtos
{
    public class PageUpdateDto
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "کلمه کلیدی")]
        public string Keyword { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "خلاصه متن")]
        public string Summary { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Body { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
    }
}
