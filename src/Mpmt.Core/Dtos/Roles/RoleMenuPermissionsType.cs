using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.Roles
{
    /// <summary>
    /// The role menu permissions type.
    /// </summary>
    public class RoleMenuPermissionsType
    {
        /// <summary>
        /// Gets or sets the menu id.
        /// </summary>
        [Required]
        public int MenuId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether view per.
        /// </summary>
        [Required]
        public bool ViewPer { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether create per.
        /// </summary>
        [Required]
        public bool CreatePer { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether update per.
        /// </summary>
        [Required]
        public bool UpdatePer { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether delete per.
        /// </summary>
        [Required]
        public bool DeletePer { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}
