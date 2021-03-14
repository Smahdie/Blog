using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CategoryDtos
{
    public class CategoryUpdateDto
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "بالاسری")]
        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Name { get; set; }
    }
}
