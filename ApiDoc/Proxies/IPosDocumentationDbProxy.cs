using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Proxies
{
    public interface IPosDocumentationDbProxy
    {
        IDictionary<int, string> GetHttpMethods();

        IList<Node> GetNodes(int? parentId = null, bool showDeleted = false);
        Node GetNodeById(int id, int? revision = null);
        Node GetNodeByName(string name, int? parentId = null, int? revision = null);
        int GetNodeId(string name, int? parentId = null);
        int InsertNode(Node newNode, int? parentId);
        int UpdateNode(Node newNode, int? parentId);
        void DeleteNode(int id, string author, string reason);
        IList<Node> GetNodeRevisions(string name, int? parentId = null);
    }
}