using System.Collections.Generic;
using ApiDoc.Models;
using ApiDoc.Proxies;
using ApiDoc.Utility;

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

        public ApiDescription CompareRevisions(int apiId, int rev1, int rev2)
        {
            var r1 = GetById(apiId, rev1);
            var r2 = GetById(apiId, rev2);
            var diff = new DiffMatchPatch();
            var nameResult = diff.diff_main(r1.Name, r2.Name);
            var descriptionResult = diff.diff_main(r1.Description, r2.Description);
            diff.diff_cleanupSemantic(nameResult);
            diff.diff_cleanupSemantic(descriptionResult);

            return new ApiDescription
                {
                    Name = diff.diff_prettyHtml(nameResult),
                    Description = diff.diff_prettyHtml(descriptionResult)
                };
        }
    }
}