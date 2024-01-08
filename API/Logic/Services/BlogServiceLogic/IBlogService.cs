using System;
using Data.DTO;

namespace API.Logic.Services.BlogServiceLogic
{
    public interface IBlogService {
        public Task CreateBlogAsync(Guid userId, CreateBlogDto blog);

        public Task<GetBlogDto> GetBlogAsync(Guid blogId );

    }
}

