namespace Mpmt.Core.Dtos.Pagination
{
    /// <summary>
    /// The action specs.
    /// </summary>
    public class ActionSpecs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSpecs"/> class.
        /// </summary>
        public ActionSpecs()
        {

        }
        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        public virtual string Controller { get; set; }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public virtual string Action { get; set; }
    }
}
