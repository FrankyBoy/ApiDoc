using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public interface INodeProvider
    {
        IList<Branch> GetAllBranches(bool showDeleted = false);

        int InsertNode(Node newNode);
        void UpdateNode(Node newNode);
        void Delete(Node oldNode);
        IList<Node> GetRevisions(Node node);
        NodeStructure GetStructure(string path, bool showDeleted = false, int? revision = null);
        HttpVerbs GetHttpVerbs();
    }
}