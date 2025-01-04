namespace Mpmt.Core.ViewModel.RoleModuleAction
{
    /// <summary>
    /// The role module action model view.
    /// </summary>
    public class RoleModuleActionModelView
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Gets or sets the module id.
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// Gets or sets the action id.
        /// </summary>
        public int ActionId { get; set; }
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public string Action { get; set; }

    }
}
