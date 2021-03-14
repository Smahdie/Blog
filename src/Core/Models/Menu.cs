using Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Menu : ICreatable, IUpdatable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Keyword { get; set; }
        public string Language { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ICollection<MenuMember> Members { get; set; }
    }
}
