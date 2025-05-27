using System.Threading.Tasks;
using Bloggie.Web.Models.ViewModel;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles ="admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> List(string searchQuery,string sortBy,string sortingField,
            int pageNumber = 1, int pageSize = 3)
        {
            var userRequest = new UserRequest();
            userRequest.GetUsers = new List<UserViewModel>();

            userRequest.SearchQuery = searchQuery;
            userRequest.SortBy = sortBy;
            userRequest.SortingField = sortingField;
            userRequest.PageNumber = pageNumber;
            userRequest.PageSize = pageSize;

            var total = await userRepository.GetTotalCount();
            var totalCount = Math.Ceiling((double) total / userRequest.PageSize);

            userRequest.TotalCount = totalCount;

            if (userRequest.PageNumber < 1)
            {
                userRequest.PageNumber++;
            }
            if(userRequest.PageNumber > userRequest.TotalCount)
            {
                userRequest.PageNumber--;
            }

            var userRepo = await userRepository.GetAllAsync(searchQuery,sortBy, sortingField,pageNumber,pageSize);

            if (userRepo is not null)
            {
                foreach (var user in userRepo)
                {
                    userRequest.GetUsers.Add(new UserViewModel
                    {
                        Id = Guid.Parse(user.Id),
                        UserName = user.UserName,
                        Email = user.Email
                    });

                }
            }
            
            return View(userRequest);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserRequest userRequest)
        {
            var identityUser = new IdentityUser
            {
                UserName = userRequest.Username,
                Email = userRequest.Email
            };

            var IdentityResult=await userManager.CreateAsync(identityUser,userRequest.Password);

            if(identityUser is not null)
            {
                if (IdentityResult.Succeeded)
                {
                    var role = new List<String>() {"user"};
                    if (userRequest.CheckBoxForRole)
                    {
                        role.Add("admin");
                    }
                    var identityResult =await userManager.AddToRolesAsync(identityUser, role);

                    if (identityResult is not null)
                    {
                        if(!IdentityResult.Succeeded)
                        {
                            return RedirectToAction("List", "User");
                        }
                    }
                }
            }

            return RedirectToAction("List", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            await userRepository.RemoveAsync(id);
            return RedirectToAction("List", "User");
        }
    }
}
