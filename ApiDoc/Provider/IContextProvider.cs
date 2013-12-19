using System;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IContextProvider
    {
        ApiDescription GetApiId(string apiUrl);
        Tuple<ApiDescription, ModuleDescription> GetModuleId(string apiUrl, string moduleUrl);
    }
}