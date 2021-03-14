using Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.MenuDtos
{
    public class MenuMemberUpdateDto
    {        
        public int Id { get; set; }

        [Display(Name = "لینک به")]
        public MenuMemberTargetType TargetType { get; set; }

        [Display(Name = "انتخاب صفحه")]
        public int? PageId { get; set; }

        [Display(Name = "انتخاب صفحه")]
        public string PageTitle { get; set; }

        [Display(Name = "انتخاب دسته")]
        public int? CategoryId { get; set; }

        [Display(Name = "انتخاب دسته")]
        public string CategoryName { get; set; }

        [Display(Name = "جایگاه")]
        public int Rank { get; set; }
    }
}
