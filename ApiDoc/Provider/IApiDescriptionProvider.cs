using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface IApiDescriptionProvider
    {
        IList<ApiDescription> GetApiDescriptions();
        ApiDescription GetApiDescription(int id);
    }
}