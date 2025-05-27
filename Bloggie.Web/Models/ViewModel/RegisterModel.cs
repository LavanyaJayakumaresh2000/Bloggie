using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModel
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Password must be over 6 characters")]
        public string Password { get; set; }
    }
}
