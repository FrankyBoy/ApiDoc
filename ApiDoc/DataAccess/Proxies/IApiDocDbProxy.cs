using System.Collections.Generic;
using ApiDoc.Models;

namespace ApiDoc.DataAccess.Proxies
{
    public interface IApiDocDbProxy
    {
        HttpVerbs GetHttpVerbs();

        // note: only for GetBranches parentId is allowed to be null as null means "all branches" instead of 
        // just the children of the root (parentId=0)
        IList<Branch> GetBranches(int? parentId, bool showDeleted = false);
        Branch GetBranchByName(string name, int parentId, int? revision = null);
        int InsertBranch(Branch newBranch);
        int UpdateBranch(Branch newBranch);
        void DeleteBranch(int id, string author, string reason);
        IList<Branch> GetBranchRevisions(string name, int parentId);


        IList<Leaf> GetLeafes(int parentId, bool showDeleted = false);
        Leaf GetLeafByName(int parentId, string name, int? httpVerb, int? revision = null);
        int InsertLeaf(Leaf newLeaf);
        void UpdateLeaf(Leaf newLeaf);
        void DeleteLeaf(int id, string author, string reason);
        IList<Leaf> GetLeafRevisions(int parentId, string name, int? httpVerb);
    }
}