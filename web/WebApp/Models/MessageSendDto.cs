using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class MessageSendDto
    {
        [Display(Name = "First Name", Prompt = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name", Prompt = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email", Prompt = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        [Display(Name = "Phone Number", Prompt = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Message Text", Prompt = "Message Text")]
        [Required(ErrorMessage = "Please enter the {0}")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Invalid form data")]
        public string Token { get; set; }
    }
}
