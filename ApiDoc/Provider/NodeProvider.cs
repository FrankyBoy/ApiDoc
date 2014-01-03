using System.Collections.Generic;
using ApiDoc.DataAccess.Proxies;
using ApiDoc.Models;
using ApiDoc.Utility;

namespace ApiDoc.Provider
{
    public class NodeProvider : INodeProvider
    {
        private readonly IPosDocumentationDbProxy _proxy;
        private readonly DiffMatchPatch _diff = new DiffMatchPatch();

        public NodeProvider(IPosDocumentationDbProxy proxy)
        {
            _proxy = proxy;
        }

        public IList<Node> GetNodes(int? parentId = 0, bool showDeleted = false)
        {
            return _proxy.GetNodes(parentId, showDeleted);
        }

        public Node GetByName(string name, int? parentId = 0, int? revision = null)
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

        public IList<Node> GetRevisions(string name, int? parentId = 0)
        {
            return _proxy.GetNodeRevisions(name, parentId);
        }

        public Node CompareRevisions(int id, int rev1, int rev2)
        {
            var r1 = GetById(id, rev1);
            var r2 = GetById(id, rev2);

            return new Node
            {
                Name = GetPrettyHtmlDiff(r1.Name, r2.Name),
                Description = GetPrettyHtmlDiff(r1.Description, r2.Description),
            };
        }

        private string GetPrettyHtmlDiff(string text1, string text2)
        {
            var diffs = _diff.diff_main(text1, text2);
            _diff.diff_cleanupSemantic(diffs);
            return _diff.diff_prettyHtml(diffs);
        }
    }
}