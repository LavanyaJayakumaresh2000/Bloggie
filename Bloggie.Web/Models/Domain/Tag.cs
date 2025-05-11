namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        /* relation between Tag and BlogPost --> Tag can have multiple BlogPost*/
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
