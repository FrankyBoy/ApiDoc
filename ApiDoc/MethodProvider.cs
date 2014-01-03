using System.Collections.Generic;
using ApiDoc.DataAccess.Proxies;
using ApiDoc.Models;
using ApiDoc.Provider;

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

        public Method GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Method> GetMethods(int? parentId)
        {
            return new List<Method>();
            throw new System.NotImplementedException();
        }

        public IList<Method> GetRevisions(string name, int parentId)
        {
            throw new System.NotImplementedException();
        }

        public Method CompareRevisions(int id, int rev1, int rev2)
        {
            throw new System.NotImplementedException();
        }
    }
}