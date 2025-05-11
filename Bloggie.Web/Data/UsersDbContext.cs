using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class UsersDbContext : IdentityDbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // seed for roles(user,admin,superadmin)

            var userId = Guid.NewGuid().ToString();
            var adminId = Guid.NewGuid().ToString();
            var superAdminId = Guid.NewGuid().ToString();

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = userId,
                    Name = "user",
                    NormalizedName = "user".ToUpper(),
                    ConcurrencyStamp = userId
                },
                new IdentityRole()
                {
                    Id = adminId,
                    Name = "admin",
                    NormalizedName= "admin".ToUpper(),
                    ConcurrencyStamp = adminId
                },
                new IdentityRole()
                {
                    Id = superAdminId,
                    Name = "superadmin",
                    NormalizedName = "superadmin".ToUpper(),
                    ConcurrencyStamp = superAdminId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //seed for superadmin

            var superAdminUserId = Guid.NewGuid().ToString();

            var superAdminUser = new IdentityUser
            {
                Id = superAdminUserId,
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper()

            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // assign the three roles to the super admin 

            var identityUserRole = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>()
                {
                    RoleId = userId,
                    UserId = superAdminUserId
                },
                new IdentityUserRole<string>()
                {
                    RoleId = adminId,
                    UserId = superAdminUserId
                },
                new IdentityUserRole<string>()
                {
                    RoleId = superAdminId,
                    UserId = superAdminUserId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(identityUserRole);

        }
    }
}
