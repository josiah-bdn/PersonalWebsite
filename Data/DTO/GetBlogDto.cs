using System;
namespace Data.DTO {
    public class GetBlogDto : CreateBlogDto {
        public Guid BlogId { get; set; }

        public Guid AppUserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        }
    }

