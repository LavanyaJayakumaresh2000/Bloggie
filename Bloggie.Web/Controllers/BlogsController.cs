using System.Threading.Tasks;
using Bloggie.Web.Models.Domain;
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
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, 
            IBlogPostlikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
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

                var blogPostComment = await blogPostCommentRepository.GetCommentsByIdAsync(blog.Id);

                var blogCommentview = new List<BlogCommentView>();

                foreach (var blogComment in blogPostComment)
                {
                    blogCommentview.Add(new BlogCommentView
                    {
                        Description = blogComment.Description,
                        Date = blogComment.Date,
                        UserName = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
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
                    Liked = liked,
                    Comment = blogCommentview
                };


            }
            return View(blogPostLikeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogPostLikeViewModel blogPostLikeViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var model = new BlogPostComment()
                {
                    BlogPostId = blogPostLikeViewModel.Id,
                    Description = blogPostLikeViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    Date = DateTime.Now
                };

                await blogPostCommentRepository.AddAsync(model);
                return RedirectToAction("Index", "Blogs",
                    new { urlhandle = blogPostLikeViewModel.UrlHandle });
            }
            return View();
        }
    }
}
