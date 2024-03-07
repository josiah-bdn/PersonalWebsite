using System;
using Data.Utils.SwaggerAnnotation;

namespace Data.DTO
{
    public class LoginDto
    {
        [SwaggerSchemaExample("bigjohn@mailinator.com")]
        public required string Email { get; set; }

        [SwaggerSchemaExample("Password1!")]
        public required string Password { get; set; }
    }
}

