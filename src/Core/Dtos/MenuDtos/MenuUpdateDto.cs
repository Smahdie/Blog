using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.MenuDtos
{
    public class MenuUpdateDto
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "کلمه کلیدی")]
        public string Keyword { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "اجزای منو")]
        public string MenuMembers { get; set; }

        public List<MenuMemberUpdateDto> PrevMenuMembers { get; set; }
    }
}
