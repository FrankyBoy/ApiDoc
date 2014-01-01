using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IMethodProvider
    {
        Method GetByName(string name, int? id);
        IList<Method> GetMethods(int? parentId);
    }
}