using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicatorCms.Core.Settings
{
    public static class RenderingSettings
    {
        public static string InitializerFileName { get; } = "_initializer.cshtml";

        public static string WwwFileName { get; } = "www.cshtml";
        public static string ActionFileName { get; } = "action.cshtml";

        public static string HeadFileName { get; } = "head.cshtml";
        public static string HeaderFileName { get; } = "header.cshtml";
        public static string ContentFileName { get; } = "content.cshtml";
        public static string FooterFileName { get; } = "footer.cshtml";

        public static string InitializerUrl { get; } = "/_rendering/_initializer";

    }
}
