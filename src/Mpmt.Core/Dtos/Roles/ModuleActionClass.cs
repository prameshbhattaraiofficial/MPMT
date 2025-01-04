namespace Mpmt.Core.Dtos.Roles
{
    /// <summary>
    /// The module action.
    /// </summary>
    public class ModuleActionClass
    {
        /// <summary>
        /// Gets or sets the module id.
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// Gets or sets the action id.
        /// </summary>
        public int ActionId { get; set; }
        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// Gets or sets the action nane.
        /// </summary>
        public string Action { get; set; }

        public string RoleName { get; set; }


    }
}
