using System.ComponentModel.DataAnnotations;
namespace BestStoreMVC.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "The first name field is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";
        [Required(ErrorMessage = "The last name field is required"), MaxLength(100)]
        public string LastName { get; set; } = "";
        [Required, EmailAddress MaxLength(100)]
        public string Email { get; set; } = ""; 
        [Phone(ErrorMessage = "The first name field is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";
    }
}
