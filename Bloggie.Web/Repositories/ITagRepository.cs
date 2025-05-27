using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModel;

namespace Bloggie.Web.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery = null,
            string? sortBy = null,
            string? sortingField = null,int pagenumber = 1, int pageSize = 100);
        Task<Tag?> GetAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tag);
        Task<Tag?> DeleteAsync(Guid id);
        Task<int> CountAsync();
    }
}
