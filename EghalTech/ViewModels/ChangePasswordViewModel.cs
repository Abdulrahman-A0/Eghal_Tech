using System.ComponentModel.DataAnnotations;

namespace EghalTech.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Enter Current Password")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password confirmation doesn't match")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
