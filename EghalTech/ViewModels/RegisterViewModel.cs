using System.ComponentModel.DataAnnotations;

namespace EghalTech.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter Your Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Your Phone Number")]
        [RegularExpression("^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Enter A Valid Number(01X-XXXX-XXXX)")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Enter Your Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Your Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password confirmation doesn't match")]
        public string ConfirmPassword { get; set; }

    }
}
