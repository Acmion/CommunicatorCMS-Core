using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicatorCms.Core.Settings
{
    public static class RenderingSettings
    {
        public static string CoreUrl { get; set; } = "/Rendering/Core";
        public static string CoreFileName { get; set; } = "Core.cshtml";

        public static string MainFileName { get; set; } = "Main.cshtml";
        public static string GetActionFileName { get; set; } = "GetAction.cshtml";
        public static string PostActionFileName { get; set; } = "PostAction.cshtml";

        public static string HeadContentFileName { get; set; } = "Head.cshtml";

        public static string ContentFileName { get; set; } = "Content.cshtml";
        public static string HeaderContentFileName { get; set; } = "Header.cshtml";
        public static string FooterContentFileName { get; set; } = "Footer.cshtml";
    }
}
