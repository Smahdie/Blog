using System;

namespace Core.Models.Interfaces
{
    public interface IDeletable
    {
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
