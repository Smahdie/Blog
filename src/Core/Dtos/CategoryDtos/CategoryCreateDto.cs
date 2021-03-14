using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CategoryDtos
{
    public class CategoryCreateDto
    {
        [Display(Name="بالاسری")]
        public int? ParentId { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Name { get; set; }

    }
}
