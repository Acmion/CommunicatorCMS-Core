using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunicatorCms.Core.Settings;

namespace CommunicatorCms.Core.AppFileSystem
{
    public static class AppUrl
    {
        public const char Separator = '/';
        public const string SeparatorString = "/";
        public const string UnservablePrefix = "/_";

        public static string ConvertToAppPath(string url)
        {
            if (UrlSettings.RootUrlsWithNonVirtualPaths.Contains(RootUrl(url)))
            {
                return AppPath.Join(GeneralSettings.WebRootPath, url);
            }

            return AppPath.Join(GeneralSettings.WebRootPath, UrlSettings.WwwRootUrl, url);
        }
        public static string ConvertToAbsolutePath(string url)
        {
            if (UrlSettings.RootUrlsWithNonVirtualPaths.Contains(RootUrl(url)))
            {
                return AppPath.Join(App.RootPath, GeneralSettings.WebRootPath, url);
            }

            return AppPath.Join(App.RootPath, GeneralSettings.WebRootPath, UrlSettings.WwwRootUrl, url);
        }

        public static bool Exists(string url) 
        {
            var appPath = ConvertToAppPath(url);
            return AppFile.Exists(appPath) || AppDirectory.Exists(appPath);
        }
        public static bool IsFile(string url) 
        {
            return AppFile.Exists(ConvertToAppPath(url));
        }
        public static bool IsDirectory(string url) 
        {
            return AppDirectory.Exists(ConvertToAppPath(url));
        }
        public static bool IsDirectlyServable(string url) 
        {
            return !(url.Contains(UnservablePrefix) || url.Contains(".cshtml"));
        }

        public static string RootUrl(string url)
        {
            if (url == SeparatorString)
            {
                return url;
            }
            else if (url.StartsWith(Separator))
            {
                var indexOfNextSeparator = url.IndexOf(Separator, 1);

                if (indexOfNextSeparator > 0)
                {
                    return url.Substring(0, indexOfNextSeparator);
                }
                else
                {
                    return url;
                }
            }
            else if (url.Contains(Separator))
            {
                var indexOfSeparator = url.IndexOf(Separator);
                return url.Substring(0, indexOfSeparator);
            }

            return url;
        }
        public static string Join(params string[] urls) 
        {
            return string.Join(Separator, urls).Replace("//", SeparatorString);
        }
    }
}
