using System;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase {

        protected Guid GetUserId() {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out Guid userId)) {
                return userId;
            }

            throw new InvalidOperationException("User ID is not valid.");
        }

    }
}

