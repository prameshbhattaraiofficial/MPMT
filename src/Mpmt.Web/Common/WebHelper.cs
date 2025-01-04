namespace Mpmt.Web.Common
{
    /// <summary>
    /// The web helper.
    /// </summary>
    public static class WebHelper
    {
        /// <summary>
        /// Are the ajax request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A bool.</returns>
        public static bool IsAjaxRequest(HttpRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers is null)
                return false;

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
