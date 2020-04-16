using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicatorCms.Core.Settings
{
    public static class UrlSettings
    {
        public static string RenderingRootUrl { get; } = "/_rendering";

        public static string WwwRootUrl { get; } = "/www";
        public static string CmsRootUrl { get; } = "/cms";
        public static string ThemesRootUrl { get; } = "/themes";
        public static string GeneralRootUrl { get; } = "/general";
        public static string ActionsRootUrl { get; } = "/actions";
        public static string ExtensionsRootUrl { get; } = "/extensions";

        public static HashSet<string> RootUrlsWithNonVirtualPaths = new HashSet<string>() { CmsRootUrl, ThemesRootUrl, GeneralRootUrl, ActionsRootUrl, ExtensionsRootUrl, RenderingRootUrl };
    }
}
