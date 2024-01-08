using System;
namespace Data.Entities {
    public class AppUser {

        public Guid AppUserId { get; set; }

        public string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Authentication Authentication { get; set; }

        public ICollection<PasswordResetRequest> PasswordResetRequests { get; set; }

        public ICollection<Blog>? Blogs { get; set; }

    }

}