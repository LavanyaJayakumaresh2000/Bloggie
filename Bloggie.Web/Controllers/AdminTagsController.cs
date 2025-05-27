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
            ValidateAddTagRequest(addTagsRequest);
            if(ModelState.IsValid == false)
            {
                return View();
            }
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
        public async Task<IActionResult> List(string searchQuery, string sortBy, string sortingField,
            int pagenumber = 1,int pageSize = 3)
        {
            var totalCountOfTags = await tagRepository.CountAsync();

            var tags = await tagRepository.GetAllAsync(searchQuery, sortBy,sortingField,pagenumber,pageSize);
            var totalPage = Math.Ceiling((double)totalCountOfTags / pageSize);

            if(pagenumber > totalPage)
            {
                pagenumber--;
            }
            if(pagenumber < 1)
            {
                pagenumber++;
            }
            ViewBag.TotalPage = totalPage;
            ViewBag.PageNumber = pagenumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortingField = sortingField;

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

        private void ValidateAddTagRequest(AddTagsRequest addTagsRequest)
        {
            if (addTagsRequest.Name is not null && addTagsRequest.DisplayName is not null)
            {
                if (addTagsRequest.Name == addTagsRequest.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name and DisplayName should not be same");
                }
            }
            
        }
        
    }
}
