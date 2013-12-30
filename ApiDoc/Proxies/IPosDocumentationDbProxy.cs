using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public interface IPosDocumentationDbProxy
    {
        IList<ApiDescription> GetApis(bool showDeleted = false);
        ApiDescription GetApiByName(string name, int? revision = null);
        ApiDescription GetApiById(int apiId, int? revision);

        IList<ModuleDescription> GetModules(int apiId, bool showDeleted = false);
        ModuleDescription GetModuleByName(int apiId, string name, int? revision = null);

        IDictionary<int, string> GetHttpMethods();
        int InsertApi(ApiDescription newApi, string author);
        void UpdateApi(ApiDescription model, string author);
        void DeleteApi(int id);
        IList<ApiDescription> GetApiRevisions(int apiId);
    }
}