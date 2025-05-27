using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllAsync(
            string? searchQuery = null,
            string? sortBy = null,
            string? sortingField= null,
            int pageNumber = 0,
            int pageSize = 3);
        Task<IdentityUser?> RemoveAsync(Guid id);
        Task<int> GetTotalCount();
    }
}
