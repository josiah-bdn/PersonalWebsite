using System;
using API.ExceptionHandlers;
using AutoMapper;
using Data.DTO;
using Data.Entities;
using Data.Enum;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Logic.Services.BlogServiceLogic {
    public class BlogService : IBlogService {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public BlogService(DataContext db, IMapper mapper) {
            _db = db;
            _mapper = mapper;
            }

        public async Task<GetBlogDto> CreateBlogAsync(Guid userId, CreateBlogDto blog) {

            if (blog is null) throw new AppException(ErrorCode.BlogError, "Please enter all required blog details");

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

            return new GetBlogDto {
                BlogId = newBlog.BlogId,
                AppUserId = newBlog.AppUserId,
                Category = newBlog.Category,
                Title = newBlog.Title,
                Body = newBlog.Title,
                Description = blog.Description,
                Created = newBlog.Created,
                Modified = newBlog.Modified,
                ImageDescription = newBlog.ImageDescription,
                ImageUrl = newBlog.ImageUrl,

                };

            }

        public async Task<GetBlogDto> GetBlogAsync(Guid blogId) {
            var blog = await CheckForExistingBlog(blogId);

            if (blog.IsDeleted is true) throw new AppException(ErrorCode.BlogError, "Blog is not found");

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

        public async Task DeleteBlogAsync(Guid userId, Guid blogId) {
            var blog = await CheckForExistingBlog(blogId);

            AuthorizedToModifyBlog(blog.AppUserId, userId);

            blog.IsDeleted = true;
            _db.Update(blog);
            await _db.SaveChangesAsync();

            }

        public async Task<GetBlogDto> UpdateBlogAsync(Guid userId, Guid blogId, UpdateBlogDto updateBlog) {
            var blog = await CheckForExistingBlog(blogId);
            AuthorizedToModifyBlog(blog.AppUserId, userId);

            _mapper.Map(updateBlog, blog);
            await _db.SaveChangesAsync();

            return new GetBlogDto {
                AppUserId = userId,
                BlogId = blog.BlogId,
                Modified = blog.Modified,
                Created = blog.Created,
                Category = blog.Category,
                Title = blog.Title,
                Description = blog.Description,
                Body = blog.Body,
                ImageDescription = blog.ImageDescription,
                ImageUrl = blog.ImageUrl
                };

            }

        private void AuthorizedToModifyBlog(Guid blogUserId, Guid userId) {
            if (userId != blogUserId) throw new AppException(ErrorCode.BlogError, "User is not authorized to modify blog");
            }

        private async Task<Blog> CheckForExistingBlog(Guid blogId) {
            var blog = await _db.Blog.Where(b => b.BlogId == blogId).FirstOrDefaultAsync();

            if (blog is null) throw new AppException(ErrorCode.BlogError, "Blog is not found");

            return blog;

            }

        }
    }

