using Core.Models.Interfaces;
using System;

namespace Core.Models
{
    public class Language : ICreatable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
