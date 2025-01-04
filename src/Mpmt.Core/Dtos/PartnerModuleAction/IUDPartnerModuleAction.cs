namespace Mpmt.Core.Dtos.PartnerModuleAction
{
    public class IUDPartnerModuleAction
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the module id.
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// Gets or sets the action id.
        /// </summary>
        public int ActionId { get; set; }
        public int[] ActionIds { get; set; }
        /// <summary>
        /// Gets or sets the action nane.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
