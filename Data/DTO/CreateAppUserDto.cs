using System;
namespace Data.DTO
{
    public class CreateAppUserDto
    {
        public string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string UserName { get; set; }
    }
}

