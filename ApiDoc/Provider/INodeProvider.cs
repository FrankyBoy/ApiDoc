using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface INodeProvider
    {
        IList<Branch> GetAllBranches(bool showDeleted = false);

        Node GetByName(string name, int? parentId = 0, int? revision = null);
        int InsertNode(Node newNode);
        void UpdateNode(Node newNode);
        void Delete(Node oldNode);
        IList<Node> GetRevisions(Node node);
        NodeStructure GetStructure(string path, bool showDeleted = false);
    }
}