using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicatorCms.Core.AppFileSystem;
using CommunicatorCms.Core.Extensions;
using CommunicatorCms.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CommunicatorCms.Core.UrlModification
{
    public static class UrlRewrites
    {
        public static void UseUrlRewrites(this IApplicationBuilder app) 
        {
            app.Use(async (context, next) =>
            {
                var requestedUrl = AppUrl.ConvertToActualUrl(context.Request.Path.Value);

                if (!AppUrl.IsDirectlyServable(requestedUrl)) 
                {
                    return;
                }

                if (AppUrl.IsFile(requestedUrl))
                {
                    context.Request.Path = requestedUrl;
                }
                else
                {
                    context.Request.Path = RenderingSettings.InitializerUrl;
                    context.Request.QueryString = new QueryString($"?{QuerySettings.ResourceParameter}={requestedUrl}&{QuerySettings.RenderingResourceParameter}={RenderingSettings.WwwFileName}");
                }

                await next();
            });
        }

    }
}
