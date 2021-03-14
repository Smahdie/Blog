using Core.Models.Interfaces;
using System;

namespace Core.Models
{
    public class Message : IAuditable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
