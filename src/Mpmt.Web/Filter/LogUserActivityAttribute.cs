using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mpmt.Core.Common.Helpers;
using Mpmt.Services.Services.UserActivityLog;

namespace Mpmt.Web.Filter
{
    /// <summary>
    /// The log user activity attribute.
    /// </summary>
    public class LogUserActivityAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogUserActivityAttribute"/> class.
        /// </summary>
        /// <param name="userAction">The user action.</param>
        /// <param name="logRequestBody">If true, log request body.</param>
        public LogUserActivityAttribute(string userAction = "", bool logRequestBody = true) : base(typeof(LogUserActivityFilter))
        {
            Arguments = new object[] { userAction, logRequestBody };
        }
        #region Nested filter
        /// <summary>
        /// The log user activity filter.
        /// </summary>
        public class LogUserActivityFilter : IAsyncResourceFilter
        {
            private readonly string _userAction;
            private readonly bool _logRequestBody;
            private readonly IUserActivityLog _activityLogService;
            [Obsolete]
            private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnv;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogUserActivityFilter"/> class.
            /// </summary>
            /// <param name="userAction">The user action.</param>
            /// <param name="logRequestBody">If true, log request body.</param>
            /// <param name="activityLogService">The activity log service.</param>
            /// <param name="hostEnv">The host env.</param>
            [Obsolete]
            public LogUserActivityFilter(string userAction, bool logRequestBody, IUserActivityLog activityLogService, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnv)
            {
                _userAction = userAction;
                _logRequestBody = logRequestBody;
                _activityLogService = activityLogService;
                _hostEnv = hostEnv;
            }


            /// <summary>
            /// Ons the resource execution async.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="next">The next.</param>
            /// <returns>A Task.</returns>
            [Obsolete]
            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                await LogUserActivityAsync(context.HttpContext);
                await next();
            }

            /// <summary>
            /// Logs the user activity async.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <returns>A Task.</returns>
            [Obsolete]
            private async Task LogUserActivityAsync(HttpContext context)
            {
                try
                {
                    var userActivityLogParams = await LogHelper.GetUserActivityLogAsync(context, _hostEnv, logRequestBody: _logRequestBody, userAction: _userAction);
                    await _activityLogService.AddAsync(userActivityLogParams);
                }
                catch (Exception) { }
            }
        }
        #endregion
    }
}
