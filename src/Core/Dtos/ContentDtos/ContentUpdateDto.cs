using Core.Dtos.CommonDtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ContentDtos
{
    public class ContentUpdateDto
    {
        [Display(Name = "شناسه")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public int Id { get; set; }
        
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string Title { get; set; }

        [Display(Name = "خلاصه")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string Summary { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string Body { get; set; }

        [Display(Name = "برچسب ها")]
        public string[] Tags { get; set; }

        [Display(Name = "دسته ها")]
        public string Categories { get; set; }

        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string ImagePath { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        public string Language { get; set; }

        public List<int> SelectedCategories { get; set; }

        public List<Select2ItemDto> SelectedTags { get; set; }
    }
}
