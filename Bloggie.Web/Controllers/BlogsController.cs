using System.Threading.Tasks;
using Bloggie.Web.Models.ViewModel;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostlikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public BlogsController(IBlogPostRepository blogPostRepository, 
            IBlogPostlikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlhandle)
        {
            var blog = await blogPostRepository.GetBlogsByUrlhandleAsync(urlhandle);
            var liked = false;
            var blogPostLikeViewModel = new BlogPostLikeViewModel();

            if(blog != null)
            {
                var totalLikes = await blogPostLikeRepository.GetAllLikesAsync(blog.Id);

                if (signInManager.IsSignedIn(User))
                {
                    var likesForPost = await blogPostLikeRepository.GetAllLikesForBlogPost(blog.Id);

                    var userId = userManager.GetUserId(User);
                    if (Guid.TryParse(userId, out var user))
                    {
                        
                        var Likedpost =likesForPost.FirstOrDefault(x => x.UserId == user);
                        liked = Likedpost!=null;
                    }
                }


                blogPostLikeViewModel = new BlogPostLikeViewModel
                {
                    Id = blog.Id,
                    Content = blog.Content,
                    PublishedDate = blog.PublishedDate,
                    ShortDescription = blog.ShortDescription,
                    Author = blog.Author,
                    PageTitle = blog.PageTitle,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    Heading = blog.Heading,
                    Tags = blog.Tags,
                    UrlHandle = urlhandle,
                    Visible = blog.Visible,
                    TotalLikes = totalLikes,
                    Liked = liked
                };


            }
            return View(blogPostLikeViewModel);
        }
    }
}
