using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunicatorCms.Core;
using CommunicatorCms.Core.AppFileSystem;
using CommunicatorCms.Core.Helpers;
using CommunicatorCms.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using CommunicatorCms.Core.UrlModification;
using YamlDotNet.Serialization;
using Microsoft.Net.Http.Headers;

namespace CommunicatorCms
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            dynamic ext = new Extension();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(opt => opt.LowercaseUrls = true)
                    .AddHttpClient()
                    .AddRazorPages()
                    .AddRazorRuntimeCompilation()
                    .WithRazorPagesRoot(GeneralSettings.WebRootPath);

            services.AddHttpContextAccessor();

            services.AddScoped<RequestState>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            env.ContentRootPath = App.RootPath;

            var staticFileCacheTimeSpan = TimeSpan.FromDays(1);

            if (env.IsDevelopment())
            {
                staticFileCacheTimeSpan = TimeSpan.FromSeconds(10);

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseUrlRewrites();
            //ConfigureRewriteOptions(app);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(App.RootPath + "/Web"),
                RequestPath = "",
                OnPrepareResponse = ctx =>
                {
                    var headers = ctx.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = staticFileCacheTimeSpan
                    };
                },
                ServeUnknownFileTypes = true, // Security risk according to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-3.1, but unlikely
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        private void ConfigureRewriteOptions(IApplicationBuilder app) 
        {
            var a = $"{RenderingSettings.InitializerUrl}?{QuerySettings.ResourceParameter}=$0&{QuerySettings.RenderingResourceParameter}={RenderingSettings.WwwFileName}";
            var rewriteOptions = new RewriteOptions()
                .AddRewrite("static/.*", "$0", true)
                .AddRedirect(".*[^/]$", "$0/")
                .AddRewrite(@"(.*)\.cshtml$", "$1", false)
                .AddRewrite("Error/.*", "/Rendering/Error", true)
                .AddRewrite("actions/get/.*", $"{RenderingSettings.InitializerUrl}?{QuerySettings.ResourceParameter}=$0&{QuerySettings.RenderingResourceParameter}={RenderingSettings.ActionFileName}", true)
                .AddRewrite("Actions/Get/.*", $"{RenderingSettings.InitializerUrl}?{QuerySettings.ResourceParameter}=$0&{QuerySettings.RenderingResourceParameter}={RenderingSettings.ActionFileName}", true)
                .AddRewrite("(.*)", $"{RenderingSettings.InitializerUrl}?{QuerySettings.ResourceParameter}=$0&{QuerySettings.RenderingResourceParameter}={RenderingSettings.WwwFileName}", false);

            app.UseRewriter(rewriteOptions);

        }

        
    }

    public class MyViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "~/Web/{0}.cshtml",
                "~/Web/{0}"
            }; // add `.Union(viewLocations)` to add default locations
        }
    }
}
