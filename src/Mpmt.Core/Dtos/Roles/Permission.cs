namespace Mpmt.Core.Dtos.Roles
{
    /// <summary>
    /// The permission.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether viewper.
        /// </summary>
        public bool viewper { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether createper.
        /// </summary>
        public bool Createper { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether updateper.
        /// </summary>
        public bool Updateper { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether deleteper.
        /// </summary>
        public bool Deleteper { get; set; }
        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Gets or sets the menuid.
        /// </summary>
        public int menuid { get; set; }
    }
}
