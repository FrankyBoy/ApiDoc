using System.Collections.Generic;
using System.Linq;
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

        public IList<ApiDescription> GetApis()
        {
            return _proxy.GetApis();
        }

        public ApiDescription GetApiDescription(int id)
        {
            return GetApis().First(x => x.Id == id);
        }
    }
}