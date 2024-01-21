using System;
using API.Logic.Services.BlogServiceLogic;
using Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    public class BlogController : BaseApiController {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService) {
            _blogService = blogService;
            }

        [Authorize]
        [HttpPost("Createblog")]
        public async Task<GetBlogDto> CreateBlogController(CreateBlogDto blog) {

            return await _blogService.CreateBlogAsync(GetUserId(), blog);

            }

        [Authorize]
        [HttpGet("GetBlog/{blogId}")]
        public async Task<GetBlogDto> GetBlogController(Guid blogId) {
            return await _blogService.GetBlogAsync(blogId);
            }


        [Authorize]
        [HttpDelete("DeleteBlog/{blogId}")]
        public async Task<IActionResult> DeleteBlogController(Guid blogId) {
            await _blogService.DeleteBlogAsync(GetUserId(), blogId);
            return Ok();

            }

        [Authorize]
        [HttpPut("UpdateBlog/{blogId}")]
        public async Task<GetBlogDto> UpdateBlogController(Guid blogId, UpdateBlogDto updateBlog) {
            return await _blogService.UpdateBlogAsync(GetUserId(), blogId, updateBlog);

            }

        }
    }

