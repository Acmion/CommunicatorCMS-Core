using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicatorCms.Core.AppFileSystem;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CommunicatorCms.Core.Extensions
{
    public static class IHtmlHelperExtension
    {
        public static Task RenderPartialAsyncFromUrl(this IHtmlHelper htmlHelper, string url)
        {
            return htmlHelper.RenderPartialAsync(AppUrl.ConvertToAppPath(url));
        }

        public static Task RenderActiveOrDefaultAsyncFromUrl(this IHtmlHelper htmlHelper, string activeUrl, string defaulturl)
        {
            if (AppUrl.Exists(activeUrl))
            {
                return htmlHelper.RenderPartialAsyncFromUrl(activeUrl);
            }
            else if (AppUrl.Exists(defaulturl))
            {
                return htmlHelper.RenderPartialAsyncFromUrl(defaulturl);
            }

            return Task.CompletedTask;
        }
    }
}
