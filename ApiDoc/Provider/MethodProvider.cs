using System.Collections.Generic;
using ApiDoc.Models;
using ApiDoc.Proxies;

namespace ApiDoc.Provider
{
    class MethodProvider : IMethodProvider
    {
        private readonly IPosDocumentationDbProxy _proxy;

        public MethodProvider(IPosDocumentationDbProxy proxy)
        {
            _proxy = proxy;
        }

        public IList<MethodDescription> GetMethods(int moduleId)
        {
            return _proxy.GetMethods(moduleId);
        }

        public MethodDescription GetMethodById(int methodId)
        {
            return _proxy.GetMethodById(methodId);
        }
    }
}