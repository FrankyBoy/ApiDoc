using System;
using System.Collections.Generic;
using System.Linq;
using ApiDoc.DataAccess;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public class PosDocumentationDbProxy : IPosDocumentationDbProxy
    {
        
        public IList<ApiDescription> GetApis (bool showDeleted = false)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.Apis_GetAll(showDeleted).Select(DbTypeConverter.MapApiDescription).ToList();
            }
        }

        public ApiDescription GetApiByName(string name, int? revision = null)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapApiDescription(context.Apis_GetByName(name, revision).First());
            }
        }

        public IList<ModuleDescription> GetModules(int apiId, bool showDeleted = false)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.Modules_GetAll(apiId, showDeleted).Select(DbTypeConverter.MapModuleDescription).ToList();
            }
        }

        public ModuleDescription GetModuleByName(int apiId, string name, int? revision = null)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapModuleDescription(context.Modules_GetByName(apiId, name, revision).First());
            }
        }

        private IDictionary<int, string> _cachedHttpMethods;
        private DateTime _cachedHttpMethodsExpiry;

        public IDictionary<int, string> GetHttpMethods()
        {
            if (_cachedHttpMethods == null || DateTime.UtcNow > _cachedHttpMethodsExpiry)
            {
                using (var context = new PosDocumentationDbDataContext())
                {
                    _cachedHttpMethods = context.GetHttpVerbs().ToDictionary(x => x.fID, x => x.fHttpVerb);
                }
                _cachedHttpMethodsExpiry = DateTime.UtcNow.AddHours(1);
            }

            return _cachedHttpMethods;
        }
    }
}