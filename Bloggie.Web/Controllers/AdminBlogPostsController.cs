using System.Threading.Tasks;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModel;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tag = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tag.Select(x=> new SelectListItem { Text=x.Name,Value=x.Id.ToString()})
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blog = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };

            var tag = new  List<Tag>();
            foreach(var selectedtag in addBlogPostRequest.SelectedList)
            {
                var selectedTag = Guid.Parse(selectedtag);
                var existingTag = await tagRepository.GetAsync(selectedTag);

                if (existingTag != null)
                {
                    tag.Add(existingTag);
                }
            }
            blog.Tags = tag;
            await blogPostRepository.AddAsync(blog);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPost = await blogPostRepository.GetAllAsync();
            return View(blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blog = await blogPostRepository.GetAsync(id);
            var tag = await tagRepository.GetAllAsync();

            if (blog != null)
            {
                var blogPost = new EditBlogPostRequest
                {
                    Id = blog.Id,
                    Author = blog.Author,
                    PublishedDate = blog.PublishedDate,
                    ShortDescription = blog.ShortDescription,
                    Heading = blog.Heading,
                    UrlHandle = blog.UrlHandle,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    Content = blog.Content,
                    PageTitle = blog.PageTitle,
                    Visible = blog.Visible,
                    Tags = tag.Select(x=> new SelectListItem
                    {
                        Text =x.Name,
                        Value=x.Id.ToString()
                    }),
                    SelectedTags = blog.Tags.Select(x=> x.Id.ToString()).ToArray(),

                   
                };
                return View(blogPost);
            }
            
            
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var tag = new List<Tag>();
            var blog = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Author = editBlogPostRequest.Author,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                Heading = editBlogPostRequest.Heading,
                Visible = editBlogPostRequest.Visible,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,


            };
            foreach (var tagItem in editBlogPostRequest.SelectedTags)
            {
                var selectedtags = Guid.Parse(tagItem);
                var tagDetails = await tagRepository.GetAsync(selectedtags);
                if(tagDetails != null)
                {
                    tag.Add(tagDetails);
                }
            }
            blog.Tags = tag;
            var updatedBlog = await blogPostRepository.UpdateAsync(blog);
            if(updatedBlog != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await blogPostRepository.DeleteAsync(id);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var blog = new BlogPost
            {
                Id = editBlogPostRequest.Id
            };
            await blogPostRepository.DeleteAsync(blog.Id);
            return RedirectToAction("List");
        }
    }

}
