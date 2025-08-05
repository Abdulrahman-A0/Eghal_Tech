using System.ComponentModel.DataAnnotations;

namespace EghalTech.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Yout Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Yout Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
