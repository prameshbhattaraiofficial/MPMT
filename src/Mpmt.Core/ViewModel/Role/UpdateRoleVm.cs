namespace Mpmt.Core.ViewModel.Role
{
    /// <summary>
    /// The update role vm.
    /// </summary>
    public class UpdateRoleVm
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
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public string LoggedInUser { get; set; }
        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public string UserType { get; set; }
        public string AgentCode { get; set; }
    }
}
