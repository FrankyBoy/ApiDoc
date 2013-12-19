using System;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IContextProvider
    {
        ApiDescription GetApiByName(string apiUrl);
        Tuple<ApiDescription, ModuleDescription> GetModuleByName(string apiUrl, string moduleUrl);
    }
}