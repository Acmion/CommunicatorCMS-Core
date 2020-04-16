using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommunicatorCms.Core.AppFileSystem;
using CommunicatorCms.Core.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CommunicatorCms.Core.AppExtensions
{
    public static class AppExtensionHandler
    {
        public static dynamic Extensions => _appExtensionContainer;
        public static int ExtensionCount => _appExtensionContainer.AppExtensionCount;

        private static AppExtensionContainer _appExtensionContainer { get; set; } = new AppExtensionContainer();

        public static void RegisterAppExtension(AppExtension appExtension) 
        {
            _appExtensionContainer.RegisterAppExtension(appExtension);
        }

        public static void ClearAppExtensions()
        {
            _appExtensionContainer.Clear();
        }
        public static async void LoadAppExtensions(IHtmlHelper htmlHelper) 
        {
            ClearAppExtensions();

            /*var extensionAppDirs = AppDirectory.GetDirectories(GeneralSettings.WebExtensionsPath);

            foreach (var extAppDir in extensionAppDirs) 
            {
                var loadExtensionFilePath = AppPath.Join(extAppDir, GeneralSettings.WebExtensionsLoadFileName);

                if (AppFile.Exists(loadExtensionFilePath)) 
                {
                    await htmlHelper.RenderPartialAsync(loadExtensionFilePath);
                }
            }
            */
        }

        public static void OnRequestStart(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnRequestStart(requestState);
            }
        }
        public static void OnRequestEnd(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnRequestEnd(requestState);
            }
        }

        public static void OnGetActionStart(RequestState requestState) 
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnGetActionStart(requestState);
            }
        }
        public static void OnGetActionEnd(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnGetActionEnd(requestState);
            }
        }

        public static void OnPostActionStart(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnPostActionStart(requestState);
            }
        }
        public static void OnPostActionEnd(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnPostActionEnd(requestState);
            }
        }

        public static void OnMainStart(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnMainStart(requestState);
            }
        }
        public static void OnMainEnd(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnMainEnd(requestState);
            }
        }

        public static void OnCmsStart(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnCmsStart(requestState);
            }
        }
        public static void OnCmsEnd(RequestState requestState)
        {
            foreach (var appExtension in _appExtensionContainer.AppExtensions)
            {
                appExtension.OnCmsEnd(requestState);
            }
        }

        private class AppExtensionContainer : DynamicObject
        {
            public int AppExtensionCount => AppExtensionsById.Count;
            public ICollection<AppExtension> AppExtensions => AppExtensionsById.Values;

            public IDictionary<string, AppExtension> AppExtensionsById { get; set; } = new Dictionary<string, AppExtension>();
            
            private IDictionary<string, object> _members { get; set; } = new Dictionary<string, object>();

            public void Clear()
            {
                _members.Clear();
                AppExtensionsById.Clear();
            }

            public void RegisterAppExtension(AppExtension appExtension) 
            {
                _members[appExtension.Id] = appExtension;
                AppExtensionsById[appExtension.Id] = appExtension;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                _members[binder.Name] = value;
                if (value is AppExtension appExt) 
                {
                    AppExtensionsById[appExt.Id] = appExt;
                }

                return true;
            }
            public override bool TryGetMember(GetMemberBinder binder, out object? result)
            {
                return _members.TryGetValue(binder.Name, out result);
            }

        }
    }

    
}
