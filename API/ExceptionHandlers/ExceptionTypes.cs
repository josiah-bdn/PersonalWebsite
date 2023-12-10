using System;
using Data.Enum;

namespace API.ExceptionHandlers {
    public class AuthenticationException : AppException {
        public AuthenticationException(string message)
            : base(ErrorCode.AuthenticationError, message) {
        }
    }

    public class ResourceNotFoundException : AppException {
        public ResourceNotFoundException(string message)
            : base(ErrorCode.ResourceNotFound, message) {
        }
    }
}
