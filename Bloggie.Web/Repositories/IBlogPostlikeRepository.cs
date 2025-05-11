using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostlikeRepository
    {
        Task<int> GetAllLikesAsync(Guid blogPostId);
        Task<IEnumerable<BlogPostLike>> GetAllLikesForBlogPost(Guid blogPostId);
        Task<BlogPostLike> AddLikeForBlogAsync(BlogPostLike blogPostLike);
    }
}
