namespace Bloggie.Web.Models.ViewModel
{
    public class UserRequest
    {
        public IList<UserViewModel> GetUsers { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool CheckBoxForRole { get; set; }
        public string SearchQuery { get; set; }
        public string SortBy { get; set; }
        public string SortingField { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public double TotalCount { get; set; }
    }
}
