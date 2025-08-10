using System.ComponentModel.DataAnnotations;

namespace EghalTech.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(100, ErrorMessage = "Name can't be more than 100 character")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression("^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Enter A Valid Number(01X-XXXX-XXXX)")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
    }
}
