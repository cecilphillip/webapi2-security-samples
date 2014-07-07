using System.ComponentModel.DataAnnotations;

namespace WebApiClaimsAuthorization.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name="User Name")]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password { get; set; }
    }
}