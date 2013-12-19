using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IApiDescriptionProvider
    {
        IList<ApiDescription> GetApis();
        ApiDescription GetApiDescription(int id);
    }
}