using CommunicatorCms.Core.AppFileSystem;
using CommunicatorCms.Core.Helpers;
using CommunicatorCms.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace CommunicatorCms.Core
{
    public class SourcePage
    {
        private static char IgnoreContentFileStartingCharacter = '_';
        private static IDeserializer YamlDeserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();

        public string PageUrl { get; set; } = "";
        public string PageAppPath { get; set; } = "N/A";
        public string Url { get => string.IsNullOrEmpty(Properties.RedirectUrl) ? PageUrl : Properties.RedirectUrl; }

        public RequestState? RequestState { get; set; }
        public SourcePageProperties Properties { get; set; }
        public SourcePagePropertiesLayout PropertiesLayout { get; set; }
        public List<string> ContentFileAppPaths { get => GetContentFileAppPaths(); }

        private List<SourcePage>? _subPages;
        private List<string>? _contentFileAppPaths;

        public SourcePage(SourcePageProperties properties, SourcePagePropertiesLayout propertiesLayout)
        {
            Properties = properties;
            PropertiesLayout = propertiesLayout;
        }

        public static async Task<SourcePage> LoadPageFromUrl(string url, RequestState? requestState = null)
        {
            var appPath = AppPath.Join(GeneralSettings.WebPagesPath, url);
            var sourcePage = await LoadPageFromAppPath(appPath, requestState);

            return sourcePage;
        }
        public static async Task<SourcePage> LoadPageFromAppPath(string appPath, RequestState? requestState = null)
        {
            if (IsAppPathSourcePage(appPath))
            {
                var propertiesFilePath = AppPath.Join(appPath, SourcePageSettings.PropertiesFileName);
                var propertiesLayoutFilePath = AppPath.Join(appPath, SourcePageSettings.PropertiesLayoutFileName);

                var sourcePageProperties = YamlDeserializer.Deserialize<SourcePageProperties>(await AppFile.ReadAllTextAsync(propertiesFilePath));
                var sourcePagePropertiesLayout = SourcePagePropertiesLayout.Default;

                if (AppFile.Exists(propertiesLayoutFilePath)) 
                {
                    sourcePagePropertiesLayout = YamlDeserializer.Deserialize<SourcePagePropertiesLayout>(await AppFile.ReadAllTextAsync(propertiesLayoutFilePath));
                }
                
                var sourcePage = new SourcePage(sourcePageProperties, sourcePagePropertiesLayout);

                sourcePage.PageUrl = AppPath.ConvertAppPathToUrl(appPath);
                sourcePage.PageAppPath = appPath;
                sourcePage.RequestState = requestState;

                return sourcePage;
            }

            return new SourcePage(new SourcePageProperties(), SourcePagePropertiesLayout.Default);
        }

        private static bool IsAppPathSourcePage(string appPath)
        {
            return AppFile.Exists(AppPath.Join(appPath, SourcePageSettings.PropertiesFileName));
        }

        public async Task<List<SourcePage>> GetSubPages()
        {
            if (_subPages == null)
            {
                var subPageAppPaths = GetSubPageAppPaths();

                _subPages = new List<SourcePage>(subPageAppPaths.Count);

                if (RequestState == null)
                {
                    foreach (var subPagePath in subPageAppPaths)
                    {
                        var subPage = await LoadPageFromAppPath(subPagePath);

                        _subPages.Add(subPage);
                    }
                }
                else 
                {
                    foreach (var subPagePath in subPageAppPaths)
                    {
                        var subPage = await RequestState.GetPageByAppPath(subPagePath);

                        _subPages.Add(subPage);
                    }
                }
            }

            return _subPages;
        }

        private List<string> GetSubPageAppPaths() 
        {
            var subPageAppPaths = new List<string>(Properties.SubPageOrder.Count);
            var subPageAppPathsSet = new HashSet<string>(Properties.SubPageOrder.Count);
            var subPageAppPathsAfterEllipsis = new List<string>(Properties.SubPageOrder.Count);

            var currentSubPageAppPathList = subPageAppPaths;

            foreach (var spo in Properties.SubPageOrder)
            {
                var subDirectoryPath = AppPath.Join(PageAppPath, spo);

                if (spo == SourcePageSettings.SubPageOrderEllipsisIdentifier) 
                {
                    currentSubPageAppPathList = subPageAppPathsAfterEllipsis;
                }
                else if (AppDirectory.Exists(subDirectoryPath) && IsAppPathSourcePage(subDirectoryPath))
                {
                    currentSubPageAppPathList.Add(subDirectoryPath);
                    subPageAppPathsSet.Add(subDirectoryPath);
                }
            }

            var subDirectories = AppDirectory.GetDirectories(PageAppPath);

            foreach (var subDirectoryPath in subDirectories)
            {
                if (!subPageAppPathsSet.Contains(subDirectoryPath) && IsAppPathSourcePage(subDirectoryPath))
                {
                    subPageAppPaths.Add(subDirectoryPath);
                }
            }

            subPageAppPaths.AddRange(subPageAppPathsAfterEllipsis);

            return subPageAppPaths;
        }
        private List<string> GetContentFileAppPaths() 
        {
            if (_contentFileAppPaths == null)
            {
                var contentFilePathsSet = new HashSet<string>(Properties.ContentOrder.Count);

                _contentFileAppPaths = new List<string>(Properties.ContentOrder.Count);

                foreach (var co in Properties.ContentOrder) 
                {
                    var contentFilePath = Path.Join(PageAppPath, co);

                    if (AppFile.Exists(contentFilePath)) 
                    {
                        _contentFileAppPaths.Add(contentFilePath);
                        contentFilePathsSet.Add(contentFilePath);
                    }
                }
                var allContentFilePaths = AppDirectory.GetFiles(PageAppPath);

                foreach (var contentFilePath in allContentFilePaths) 
                {
                    var fileName = Path.GetFileName(contentFilePath);

                    if (!fileName.StartsWith(IgnoreContentFileStartingCharacter)) 
                    {
                        if (!contentFilePathsSet.Contains(contentFilePath)) 
                        {
                            _contentFileAppPaths.Add(contentFilePath);
                        }
                    }
                }
            }

            return _contentFileAppPaths;
        }

        public override string ToString()
        {
            return $"Title: {Properties.Title}, Url: {Url}";
        }
    }
}
