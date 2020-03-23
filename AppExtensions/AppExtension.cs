using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicatorCms.Core.AppFileSystem;
using CommunicatorCms.Core.Settings;

namespace CommunicatorCms.Core.AppExtensions
{
    public abstract class AppExtension
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public AppExtension(string id, string name) 
        {
            Id = id;
            Name = name;
        }

        public abstract void OnRequestStart(RequestState requestState);
        public abstract void OnRequestEnd(RequestState requestState);
        
        public abstract void OnGetActionStart(RequestState requestState);
        public abstract void OnGetActionEnd(RequestState requestState);

        public abstract void OnPostActionStart(RequestState requestState);
        public abstract void OnPostActionEnd(RequestState requestState);

        public abstract void OnMainStart(RequestState requestState);
        public abstract void OnMainEnd(RequestState requestState);

        public abstract void OnCmsStart(RequestState requestState);
        public abstract void OnCmsEnd(RequestState requestState);

        public abstract object HeadContent(RequestState requestState);

    }
}
