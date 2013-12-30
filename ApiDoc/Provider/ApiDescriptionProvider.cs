using System.Collections.Generic;
using ApiDoc.Models;
using ApiDoc.Proxies;

namespace ApiDoc.Provider
{
    public class ApiDescriptionProvider : IApiDescriptionProvider
    {
        private readonly IPosDocumentationDbProxy _proxy;

        public ApiDescriptionProvider(IPosDocumentationDbProxy proxy)
        {
            _proxy = proxy;
        }

        public IList<ApiDescription> GetApis(bool showDeleted)
        {
            return _proxy.GetApis(showDeleted);
        }

        public int InsertApi(ApiDescription newApi, string author)
        {
            return _proxy.InsertApi(newApi, author);
        }

        public ApiDescription GetByName(string apiName)
        {
            return _proxy.GetApiByName(apiName);
        }

        public ApiDescription GetById(int apiId, int? revision = null)
        {
            return _proxy.GetApiById(apiId, revision);
        }

        public void UpdateApi(ApiDescription model, string author)
        {
            _proxy.UpdateApi(model, author);
        }

        public void DeleteApi(int id)
        {
            _proxy.DeleteApi(id);
        }

        public IList<ApiDescription> GetRevisions(int apiId)
        {
            return _proxy.GetApiRevisions(apiId);
        }

    }
}