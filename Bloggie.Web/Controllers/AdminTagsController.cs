using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModel;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    [Authorize( Roles = "admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagsRequest addTagsRequest)
        {
            var tag = new Tag
            {
                Name = addTagsRequest.Name,
                DisplayName = addTagsRequest.DisplayName,
            };
            await tagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);
            if(tag != null)
            {
                var editTagsRequest = new EditTagsRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTagsRequest);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagsRequest editTagsRequest)
        {
            var tag = new Tag
            {
                Id = editTagsRequest.Id,
                Name = editTagsRequest.Name,
                DisplayName = editTagsRequest.DisplayName,
            };
            var updatedTag = await tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                //show pass popup
            }
            else
            {
                //show error popup
            }
            return RedirectToAction("Edit", new { id = editTagsRequest.Id });

        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedTag = await tagRepository.DeleteAsync(id);
            if (deletedTag != null)
            {
                //show pass popup
            }
            else
            {
                //show error popup
            }
            return RedirectToAction("List");
            
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagsRequest editTagsRequest)
        {
            var tag = new Tag
            {
                Id = editTagsRequest.Id,
            };
            var deletedTag =  await tagRepository.DeleteAsync(tag.Id);
            if (deletedTag != null)
            {
                //show pass popup
            }
            else
            {
                //show error popup
            }

            return RedirectToAction("List");
        }
    }
}
