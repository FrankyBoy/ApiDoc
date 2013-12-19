using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public interface IPosDocumentationDbProxy
    {
        IList<ApiDescription> GetApis();
        ApiDescription GetApiByName(string name);

        IList<ModuleDescription> GetModules(int apiId);
        ModuleDescription GetModuleByName(int apiId, string name);

        IList<MethodDescription> GetMethods(int moduleId);
        MethodDescription GetMethodById(int methodId);
        MethodDescription GetMethodByName(int moduleId, string name);

        IDictionary<int, string> GetHttpMethods();
    }
}