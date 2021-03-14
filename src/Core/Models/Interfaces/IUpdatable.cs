using System;

namespace Core.Models.Interfaces
{
    public interface IUpdatable
    {
        public DateTime? UpdatedOn { get; set; }
    }
}
