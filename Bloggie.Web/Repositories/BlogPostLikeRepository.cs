
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostlikeRepository
    {
        private readonly BloggieDbcontext bloggieDbcontext;

        public BlogPostLikeRepository(BloggieDbcontext bloggieDbcontext)
        {
            this.bloggieDbcontext = bloggieDbcontext;
        }

        public async Task<BlogPostLike> AddLikeForBlogAsync(BlogPostLike blogPostLike)
        {
            await bloggieDbcontext.BlogPostLikes.AddAsync(blogPostLike);
            await bloggieDbcontext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<int> GetAllLikesAsync(Guid blogPostId)
        {

            return await bloggieDbcontext.BlogPostLikes.CountAsync(x => x.BlogPostId == blogPostId);

        }

        public async Task<IEnumerable<BlogPostLike>> GetAllLikesForBlogPost(Guid blogPostId)
        {
            return await bloggieDbcontext.BlogPostLikes.Where(x=>x.BlogPostId==blogPostId).ToListAsync();
        }
    }
}
