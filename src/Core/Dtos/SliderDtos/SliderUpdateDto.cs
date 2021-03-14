using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.SliderDtos
{
    public class SliderUpdateDto
    {
        [Display(Name = "شناسه")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string Title { get; set; }

        [Display(Name = "تیتر")]
        public string Heading { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string ImagePath { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Display(Name = "متن لینک")]
        public string LinkText { get; set; }
    }
}
