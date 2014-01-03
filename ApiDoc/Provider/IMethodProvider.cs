using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IMethodProvider
    {
        Method GetByName(string name, int? id);
        Method GetById(int id);
        IList<Method> GetMethods(int? parentId);
        IList<Method> GetRevisions(string name, int parentId);
        Method CompareRevisions(int id, int rev1, int rev2);
    }
}