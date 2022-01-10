using System;
using System.Net;

namespace DeviceApi.Infrastructure.CrossCutting.Exceptions
{
    public class ApiErrorException : Exception
    {
        public ErrorCodes ErrorCode { get; }

        public HttpStatusCode StatusCode { get; }

        public ApiErrorException()
        {
            ErrorCode = ErrorCodes.Generic;
            StatusCode = HttpStatusCode.BadRequest;
        }

        public ApiErrorException(string message, ErrorCodes errorCode, HttpStatusCode statusCode) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public ApiErrorException(string message) : base(message)
        {
            ErrorCode = ErrorCodes.Generic;
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
