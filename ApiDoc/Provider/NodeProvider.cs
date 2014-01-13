using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApiDoc.DataAccess.Proxies;
using ApiDoc.Models;
using ApiDoc.Models.Exceptions;
using ApiDoc.Utility;
using Newtonsoft.Json;

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

        public int InsertNode(Node newNode)
        {
            if (newNode is Branch)
                return _proxy.InsertBranch(newNode as Branch);

            return _proxy.InsertLeaf(PrettyPrint(newNode as Leaf));
        }

        public void UpdateNode(Node newNode)
        {
            if (newNode is Branch)
                _proxy.UpdateBranch(newNode as Branch);
            else {
                _proxy.UpdateLeaf(PrettyPrint(newNode as Leaf));
            }
        }
        
        public void Delete(Node oldNode)
        {
            if (oldNode is Branch)
                _proxy.DeleteBranch(oldNode.Id, oldNode.Author, oldNode.ChangeNote);
            else
                _proxy.DeleteLeaf(oldNode.Id, oldNode.Author, oldNode.ChangeNote);
        }

        public IList<Node> GetRevisions(Node node)
        {
            if(node is Branch)
                return _proxy.GetBranchRevisions(node.Name, node.ParentId).Cast<Node>().ToList();

            var leaf = (Leaf)node;
            var httpVerbs = _proxy.GetHttpVerbs();
            return _proxy.GetLeafRevisions(leaf.ParentId, leaf.Name, httpVerbs.IdForName(leaf.HttpVerb)).Cast<Node>().ToList();
        }

        // TODO: replace with some (cached!) path lookup
        public NodeStructure GetStructure(string path, bool showDeleted = false, int? revision = null)
        {
            IList<Node> nodes = new List<Node> { Root };
            var result = new NodeStructure
                {
                    Nodes = nodes,
                    OriginalPath = path
                };

            var isNode = string.IsNullOrEmpty(path) || path.EndsWith("/");

            if (path != null)
            {
                var chunks = path.Split('/');
                chunks = chunks.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                try
                {
                    for (var i = 0; i < chunks.Length; i++)
                    {
                        // fetch next deeper chunk
                        var name = chunks[i];
                        var current = nodes.Last();
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (i + 1 < chunks.Length)
                                nodes.Add(_proxy.GetBranchByName(name.FromWikiUrlString(), current.Id));
                            else if (isNode)
                                nodes.Add(_proxy.GetBranchByName(name.FromWikiUrlString(), current.Id, revision));
                            else
                                nodes.Add(GetLeafByWikiName(name, current.Id, revision));
                        }
                    }
                }
                catch (Exception)
                {
                    result.SetPathError(new InvalidPathError("Path does not exist"));
                }
            }

            GetChildren(nodes.Last(), showDeleted);
            return result;
        }

        public HttpVerbs GetHttpVerbs()
        {
            return _proxy.GetHttpVerbs();
        }

        private Leaf GetLeafByWikiName(string name, int parentId, int? revision)
        {
            name = name.Trim();
            int? httpVerb = null;
            if (Regex.IsMatch(name, @".?_\([A-Za-z]+\)"))
            {
                var methodName = name.Split('_').Last();
                name = name.Remove(name.Length - methodName.Length - 1).FromWikiUrlString();
                methodName = methodName.Replace("(","").Replace(")","");
                httpVerb = _proxy.GetHttpVerbs().IdForName(methodName);
            }
            return _proxy.GetLeafByName(parentId, name, httpVerb, revision);
        }

        private void GetChildren(Node element, bool showDeleted)
        {
            var node = element as Branch;
            if (node != null)
            {
                var children = _proxy.GetBranches(element.Id, showDeleted)
                    .Cast<Node>().OrderBy(x => x.Name).ToList();
                children.AddRange(_proxy.GetLeafes(node.Id, showDeleted)
                    .OrderBy(x => x.Name).ThenBy(x => x.HttpVerb).Cast<Node>().ToList());
                
                foreach (var child in children)
                    child.Description = child.Description.Split('\n')[0];

                node.Children = children;
            }
        }

        private static Leaf PrettyPrint(Leaf leaf)
        {
            if (!string.IsNullOrEmpty(leaf.SampleRequest))
                leaf.SampleRequest = PrettyPrintJson(leaf.SampleRequest);

            if (!string.IsNullOrEmpty(leaf.SampleResponse))
                leaf.SampleResponse = PrettyPrintJson(leaf.SampleResponse);

            return leaf;
        }

        private static string PrettyPrintJson(string input)
        {
            var dynamicObject = JsonConvert.DeserializeObject(input);
            return JsonConvert.SerializeObject(dynamicObject, Formatting.Indented);
        }
    }
}