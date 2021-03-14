using Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Category : IAuditable
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Category Parent { get; set; }
        public ICollection<Category> Children { get; set; }
        public ICollection<CategoryTranslation> Translations { get; set; }
    }
}
