using System;
using ApiDoc.Models;
using ApiDoc.Proxies;
using ApiDoc.Utility;

namespace ApiDoc.Provider
{
    public class ContextProvider : IContextProvider
    {
        private readonly IPosDocumentationDbProxy _proxy;

        public ContextProvider(IPosDocumentationDbProxy proxy)
        {
            _proxy = proxy;
        }

        public ApiDescription GetApiByName(string apiUrl)
        {
            apiUrl = apiUrl.FromWikiUrlString();
            return _proxy.GetApiByName(apiUrl);
        }

        public Tuple<ApiDescription, ModuleDescription> GetModuleByName(string apiUrl, string moduleUrl)
        {
            var api = GetApiByName(apiUrl);

            moduleUrl = moduleUrl.FromWikiUrlString();
            var module = _proxy.GetModuleByName(api.Id, moduleUrl);

            return new Tuple<ApiDescription, ModuleDescription>(api, module);
        }


        public Tuple<ApiDescription, ModuleDescription, MethodDescription> GetMethodByName(string apiUrl, string moduleUrl, string methodUrl)
        {
            var module = GetModuleByName(apiUrl, moduleUrl);

            methodUrl = methodUrl.FromWikiUrlString();
            var method = _proxy.GetMethodByName(module.Item2.Id, methodUrl);

            return new Tuple<ApiDescription, ModuleDescription, MethodDescription>(module.Item1, module.Item2, method);
        }
    }
}