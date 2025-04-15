using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.DTOS
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^.{8,}$"
      , ErrorMessage = "Must contain at least 1 uppercase letter , 1 Lowercase letter , 1 digit ,1 special character")]
        public string Password { get; set; }

    }
}
