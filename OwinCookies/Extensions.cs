using Microsoft.Owin;

namespace OwinCookies
{
    public static class Extensions
    {
        public static bool IsAjaxRequest(this IOwinRequest request)
        {         
           IHeaderDictionary headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}