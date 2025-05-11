using System.Threading.Tasks;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModel;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostlikeRepository blogPostlikeRepository;

        public BlogPostLikeController(IBlogPostlikeRepository blogPostlikeRepository)
        {
            this.blogPostlikeRepository = blogPostlikeRepository;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLikes([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new BlogPostLike
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId,
            };
            await blogPostlikeRepository.AddLikeForBlogAsync(model);
            return Ok();
        }

        [HttpGet]
        [Route("{BlogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikeForBlog([FromRoute] Guid BlogPostId)
        {
            var totalLikes = await blogPostlikeRepository.GetAllLikesAsync(BlogPostId);
            return Ok(totalLikes);
        }
    }
}
