using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext usersDbContext;

        public UserRepository(UsersDbContext usersDbContext)
        {
            this.usersDbContext = usersDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAllAsync(string? searchQuery,string? sortBy,string? sortingField,
            int pageNumber = 1,int pageSize = 3)
        {
            var users = await usersDbContext.Users.ToListAsync();

            var search = users.AsQueryable();

            //Ignoring SuperAdmin 

            var adminUser = await usersDbContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if (adminUser != null)
            {
                users.Remove(adminUser);
            }

            //Filtering
            if (!string.IsNullOrEmpty(searchQuery))
            {
                search = search.Where(x => x.UserName.Contains(searchQuery));
            }

            //Sorting
            if (string.IsNullOrEmpty(sortBy) == false)
            {
                var isDesc = string.Equals(sortBy, "Desc", StringComparison.OrdinalIgnoreCase);
                
                if(string.Equals(sortingField,"UserName",StringComparison.OrdinalIgnoreCase))
                {
                    search = isDesc ? search.OrderByDescending(x=>x.UserName) : search.OrderBy(x=>x.UserName);
                }

                if (string.Equals(sortingField, "Email", StringComparison.OrdinalIgnoreCase))
                {
                    search = isDesc ? search.OrderByDescending(x=>x.Email) : search.OrderBy(x=>x.Email);
                }
            }
            //pagination

            search = search.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            users = search.ToList();

            return users;
        }

        public async Task<int> GetTotalCount()
        {
            var user = await usersDbContext.Users.ToListAsync();

            var adminUser = await usersDbContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if(adminUser != null)
            {
                user.Remove(adminUser);
            }
            return user.Count();
        }

        public async Task<IdentityUser?> RemoveAsync(Guid id)
        {
            var identityUser = new IdentityUser
            {
                Id = id.ToString()
            };
            var user = await usersDbContext.Users.FindAsync(identityUser.Id);
            if (user != null)
            {
                usersDbContext.Remove(user);
                await usersDbContext.SaveChangesAsync();
                return user;

            }
            return null;
        }
    }
}
