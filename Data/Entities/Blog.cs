using System.ComponentModel.DataAnnotations.Schema;
using Data.Enum;

namespace Data.Entities {
    public class Blog {
        public Guid BlogId { get; set; }

        [ForeignKey("AppUser")]
        public Guid AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public BlogCategories Category { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public string Body { get; set; }

        public string? ImageDescription { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        }
    }

