using Core.Models.Interfaces;
using System;

namespace Core.Models
{
    public class Slider : IAuditable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
