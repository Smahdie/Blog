using Core.Models.Enums;
using Core.Models.Interfaces;
using System;

namespace Core.Models
{
    public class ContactInfo : IAuditable
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public ContactType ContactType { get; set; }
        public string Language { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
