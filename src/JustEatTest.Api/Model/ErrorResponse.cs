using System;
using System.Collections.Generic;

namespace JustEatTest.Api.Model
{
    public class ErrorResponse
    {
        public ErrorResponse(string requestId, string errorType, IEnumerable<string> errorCodes = null)
        {
            RequestId = requestId;
            ErrorType = errorType;
            ErrorCodes = errorCodes;
        }
        
        public string RequestId { get; }
        public string ErrorType {get;}
        public IEnumerable<string> ErrorCodes {get;}
    }
}