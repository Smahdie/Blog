using System.ComponentModel.DataAnnotations;

namespace Core.Models.Enums
{
    public enum Gender : byte
    {
        [Display(Name = "نامشخص")]
        Other = 0,

        [Display(Name = "زن")]
        Female = 1,
        
        [Display(Name = "مرد")]
        Male = 2
    }
}
