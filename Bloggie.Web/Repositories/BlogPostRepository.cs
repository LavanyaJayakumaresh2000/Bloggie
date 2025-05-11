using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbcontext bloggieDbcontext;

        public BlogPostRepository(BloggieDbcontext bloggieDbcontext)
        {
            this.bloggieDbcontext = bloggieDbcontext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbcontext.BlogPosts.AddAsync(blogPost);
            await bloggieDbcontext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var blog = await bloggieDbcontext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
            if (blog != null)
            {
                bloggieDbcontext.BlogPosts.Remove(blog);
                await bloggieDbcontext.SaveChangesAsync();
                return blog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
             return await bloggieDbcontext.BlogPosts.Include(x=> x.Tags).ToListAsync();
            
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await bloggieDbcontext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Id == id);
            
        }


        public async Task<BlogPost?> GetBlogsByUrlhandleAsync(string urlHandle)
        {
            return await bloggieDbcontext.BlogPosts.Include(x=>x.Tags)
                .FirstOrDefaultAsync(x=>x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var blog = await bloggieDbcontext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
           
            if(blog != null)
            {
                blog.Author = blogPost.Author;
                blog.PageTitle = blogPost.PageTitle;
                blog.Content = blogPost.Content;
                blog.ShortDescription = blogPost.ShortDescription;
                blog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                blog.UrlHandle = blogPost.UrlHandle;
                blog.PublishedDate = blogPost.PublishedDate;
                blog.Author = blogPost.Author;
                blog.Visible = blogPost.Visible;
                blog.Heading = blogPost.Heading;
                blog.Tags = blogPost.Tags;
                
                await bloggieDbcontext.SaveChangesAsync();
                return blog;
            }
            return null;
        }
    }
}
