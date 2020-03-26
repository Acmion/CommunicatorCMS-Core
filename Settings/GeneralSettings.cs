using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicatorCms.Core.Settings
{
    public static class GeneralSettings
    {
        public static string WebRootPath { get; private set; } = "/Web";
        public static string WebRenderingPath { get; private set; } = "/Web/Rendering";
        public static string WebActionsPath { get; private set; } = "/Web/Actions/";
        public static string WebActionsGetPath { get; private set; } = "/Web/Actions/Get";
        public static string WebPagesPath { get; private set; } = "/Web/Pages";
        public static string WebThemesPath { get; private set; } = "/Web/Themes";
        public static string WebExtensionsPath { get; private set; } = "/Web/Extensions";
        public static string WebFooterPath { get; set; } = "/Web/General/Footer";
        public static string WebHeadPath { get; set; } = "/Web/General/Head";

        public static string WebExtensionsLoadFileName { get; private set; } = "LoadExtension.cshtml";

    }
}
