using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunicatorCms.Core;
using CommunicatorCms.Core.AppExtensions;
using CommunicatorCms.Core.Helpers;
using CommunicatorCms.Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace CommunicatorCms
{
    public class App
    {
        public static string RootPath { get; } = GetAppRootPath();
        public static CultureInfo AmericanCultureInfo { get; } = CultureInfo.GetCultureInfo("en-US");

        public static void Main(string[] args)
        {
            WebSampleInstaller.InstallIfEmpty();
            LoadSettings();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) 
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        public static void LoadSettings()
        {
            var yamlDeserializer = new DeserializerBuilder().Build();

            return; 

            var generalSettings = yamlDeserializer.Deserialize<Dictionary<string, object>>(File.ReadAllText("Settings/GeneralSettings.yaml"));
            var sourcePageSettings = yamlDeserializer.Deserialize<Dictionary<string, object>>(File.ReadAllText("Settings/SourcePageSettings.yaml"));

            ReflectionHelper.PopulatePublicStaticProperties(typeof(GeneralSettings), generalSettings);
            ReflectionHelper.PopulatePublicStaticProperties(typeof(SourcePageSettings), sourcePageSettings);
        }

        private static string GetAppRootPath([CallerFilePath] string sourceFilePath = "")
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(sourceFilePath))!.Replace('\\', '/');
        }

        
        
    }

}
