namespace Mpmt.Core.Domain
{
    public class ApiResponse
    {
        private string _responseDetailMessage;
        private string _responseMessage;
        private List<FieldError> _fieldErrors;
        private string _responseCode;
        private string _responseStatus;

        public string ResponseCode
        {
            get => _responseCode ?? string.Empty;
            set => _responseCode = value;
        }

        public string ResponseStatus
        {
            get => _responseStatus ?? ResponseStatuses.Error;
            set => _responseStatus = value;
        }

        public string ResponseMessage
        {
            get => _responseMessage ?? ResponseCodes.GetMessageForResponseCode(ResponseCode);
            set => _responseMessage = value;
        }

        public string ResponseDetailMessage
        {
            get => _responseDetailMessage ?? string.Empty;
            set => _responseDetailMessage = value;
        }

        public List<FieldError> FieldErrors
        {
            get => _fieldErrors ?? new();
            set => _fieldErrors = value;
        }
    }
}
