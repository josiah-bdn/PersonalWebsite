using System;
using Data.Enum;

namespace API.ExceptionHandlers;

public class AppException : System.Exception {
    public ErrorCode ErrorCode { get; }

    public AppException(ErrorCode errorCode, string message)
        : base(message) {
        ErrorCode = errorCode;
    }
}
