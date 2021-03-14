using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.MenuDtos
{
    public class MenuCreateDto
    {
        [Display(Name = "عنوان منو")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "کلمه کلیدی")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Keyword { get; set; }

        [Display(Name = "اجزای منو")]
        public string MenuMembers { get; set; }
    }
}
