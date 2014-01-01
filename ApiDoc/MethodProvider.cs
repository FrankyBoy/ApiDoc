using System.Collections.Generic;
using ApiDoc.Models;
using ApiDoc.Provider;
using ApiDoc.Proxies;

namespace ApiDoc
{
    public class MethodProvider : IMethodProvider
    {
        private IPosDocumentationDbProxy _proxy;

        public MethodProvider(IPosDocumentationDbProxy proxy)
        {
            _proxy = proxy;
        }

        public Method GetByName(string name, int? id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Method> GetMethods(int? parentId)
        {
            throw new System.NotImplementedException();
        }
    }
}