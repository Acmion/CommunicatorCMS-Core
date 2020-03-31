using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicatorCms.Core
{
    public class SourcePageProperties
    {
        public string Icon { get; set; } = "";
        public string Title { get; set; } = "Unknown";
        public string RedirectUrl { get; set; } = "";


        public string Layout { get; set; } = "";
        public bool ShowInNavigationMenus { get; set; } = true;
        public bool ShowTopNavigationMenu { get; set; } = true;
        public bool ShowSideNavigationMenu { get; set; } = true;

        public List<string> SubPageOrder { get; set; } = new List<string>();
        public List<string> SubPageOrderFromEnd { get; set; } = new List<string>();

        public List<string> ContentOrder { get; set; } = new List<string>();


    }
}
