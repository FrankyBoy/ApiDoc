﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                try
                {
                    for (var i = 0; i < chunks.Length; i++)
                    {
                        // fetch next deeper chunk
                        var name = chunks[i];
                        var current = nodes.Last();
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (chunks.Length < i - 1)
                                nodes.Add(_proxy.GetBranchByName(name, current.Id));
                            else if (isNode)
                                nodes.Add(_proxy.GetBranchByName(name, current.Id, revision));
                            else
                                nodes.Add(GetLeafByWikiName(name, current.Id, revision));
                        }
                    }
                }
                catch (Exception)
                {
                    result.HasPathError = true;
                }
            }

            GetChildren(nodes.Last(), showDeleted);
            return result;
        }

        private Leaf GetLeafByWikiName(string name, int parentId, int? revision)
        {
            name = name.Trim();
            int? methodId = null;
            if (Regex.IsMatch(name, @".+_\([A-Za-z]+\)"))
            {
                var methodName = name.Split('_').Last();
                methodName = Regex.Replace(methodName, @"\(\)", "");
                methodId = _proxy.GetHttpVerbs().IdForName(methodName);
            }
            return _proxy.GetLeafByName(parentId, name, methodId, revision);
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