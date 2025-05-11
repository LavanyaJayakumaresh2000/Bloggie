using System.Threading.Tasks;
using Bloggie.Web.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerModel.Username,
                Email = registerModel.Email,
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerModel.Password);
            
            if(identityResult.Succeeded)
            {
                var identityUserRole = await userManager.AddToRoleAsync(identityUser, "user");

                if(identityUserRole.Succeeded)
                {
                    return RedirectToAction("Register");
                }
                
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var login = new LoginModel()
            {
                ReturnUrl = returnUrl,
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var signIn = await signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password,false,false);

            if (signIn.Succeeded && signIn!= null)
            {

                if(!string.IsNullOrEmpty(loginModel.ReturnUrl))
                {
                    return Redirect(loginModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
