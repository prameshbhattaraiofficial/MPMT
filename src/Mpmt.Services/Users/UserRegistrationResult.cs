namespace Mpmt.Services.Users
{
    /// <summary>
    /// The user registration result.
    /// </summary>
    public class UserRegistrationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistrationResult"/> class.
        /// </summary>
        public UserRegistrationResult()
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
