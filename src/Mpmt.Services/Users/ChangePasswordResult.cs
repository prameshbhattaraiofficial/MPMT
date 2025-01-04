namespace Mpmt.Services.Users
{
    /// <summary>
    /// The change password result.
    /// </summary>
    public class ChangePasswordResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordResult"/> class.
        /// </summary>
        public ChangePasswordResult()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Gets a value indicating whether success.
        /// </summary>
        public bool Success => !Errors.Any();

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void AddError(string error) => Errors.Add(error);

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public IList<string> Errors { get; set; }
    }
}
