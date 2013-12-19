using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IModulesProvider
    {
        IList<ModuleDescription> GetModules(int apiId);
        ModuleDescription GetModule(int moduleId);
    }
}
