using System;
using Data.Enum;

namespace Data.DTO {
    public class CreateBlogDto {
        public BlogCategories Category { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public string Body { get; set; }

        public string? ImageDescription { get; set; }

        public string ImageUrl { get; set; }

        }

    }