using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IApiDescriptionProvider
    {
        IList<ApiDescription> GetApis(bool showDeleted = false);
        ApiDescription GetByName(string apiName);
        ApiDescription GetById(int apiId, int? revision = null);
        int InsertApi(ApiDescription newApi, string author);
        void UpdateApi(ApiDescription model, string author);
    }
}