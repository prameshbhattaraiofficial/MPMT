namespace Mpmt.Core.Dtos.Roles
{
    /// <summary>
    /// The app role.
    /// </summary>
    public class AppRole
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        public char Event { get; set; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether system is role.
        /// </summary>
        public bool IsSystemRole { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string LoggedInUser { get; set; }
        ///// <summary>
        ///// Gets or sets the created local date.
        ///// </summary>
        //public DateTime? CreatedLocalDate { get; set; }
        ///// <summary>   
        ///// Gets or sets the created utc date.
        ///// </summary>
        //public DateTime? CreatedUtcDate { get; set; }
        ///// <summary>
        ///// Gets or sets the created nepali date.
        ///// </summary>
        //public string CreatedNepaliDate { get; set; }
        ///// <summary>
        ///// Gets or sets the updated by.
        ///// </summary>
        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public string UserType { get; set; }
        ///// <summary>
        ///// Gets or sets the updated local date.
        ///// </summary>
        //public DateTime? UpdatedLocalDate { get; set; }
        ///// <summary>
        ///// Gets or sets the updated utc date.
        ///// </summary>
        //public DateTime? UpdatedUtcDate { get; set; }
        ///// <summary>
        ///// Gets or sets the updated nepali date.
        ///// </summary>
        //public string UpdatedNepaliDate { get; set; }
        public string AgentCode { get; set; }
    }
}
