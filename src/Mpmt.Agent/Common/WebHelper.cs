namespace Mpmt.Agent.Common;

public static class WebHelper
{
    public static bool IsAjaxRequest(HttpRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (request.Headers is null)
            return false;

        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
