using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbcontext bloggieDbcontext;

        public TagRepository(BloggieDbcontext bloggieDbcontext)
        {
            this.bloggieDbcontext = bloggieDbcontext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbcontext.Tags.AddAsync(tag);
            await bloggieDbcontext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await bloggieDbcontext.Tags.FindAsync(id);
            if (tag != null)
            {
                bloggieDbcontext.Tags.Remove(tag);
                await bloggieDbcontext.SaveChangesAsync();
                return tag;
            }
            return null;

        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await bloggieDbcontext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await bloggieDbcontext.Tags.FirstOrDefaultAsync(t => t.Id == id); 
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingData = await bloggieDbcontext.Tags.FindAsync(tag.Id);
            if (existingData != null)
            {
                existingData.Name = tag.Name;
                existingData.DisplayName = tag.DisplayName;
                await bloggieDbcontext.SaveChangesAsync();

                return existingData;
            }
            return null;
            
        }

     
    }
}
