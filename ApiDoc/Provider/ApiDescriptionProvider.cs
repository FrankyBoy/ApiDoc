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

        public IList<ApiDescription> GetApiDescriptions()
        {
            return _proxy.GetApiDescriptions();
        }

        public ApiDescription GetApiDescription(int id)
        {
            return GetApiDescriptions().First(x => x.Id == id);
        }
    }
}