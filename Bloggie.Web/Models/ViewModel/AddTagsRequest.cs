using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModel
{
    public class AddTagsRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
                
    }
}
