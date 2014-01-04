using System.Collections.Generic;
using System.Linq;
using ApiDoc.DataAccess.Proxies;
using ApiDoc.Models;

namespace ApiDoc.Provider
{
    public class NodeProvider : INodeProvider
    {
        private readonly IApiDocDbProxy _proxy;
        private static readonly Branch Root = new Branch { Name = "ROOT", Id = 0 };

        public NodeProvider(IApiDocDbProxy proxy)
        {
            _proxy = proxy;
        }

        public IList<Branch> GetAllBranches(bool showDeleted = false)
        {
            return _proxy.GetBranches(null, showDeleted);
        }

        public Node GetByName(string name, int? parentId = 0, int? revision = null)
        {
            return _proxy.GetBranchByName(name, parentId, revision);
        }

        public Node GetById(int id, int? revision = null)
        {
            return _proxy.GetBranchById(id, revision);
        }

        public int InsertNode(Node newNode)
        {
            if(newNode is Branch)
                return _proxy.InsertBranch(newNode as Branch);

            return _proxy.InsertLeaf(newNode as Leaf);
        }

        public void UpdateNode(Node newNode)
        {
            if (newNode is Branch)
                _proxy.UpdateBranch(newNode as Branch);
            else {
                _proxy.UpdateLeaf(newNode as Leaf);
            }
        }   
        
        public void Delete(Node oldNode)
        {
            if(oldNode is Branch)
                _proxy.DeleteBranch(oldNode.Id, oldNode.Author, oldNode.ChangeNote);
        }

        public IList<Node> GetRevisions(Node node)
        {
            if(node is Branch)
                return _proxy.GetBranchRevisions(node.Name, node.ParentId).Cast<Node>().ToList();

            return _proxy.GetLeafRevisions(node.Name, node.ParentId).Cast<Node>().ToList();
        }

        // TODO: replace with some (cached!) path lookup
        public NodeStructure GetStructure(string path, bool showDeleted = false)
        {
            IList<Node> result = new List<Node> { Root };
            var isNode = string.IsNullOrEmpty(path) || path.EndsWith("/");

            if (path != null)
            {
                var chunks = path.Split('/');
                for (var i = 0; i < chunks.Length; i++)
                {
                    // fetch next deeper chunk
                    var name = chunks[i];
                    var current = result.Last();
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (isNode || chunks.Length < i - 1)
                            result.Add(_proxy.GetBranchByName(name, current.Id));
                        else
                            result.Add(_proxy.GetLeafByName(name, current.Id));
                    }
                }
            }

            GetChildren(result.Last(), showDeleted);
            return new NodeStructure
                {
                    Nodes = result,
                    OriginalPath = path
                };
        }

        private void GetChildren(Node element, bool showDeleted)
        {
            var node = element as Branch;
            if (node != null)
            {
                var children = _proxy.GetBranches(element.Id, showDeleted).Cast<Node>().ToList();
                children.AddRange(_proxy.GetLeafes(node.Id, showDeleted).Cast<Node>().ToList());
                node.Children = children;
            }
        }
    }
}