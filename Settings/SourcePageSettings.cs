using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicatorCms.Core.Settings
{
    public static class SourcePageSettings
    {
        public static string SubPageOrderEllipsisIdentifier { get; } = "...";

        public static string PropertiesFileName { get; private set; } = "_Properties.yaml";
        public static string PropertiesLayoutFileName { get; private set; } = "_PropertiesLayout.yaml";
    }

}
