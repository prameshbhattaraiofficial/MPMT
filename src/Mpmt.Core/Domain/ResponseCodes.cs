namespace Mpmt.Core.Domain
{
    public static class ResponseCodes
    {
        public const string Code200_Success = "200";
        public const string Code201_Created = "201";
        public const string Code202_Accepted = "202";

        public const string Code400_BadRequest = "400";
        public const string Code401_Unauthorized = "401";
        public const string Code402_PaymentRequired = "402";
        public const string Code403_Forbidden = "403";
        public const string Code404_ResourceNotFound = "404";
        public const string Code405_MethodNotAllowed = "405";
        public const string Code406_NotAcceptable = "406";
        public const string Code407_ProxyAuthenticationRequired = "407";
        public const string Code408_RequestTimeout = "408";
        public const string Code409_Conflict = "409";
        public const string Code410_Gone = "410";
        public const string Code415_UnsupportedMediaType = "415";
        public const string Code422_UnprocessableEntity = "422";

        public const string Code500_InternalServerError = "500";
        public const string Code501_NotImplemented = "501";
        public const string Code502_BadGateway = "502";
        public const string Code503_ServiceUnavailable = "503";
        public const string Code504_GatewayTimeout = "504";


        // Custom Error codes
        public const string CodeE601_InvalidSignatureToken = "E601";
        public const string CodeE602_InvalidProcessId = "E602";
        public const string CodeE603_InvalidEncryption = "E603";


        public static string GetMessageForResponseCode(string code) => code switch
        {
            Code200_Success => ResponseMessages.Msg200_Success,
            Code201_Created => ResponseMessages.Msg201_Created,
            Code202_Accepted => ResponseMessages.Msg202_Accepted,

            Code400_BadRequest => ResponseMessages.Msg400_BadRequest,
            Code401_Unauthorized => ResponseMessages.Msg401_Unauthorized,
            Code402_PaymentRequired => ResponseMessages.Msg402_PaymentRequired,
            Code403_Forbidden => ResponseMessages.Msg403_Forbidden,
            Code404_ResourceNotFound => ResponseMessages.Msg404_ResourceNotFound,
            Code405_MethodNotAllowed => ResponseMessages.Msg405_MethodNotAllowed,
            Code406_NotAcceptable => ResponseMessages.Msg406_NotAcceptable,
            Code407_ProxyAuthenticationRequired => ResponseMessages.Msg407_ProxyAuthenticationRequired,
            Code408_RequestTimeout => ResponseMessages.Msg408_RequestTimeout,
            Code409_Conflict => ResponseMessages.Msg409_Conflict,
            Code410_Gone => ResponseMessages.Msg410_Gone,
            Code415_UnsupportedMediaType => ResponseMessages.Msg415_UnsupportedMediaType,
            Code422_UnprocessableEntity => ResponseMessages.Msg422_UnprocessableEntity,

            Code500_InternalServerError => ResponseMessages.Msg500_InternalServerError,
            Code501_NotImplemented => ResponseMessages.Msg501_NotImplemented,
            Code502_BadGateway => ResponseMessages.Msg502_BadGateway,
            Code503_ServiceUnavailable => ResponseMessages.Msg503_ServiceUnavailable,
            Code504_GatewayTimeout => ResponseMessages.Msg504_GatewayTimeout,

            CodeE601_InvalidSignatureToken => ResponseMessages.MsgE601_InvalidSignatureToken,
            CodeE602_InvalidProcessId => ResponseMessages.MsgE601_InvalidSignatureToken,
            _ => string.Empty
        };
    }
}
