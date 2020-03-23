using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicatorCms.Core
{
    public class SourcePagePropertiesLayout
    {
        public static SourcePagePropertiesLayout Default { get; } = new SourcePagePropertiesLayout();

        public double NavItemSpacerBefore { get; set; } = 0;
        public double NavItemSpacerAfter { get; set; } = 0;

        public string NavItemCssInline { get; set; } = "";
        public string NavItemCssClasses { get; set; } = "";

        public string NavExpandCssInline { get; set; } = "";
        public string NavExpandCssClasses { get; set; } = "";
    }
}
