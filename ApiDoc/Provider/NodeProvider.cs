using System.Collections.Generic;
using ApiDoc.DataAccess.Proxies;
using ApiDoc.Models;
using ApiDoc.Utility;

namespace ApiDoc.Provider
{
    public class NodeProvider : INodeProvider
    {
        private readonly IPosDocumentationDbProxy _proxy;

        public NodeProvider(IPosDocumentationDbProxy proxy)
        {
            _proxy = proxy;
        }

        public IList<Node> GetNodes(int? parentId = null, bool showDeleted = false)
        {
            return _proxy.GetNodes(parentId, showDeleted);
        }

        public Node GetByName(string name, int? parentId = null, int? revision = null)
        {
            return _proxy.GetNodeByName(name, parentId, revision);
        }

        public Node GetById(int id, int? revision = null)
        {
            return _proxy.GetNodeById(id, revision);
        }
        
        public int InsertNode(Node newNode)
        {
            return _proxy.InsertNode(newNode);
        }

        public void UpdateNode(Node newNode)
        {
            _proxy.UpdateNode(newNode);
        }

        public void DeleteNode(int id, string author, string reason)
        {
            _proxy.DeleteNode(id, author, reason);
        }

        public IList<Node> GetRevisions(string name, int? parentId = null)
        {
            return _proxy.GetNodeRevisions(name, parentId);
        }

        public Node CompareRevisions(int id, int rev1, int rev2)
        {
            var r1 = GetById(id, rev1);
            var r2 = GetById(id, rev2);
            var diff = new DiffMatchPatch();
            var nameResult = diff.diff_main(r1.Name, r2.Name);
            var descriptionResult = diff.diff_main(r1.Description, r2.Description);
            diff.diff_cleanupSemantic(nameResult);
            diff.diff_cleanupSemantic(descriptionResult);

            return new Node
                {
                    Name = diff.diff_prettyHtml(nameResult),
                    Description = diff.diff_prettyHtml(descriptionResult)
                };
        }
    }
}