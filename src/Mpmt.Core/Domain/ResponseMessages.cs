namespace Mpmt.Core.Domain
{
    public static class ResponseMessages
    {
        public const string Msg200_Success = "Success";
        public const string Msg201_Created = "Created";
        public const string Msg202_Accepted = "Accepted";

        public const string Msg400_BadRequest = "Bad Request";
        public const string Msg401_Unauthorized = "Unauthorized, you are not authorized to access the resource";
        public const string Msg402_PaymentRequired = "Payment Required";
        public const string Msg403_Forbidden = "Forbidden, you don't have permission to access this resource";
        public const string Msg404_ResourceNotFound = "Resource not found";
        public const string Msg405_MethodNotAllowed = "Method Not Allowed";
        public const string Msg406_NotAcceptable = "Not Acceptable";
        public const string Msg407_ProxyAuthenticationRequired = "Proxy Authentication Required (RFC 7235)";
        public const string Msg408_RequestTimeout = "Request Timeout";
        public const string Msg409_Conflict = "A conflict request, resource already exists";
        public const string Msg410_Gone = "Gone, the resource requested is no longer available";
        public const string Msg415_UnsupportedMediaType = "Unsupported Media Type";
        public const string Msg422_UnprocessableEntity = "Unprocessable Entity";
        public const string Msg429_TooManyRequests = "Too Many Requests";
        
        public const string Msg500_InternalServerError = "Internal Server Error, an error occurred while processing your request";
        public const string Msg501_NotImplemented = "Not Implemented";
        public const string Msg502_BadGateway = "Bad Gateway";
        public const string Msg503_ServiceUnavailable = "Service Unavailable";
        public const string Msg504_GatewayTimeout = "Gateway Timeout";

        public const string MsgE601_InvalidSignatureToken = "Invalid signature token";
        public const string MsgE602_InvalidProcessId = "Process ID is invalid or expired";
        public const string MsgE603_InvalidEncryption = "The encrypted data is invalid";
    }
}
