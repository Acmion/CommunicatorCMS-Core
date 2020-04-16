using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommunicatorCms.Core.AppFileSystem;
using CommunicatorCms.Core.Settings;
using Microsoft.AspNetCore.Http;

namespace CommunicatorCms.Core
{
    public class RequestState
    {
        public dynamic Dynamic { get; set; } = new ExpandoObject();
        public string Url => HttpRequest.Query[QuerySettings.ResourceParameter];
        public HttpRequest HttpRequest => _httpContextAccessor.HttpContext.Request;

        private IHttpContextAccessor _httpContextAccessor;
        private Dictionary<string, SourcePage> _pageByUrl;

        public RequestState(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
            _pageByUrl = new Dictionary<string, SourcePage>();
        }

        public async Task<SourcePage> GetCurrentPage() 
        {
            var pageUrl = HttpRequest.Query[QuerySettings.ResourceParameter];

            return await GetPageByUrl(pageUrl);
        }

        public async Task<SourcePage> GetPageByUrl(string url)
        {
            if (!_pageByUrl.ContainsKey(url))
            {
                var sourcePage = await SourcePage.LoadPageFromUrl(url, this);

                _pageByUrl[url] = sourcePage;
                _pageByUrl[sourcePage.Url] = sourcePage;
            }

            return _pageByUrl[url];
        }
        public async Task<SourcePage> GetPageByAppPath(string appPath)
        {
            var url = AppPath.ConvertToUrl(appPath);

            return await GetPageByUrl(url);
        }

    }
}
