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

    public class BlogException : AppException {
        public BlogException(string message)
            : base(ErrorCode.BlogError, message) {

            }
        }
    }
