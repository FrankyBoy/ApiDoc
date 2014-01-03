using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
{
    public interface IPosDocumentationDbProxy
    {
        IDictionary<int, string> GetHttpMethods();

        IList<Node> GetNodes(int? parentId = 0, bool showDeleted = false);
        Node GetNodeById(int id, int? revision = null);
        Node GetNodeByName(string name, int? parentId = 0, int? revision = null);
        int GetNodeId(string name, int? parentId = 0);
        int InsertNode(Node newNode);
        int UpdateNode(Node newNode);
        void DeleteNode(int id, string author, string reason);
        IList<Node> GetNodeRevisions(string name, int? parentId = 0);
    }
}