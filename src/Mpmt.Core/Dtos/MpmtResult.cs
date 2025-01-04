namespace Mpmt.Core.Dtos
{
    /// <summary>
    /// The mpmt result.
    /// </summary>
    public class MpmtResult
    {
        private int? _resultCode;
        private string _message;

        /// <summary>
        /// Adds success result code with message
        /// </summary>
        /// <param name="resultCode">Success result code</param>
        /// <param name="message">Success message</param>
        public void AddSuccess(int resultCode, string message = null)
        {
            ResultCode = resultCode;
            Message = message;
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void AddError(string error) => Errors.Add(error);

        /// <summary>
        /// Adds errors.
        /// </summary>
        /// <param name="errors"></param>
        public void AddErrors(params string[] errors) => Errors.AddRange(errors);

        /// <summary>
        /// Adds errors with error status code.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="error">The error.</param>
        public void AddError(int errorCode, string error)
        {
            ResultCode = errorCode;
            Errors.Add(error);
        }

        /// <summary>
        /// Adds errors with error status code
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errors"></param>
        public void AddErrors(int errorCode, params string[] errors)
        {
            ResultCode = errorCode;
            if (errors.Any()) Errors.AddRange(errors);
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public List<string> Errors { get; private set; } = new();

        /// <summary>
        /// Gets a value indicating whether success.
        /// </summary>
        public bool Success => !Errors.Any();

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public int ResultCode
        {
            get => _resultCode ?? (Success ? 200 : 400);
            private set => _resultCode = value;
        }

        /// <summary>
        /// Gets or Sets Result meassage
        /// </summary>
        public string Message
        {
            get => _message ?? string.Empty;
            private set => _message = value;
        }

    }
}
