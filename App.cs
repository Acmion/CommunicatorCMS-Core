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
using CommunicatorCms.Core.AppFileSystem;
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
        public static IDeserializer YamlDeserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
        

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
            LoadSettingsClass(typeof(AppSettings), "/Settings/AppSettings.yaml");
            return;

            var sourcePageSettings = YamlDeserializer.Deserialize<Dictionary<string, object>>(AppFile.ReadAllText("Settings/SourcePageSettings.yaml"));

            ReflectionHelper.PopulatePublicStaticProperties(typeof(SourcePageSettings), sourcePageSettings);
        }

        private static void LoadSettingsClass(Type settingsType, string settingsFileAppPath) 
        {
            if (AppFile.Exists(settingsFileAppPath))
            {
                var settings = YamlDeserializer.Deserialize<Dictionary<string, object>>(AppFile.ReadAllText(settingsFileAppPath));
                ReflectionHelper.PopulatePublicStaticProperties(settingsType, settings);
            }

        }

        private static string GetAppRootPath([CallerFilePath] string sourceFilePath = "")
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(sourceFilePath))!.Replace('\\', '/');
        }

        
        
    }

}
