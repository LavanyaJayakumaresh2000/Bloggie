using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModel
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Password length must be over 6 characters")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
