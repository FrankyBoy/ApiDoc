using System;
using System.Collections.Generic;
using System.Linq;
using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
{
    public class ApiDocDbProxy : IApiDocDbProxy
    {
        #region HttpVerbs
        private IDictionary<int, string> _cachedHttpMethods;
        private DateTime _cachedHttpMethodsExpiry;

        public IDictionary<int, string> GetHttpMethods()
        {
            if (_cachedHttpMethods == null || DateTime.UtcNow > _cachedHttpMethodsExpiry)
            {
                using (var context = new ApiDocDbDataContext())
                {
                    _cachedHttpMethods = context.GetHttpVerbs().ToDictionary(x => x.fID, x => x.fHttpVerb);
                }
                _cachedHttpMethodsExpiry = DateTime.UtcNow.AddHours(1);
            }

            return _cachedHttpMethods;
        }
        #endregion

        #region Nodes
        public IList<Branch> GetBranches(int? parentId = 0, bool showDeleted = false)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Nodes_GetAll(parentId, showDeleted)
                    .Select(DbTypeConverter.MapNode).ToList();
            }
        }

        public Branch GetBranchById(int id, int? revision = null)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return DbTypeConverter.MapNode(context.Nodes_GetById(id, revision).First());
            }
        }

        public Branch GetBranchByName(string name, int? parentId = 0, int? revision = null)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return DbTypeConverter.MapNode(context.Nodes_GetByName(parentId, name, revision).First());
            }
        }

        public int GetBranchId(string name, int? parentId = 0)
        {
            using (var context = new ApiDocDbDataContext())
            {
                int? result = 0;
                context.Nodes_LookupId(parentId, name, ref result);
                return result ?? -1;
            }
        }

        public int InsertBranch(Branch newBranch)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Nodes_Insert(newBranch.ParentId, newBranch.Name, newBranch.Description, newBranch.Author, newBranch.ChangeNote);
            }
        }

        public int UpdateBranch(Branch newBranch)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Nodes_Update(
                    newBranch.ParentId, newBranch.Id, newBranch.Name,
                    newBranch.Description, newBranch.Author, newBranch.ChangeNote);
            }
        }

        public void DeleteBranch(int id, string author, string reason)
        {
            using (var context = new ApiDocDbDataContext())
            {
                context.Nodes_Delete(id, author, reason);
            }
        }

        public IList<Branch> GetBranchRevisions(string name, int? parentId = 0)
        {
            using (var context = new ApiDocDbDataContext())
            {
                return context.Nodes_GetRevisions(parentId, name)
                    .Select(DbTypeConverter.MapNode).ToList();
            }
        }

        public IList<Leaf> GetLeafes(int? parentId, bool showDeleted = false)
        {
            return new List<Leaf>();
            throw new NotImplementedException();
        }

        public IList<Leaf> GetLeafRevisions(string name, int? parentId)
        {
            throw new NotImplementedException();
        }

        public int InsertLeaf(Leaf newLeaf)
        {
            throw new NotImplementedException();
        }

        public void UpdateLeaf(Leaf newLeaf)
        {
            throw new NotImplementedException();
        }

        public Node GetLeafByName(string name, int? parentId, int? revision = null)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}