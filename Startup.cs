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
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YamlDotNet.Serialization;

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

            services.AddScoped<RequestState>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            env.ContentRootPath = App.RootPath;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            ConfigureRewriteOptions(app);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        private void ConfigureRewriteOptions(IApplicationBuilder app) 
        {
            var a = $"{RenderingSettings.CoreUrl}?{QuerySettings.ResourceParameter}=$0&{QuerySettings.RenderingResourceParameter}={RenderingSettings.MainFileName}";
            var rewriteOptions = new RewriteOptions()
                .AddRewrite(@"(.*)\.cshtml$", "$1", false)
                .AddRewrite("static/.*", "$0", true)
                .AddRewrite("Error/.*", "/Rendering/Error", true)
                .AddRewrite("actions/get/.*", $"{RenderingSettings.CoreUrl}?{QuerySettings.ResourceParameter}=$0&{QuerySettings.RenderingResourceParameter}={RenderingSettings.GetActionFileName}", true)
                .AddRewrite("Actions/Get/.*", $"{RenderingSettings.CoreUrl}?{QuerySettings.ResourceParameter}=$0&{QuerySettings.RenderingResourceParameter}={RenderingSettings.GetActionFileName}", true)
                .AddRewrite("(.*)", $"{RenderingSettings.CoreUrl}?{QuerySettings.ResourceParameter}=$0&{QuerySettings.RenderingResourceParameter}={RenderingSettings.MainFileName}", false);

            app.UseRewriter(rewriteOptions);

        }

        
    }
}
