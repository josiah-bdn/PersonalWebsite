using System;
using Data.DTO;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Logic.Services.BlogServiceLogic {
    public class BlogService : IBlogService {
        private readonly DataContext _db;
        public BlogService(DataContext db) {
            _db = db;
            }

        public async Task CreateBlogAsync(Guid userId, CreateBlogDto blog) {

            if (blog is null) throw new System.Exception();

            var newBlog = new Blog {
                BlogId = Guid.NewGuid(),
                AppUserId = userId,
                Category = blog.Category,
                Title = blog.Title,
                Description = blog.Description,
                Body = blog.Body,
                ImageDescription = blog.ImageDescription,
                ImageUrl = blog.ImageUrl,
                Created = DateTime.UtcNow
                };

            _db.Add(newBlog);
           await _db.SaveChangesAsync();

            }

        public async Task<GetBlogDto> GetBlogAsync(Guid blogId) {
            var blog = await _db.Blog.Where(b => b.BlogId == blogId).FirstOrDefaultAsync();

            if (blog is null) throw new System.Exception();

            return new GetBlogDto {
                BlogId = blog.BlogId,
                AppUserId = blog.AppUserId,
                Category = blog.Category,
                Title = blog.Title,
                Body = blog.Title,
                Description = blog.Description,
                Created = blog.Created,
                Modified = blog.Modified,
                ImageDescription = blog.ImageDescription,
                ImageUrl = blog.ImageUrl,

                };
            }

        }
    }

