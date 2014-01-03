using System;
using System.Collections.Generic;
using System.Linq;
using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
{
    public class PosDocumentationDbProxy : IPosDocumentationDbProxy
    {
        #region HttpVerbs
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
        #endregion

        #region Nodes
        public IList<Node> GetNodes(int? parentId = null, bool showDeleted = false)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.Nodes_GetAll(parentId, showDeleted)
                    .Select(DbTypeConverter.MapNode).ToList();
            }
        }

        public Node GetNodeById(int id, int? revision = null)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapNode(context.Nodes_GetById(id, revision).First());
            }
        }

        public Node GetNodeByName(string name, int? parentId = null, int? revision = null)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return DbTypeConverter.MapNode(context.Nodes_GetByName(parentId, name, revision).First());
            }
        }

        public int GetNodeId(string name, int? parentId = null)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                int? result = 0;
                context.Nodes_LookupId(parentId, name, ref result);
                return result ?? -1;
            }
        }

        public int InsertNode(Node newNode)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.Nodes_Insert(newNode.ParentId, newNode.Name, newNode.Description, newNode.Author, newNode.ChangeNote);
            }
        }

        public int UpdateNode(Node newNode)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.Nodes_Update(
                    newNode.ParentId, newNode.Id, newNode.Name,
                    newNode.Description, newNode.Author, newNode.ChangeNote);
            }
        }

        public void DeleteNode(int id, string author, string reason)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                context.Nodes_Delete(id, author, reason);
            }
        }

        public IList<Node> GetNodeRevisions(string name, int? parentId = null)
        {
            using (var context = new PosDocumentationDbDataContext())
            {
                return context.Nodes_GetRevisions(parentId, name)
                    .Select(DbTypeConverter.MapNode).ToList();
            }
        }

        #endregion

    }
}