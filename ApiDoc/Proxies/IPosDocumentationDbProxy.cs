using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public interface IPosDocumentationDbProxy
    {
        IList<ApiDescription> GetApiDescriptions ();
        IList<ModuleDescription> GetModules(int apiId);
    }
}