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

        public ApiDescription GetApiById(int apiId, int? revision)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapApiDescription(context.Apis_GetById(apiId, revision).First());
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

        public int InsertApi(ApiDescription newApi, string author)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.Apis_Insert(newApi.Name, newApi.Description, author);
            }
        }
        
        public void UpdateApi(ApiDescription model, string author)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                var result = context.Apis_Update(model.Id, model.Name, model.Description, author);
                if(result < 0)
                    throw new Exception("No such ApiId");
            }
        }

        public void DeleteApi(int id)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                context.Apis_Delete(id);
            }
        }

        public IList<ApiDescription> GetApiRevisions(int apiId)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.Apis_GetRevisions(apiId).Select(DbTypeConverter.MapApiDescription).ToList();
            }
        }
    }
}