using Core.Models.Interfaces;
using System;

namespace Core.Models
{
    public class Tag : ICreatable, IDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
