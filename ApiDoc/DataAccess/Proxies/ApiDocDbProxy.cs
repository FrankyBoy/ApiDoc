using System;
using System.Collections.Generic;
using System.Linq;
using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
{
    public class ApiDocDbProxy : IApiDocDbProxy
    {
        #region HttpVerbs
        private HttpVerbs _cachedHttpVerbs;
        private DateTime _cachedVerbsExpiry;

        public HttpVerbs GetHttpVerbs()
        {
            if (_cachedHttpVerbs == null || DateTime.UtcNow > _cachedVerbsExpiry)
            {
                using (var context = new ApiDocDbDataContext())
                {
                    _cachedHttpVerbs = new HttpVerbs(
                        context.GetHttpVerbs().ToDictionary(x => x.fID, x => x.fHttpVerb));
                }
                _cachedVerbsExpiry = DateTime.UtcNow.AddHours(1);
            }

            return _cachedHttpVerbs;
        }
        #endregion

        #region Branches
        public IList<Branch> GetBranches(int? parentId = 0, bool showDeleted = false)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Nodes_GetAll(parentId, showDeleted)
                    .Select(DbTypeConverter.MapBranch).ToList();
            }
        }
        
        public Branch GetBranchByName(string name, int parentId = 0, int? revision = null)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return DbTypeConverter.MapBranch(context.Nodes_GetByName(parentId, name, revision).First());
            }
        }
        
        public int InsertBranch(Branch newBranch)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Nodes_Insert(
                    newBranch.ParentId, newBranch.Name, newBranch.Description,
                    newBranch.Author, newBranch.ChangeNote);
            }
        }

        public int UpdateBranch(Branch newBranch)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Nodes_Update(
                    newBranch.ParentId, newBranch.Id, newBranch.Name, newBranch.Description,
                    newBranch.Author, newBranch.ChangeNote);
            }
        }

        public void DeleteBranch(int id, string author, string reason)
        {
            using (var context = new ApiDocDbDataContext())
            {
                context.Nodes_Delete(id, author, reason);
            }
        }

        public IList<Branch> GetBranchRevisions(string name, int parentId = 0)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Nodes_GetRevisions(parentId, name)
                    .Select(DbTypeConverter.MapBranch).ToList();
            }
        }
        #endregion

        #region Leafes
        public IList<Leaf> GetLeafes(int parentId, bool showDeleted = false)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Leafes_GetAll(parentId, showDeleted)
                    .Select(x => DbTypeConverter.MapLeaf(x, GetHttpVerbs())).ToList();
            }
        }

        public Leaf GetLeafByName(int parentId, string name, int? httpVerb, int? revision = null)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return DbTypeConverter.MapLeaf(
                    context.Leafes_GetByName(parentId, name, httpVerb, revision).First(),
                    GetHttpVerbs());
            }
        }

        public int InsertLeaf(Leaf newLeaf)
        {
            using (var context = new ApiDocDbDataContext())
            {
                var httpVerb = GetHttpVerbs().IdForName(newLeaf.HttpVerb);
                return context.Leafes_Insert(newLeaf.ParentId, newLeaf.Name, httpVerb, newLeaf.Description,
                    newLeaf.RequiresAuthentication, newLeaf.RequiresAuthorization, newLeaf.SampleRequest, newLeaf.SampleResponse,
                    newLeaf.Author, newLeaf.ChangeNote);
            }
        }

        public void UpdateLeaf(Leaf newLeaf)
        {
            using (var context = new ApiDocDbDataContext())
            {
                var httpVerb = GetHttpVerbs().IdForName(newLeaf.HttpVerb);
                context.Leafes_Update(newLeaf.ParentId, newLeaf.Id, newLeaf.Name, httpVerb, newLeaf.Description,
                    newLeaf.RequiresAuthentication, newLeaf.RequiresAuthorization, newLeaf.SampleRequest, newLeaf.SampleResponse,
                    newLeaf.Author, newLeaf.ChangeNote);
            }
        }

        public void DeleteLeaf(int id, string author, string reason)
        {
            using (var context = new ApiDocDbDataContext())
            {
                context.Leafes_Delete(id, author, reason);
            }
        }

        public IList<Leaf> GetLeafRevisions(int parentId, string name, int? httpVerb)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Leafes_GetRevisions(parentId, name, httpVerb)
                    .Select(x => DbTypeConverter.MapLeaf(x, GetHttpVerbs())).ToList();
            }
        }


        #endregion

    }
}