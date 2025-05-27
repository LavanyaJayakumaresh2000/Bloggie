using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery = null,
            string? sortBy = null,
            string? sortingField = null,
            int pageNumber = 1,
            int pageSize = 3);
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost?> GetBlogsByUrlhandleAsync(string urlHandle);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);
        Task<int> CountAsync();
    }
}
