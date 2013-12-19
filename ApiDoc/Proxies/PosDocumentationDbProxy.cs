using System;
using System.Collections.Generic;
using System.Linq;
using ApiDoc.DataAccess;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public class PosDocumentationDbProxy : IPosDocumentationDbProxy
    {
        
        public IList<ApiDescription> GetApis ()
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.GetAPIs().Select(DbTypeConverter.MapApiDescription).ToList();
            }
        }

        public ApiDescription GetApiByName(string name)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapApiDescription(context.GetApiByName(name).First());
            }
        }

        public IList<ModuleDescription> GetModules(int apiId)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.GetModulesForApi(apiId).Select(DbTypeConverter.MapModuleDescription).ToList();
            }
        }

        public ModuleDescription GetModuleByName(int apiId, string name)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapModuleDescription(context.GetModuleByName(name, apiId).First());
            }
        }

        public IList<MethodDescription> GetMethods(int moduleId)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.GetMethodsForService(moduleId).Select(DbTypeConverter.MapMethodDescription).ToList();
            }
        }

        public MethodDescription GetMethodById(int methodId)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapMethodDescription(context.GetMethodDetails(methodId).First());
            }
        }

        public MethodDescription GetMethodByName(int moduleId, string name)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapMethodDescription(context.GetMethodByName(moduleId, name).First());
            }
        }

        private IDictionary<int, string> _cachedHttpMethods;
        private DateTime _cachedHttpMethodsExpiry;

        public IDictionary<int, string> GetHttpMethods()
        {
            if (_cachedHttpMethods == null && DateTime.UtcNow < _cachedHttpMethodsExpiry)
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