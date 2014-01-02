using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface INodeProvider
    {
        IList<Node> GetNodes(int? parentId = null, bool showDeleted = false);
        Node GetByName(string name, int? parentId = null, int? revision = null);
        Node GetById(int id, int? revision = null);
        int InsertNode(Node newNode);
        void UpdateNode(Node newNode);
        void DeleteNode(int id, string author, string reason);
        IList<Node> GetRevisions(string name, int? parentId = null);
        Node CompareRevisions(int id, int rev1, int rev2);
    }
}