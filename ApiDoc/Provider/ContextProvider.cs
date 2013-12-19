using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiDoc.Models;
using ApiDoc.Utility;

namespace ApiDoc.Provider
{
    public class ContextProvider : IContextProvider
    {
        private readonly IModulesProvider _modulesProvider;
        private readonly IApiDescriptionProvider _apiDescriptionProvider;

        public ContextProvider(IModulesProvider modulesProvider, IApiDescriptionProvider apiDescriptionProvider)
        {
            _modulesProvider = modulesProvider;
            _apiDescriptionProvider = apiDescriptionProvider;
        }

        public ApiDescription GetApiId(string apiUrl)
        {
            apiUrl = apiUrl.FromWikiUrlString();
            var apis = _apiDescriptionProvider.GetApiDescriptions();

            return apis.First(x => x.Name.EqualsIgnoreCase(apiUrl));
        }

        public Tuple<ApiDescription, ModuleDescription> GetModuleId(string apiUrl, string moduleUrl)
        {
            var api = GetApiId(apiUrl);

            moduleUrl = moduleUrl.FromWikiUrlString();
            var module = _modulesProvider.GetModules(api.Id).First(x => x.Name.EqualsIgnoreCase(moduleUrl));

            return new Tuple<ApiDescription, ModuleDescription>(api, module);
        }
    }
}