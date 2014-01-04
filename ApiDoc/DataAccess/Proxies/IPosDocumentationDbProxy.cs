using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
{
    public interface IPosDocumentationDbProxy
    {
        IDictionary<int, string> GetHttpMethods();

        IList<Branch> GetBranches(int? parentId = 0, bool showDeleted = false);
        Branch GetBranchById(int id, int? revision = null);
        Branch GetBranchByName(string name, int? parentId = 0, int? revision = null);
        int GetBranchId(string name, int? parentId = 0);
        int InsertBranch(Branch newBranch);
        int UpdateBranch(Branch newBranch);
        void DeleteBranch(int id, string author, string reason);
        IList<Branch> GetBranchRevisions(string name, int? parentId = 0);


        IList<Leaf> GetLeafes(int? parentId = 0, bool showDeleted = false);
        IList<Leaf> GetLeafRevisions(string name, int? parentId = 0);
        int InsertLeaf(Leaf newLeaf);
        void UpdateLeaf(Leaf newLeaf);
        Node GetLeafByName(string name, int? parentId = 0, int? revision = null);
    }
}