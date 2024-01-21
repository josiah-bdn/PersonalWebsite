using System;
using Data.DTO;

namespace API.Logic.Services.BlogServiceLogic
{
    public interface IBlogService {
        public Task<GetBlogDto> CreateBlogAsync(Guid userId, CreateBlogDto blog);

        public Task<GetBlogDto> GetBlogAsync(Guid blogId );

        public Task DeleteBlogAsync(Guid userId, Guid blogId);

        public Task<GetBlogDto> UpdateBlogAsync(Guid userId, Guid blogId, UpdateBlogDto blog);

    }
}

