using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Authentication
    {
        [Key]
        public Guid AppUserId { get; set; }

        public string HashPassword { get; set; }

        public string PasswordSalt { get; set; }

        public int LoginCount { get; set; }

        public DateTime LastLogin { get; set; }

        public AppUser AppUser { get; set; }
    }
}

