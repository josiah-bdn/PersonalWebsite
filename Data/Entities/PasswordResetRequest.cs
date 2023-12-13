using System;
namespace Data.Entities
{
    public class PasswordResetRequest
    {

       public Guid PasswordRequestId { get; set; }
       
       public Guid AppUserId { get; set; }

       public int Code { get; set; }

       public DateTime SendDate { get; set; }

       public bool UserHasValidated { get; set; } = false;

       public bool IsExpiredOrFailed { get; set; } = false;

       public int CodeEntryCount { get; set; } = 0;

       public AppUser AppUser { get; set; }

    }
}

