using System.Collections.Generic;
using System.Linq;
using ApiDoc.Models;
using ApiDoc.Proxies;

namespace ApiDoc.Provider
{
    public class ModulesProvider : IModulesProvider
    {
        private readonly IPosDocumentationDbProxy _proxy;

        public ModulesProvider(IPosDocumentationDbProxy proxy)
        {
            _proxy = proxy;
        }

        public IList<ModuleDescription> GetModules(int apiId)
        {
            return _proxy.GetModules(apiId);
        }

        public ModuleDescription GetModule(int moduleId)
        {
            return GetModules(0).First(x => x.Id == moduleId);
        }
    }
}