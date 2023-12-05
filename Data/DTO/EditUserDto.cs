using System;
namespace Data.DTO
{
    public class EditUserDto
    {
      public Guid AppUserId { get; set; }

      public string? FirstName { get; set; }

      public string? LastName { get; set; }

      public string? UserName { get; set; }
      
    }
}

