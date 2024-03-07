using System.ComponentModel.DataAnnotations;
using Data.Enum;
using Data.Utils.SwaggerAnnotation;

namespace Data.DTO {
    public class CreateBlogDto {

        [SwaggerSchemaExample(BlogCategories.Bitcoin)]
        public BlogCategories Category { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be bewteen 5 and 100 characters")]
        [SwaggerSchemaExample("Grand Title of My blog")]
        public string Title { get; set; }

        [StringLength(50, ErrorMessage = "Descritpion must be less than 50 characters")]
        [SwaggerSchemaExample("BlogDescription")]
        public string? Description { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 100, ErrorMessage = "Title must be bewteen 5 and 100 characters")]
        [SwaggerSchemaExample("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.")]
        public string Body { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Image description must be between 5 and 100 characters")]
        [SwaggerSchemaExample("ImageDescription")]
        public string? ImageDescription { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL")]
        [SwaggerSchemaExample("https://i.pravatar.cc/300/433245")]
        public string ImageUrl { get; set; }

        }

    }