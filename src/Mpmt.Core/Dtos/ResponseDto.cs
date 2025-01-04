namespace Mpmt.Core.Dtos
{
    /// <summary>
    /// The response dto.
    /// </summary>
    public class ResponseDto
    {
        private List<string> _errors = new();
        private string _message = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether success.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get => _message; set => _message = value ?? string.Empty; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public List<string> Errors { get => _errors; set => _errors = value ?? new(); }
    }
}
