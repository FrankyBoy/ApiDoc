using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IMethodProvider
    {
        IList<MethodDescription> GetMethods(int moduleId);
        MethodDescription GetMethodById(int methodId);
    }
}
