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
        public async Task<IActionResult> CreateBlogController(CreateBlogDto blog) {

            await _blogService.CreateBlogAsync(GetUserId(), blog);
            return Ok();
            }

        [Authorize]
        [HttpGet("GetBlog/{blogId}")]
        public async Task<GetBlogDto> GetBlogController(Guid blogId) {
            return await _blogService.GetBlogAsync(blogId);
            }
        }

    }

