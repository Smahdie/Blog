using Core.Models.Enums;
using Core.Models.Interfaces;
using System;

namespace Core.Models
{
    public class MenuMember: ICreatable
    {
        public int Id { get; set; }
        public MenuMemberTargetType TargetType { get; set; }
        public int? PageId { get; set; }
        public int? CategoryId { get; set; }
        public int Rank { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public DateTime CreatedOn { get; set; }
        public Category Category { get; set; }
        public Page Page { get; set; }
    }
}
