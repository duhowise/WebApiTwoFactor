using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TwoFactorAuthentication.API.Models
{
    public class AuthContext:IdentityDbContext<ApplicationUser>
    {
        public AuthContext():base("AuthContext")
        {
            
        }
    }

    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(16)]
        public string Psk { get; set; }
    }

    public class UserModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
 
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
 
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}