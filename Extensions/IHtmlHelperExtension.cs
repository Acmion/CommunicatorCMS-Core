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
        public static Task RenderActiveOrDefaultAsync(this IHtmlHelper<dynamic> htmlHelper, string activePath, string defaultPath)
        {
            if (AppFile.Exists(activePath))
            {
                return htmlHelper.RenderPartialAsync(activePath);
            }
            else if (AppFile.Exists(defaultPath))
            {
                return htmlHelper.RenderPartialAsync(defaultPath);
            }

            return Task.CompletedTask;
        }
    }
}
