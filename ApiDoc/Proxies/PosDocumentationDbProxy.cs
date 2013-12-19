using System;
using System.Collections.Generic;
using System.Linq;
using ApiDoc.DataAccess;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public class PosDocumentationDbProxy : IPosDocumentationDbProxy
    {
        public IList<ApiDescription> GetApiDescriptions ()
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.GetAPIs().Select(MapApiDescriptions).ToList();
            }
        }

        public IList<ModuleDescription> GetModules(int apiId)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.GetModulesForApi(apiId).Select(MapModuleDescription).ToList();
            }
        }

        private static ModuleDescription MapModuleDescription(GetModulesForApiResult input)
        {
            return new ModuleDescription
                {
                    Id = input.fID,
                    Name = input.fServiceName
                };
        }

        private static ApiDescription MapApiDescriptions(GetAPIsResult input)
        {
            return new ApiDescription
                {
                    Id = input.fID,
                    Name = input.fApiName,
                    Description = input.fDescription
                };
        }
    }
}