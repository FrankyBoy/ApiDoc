using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ApiDoc.Models;
using ApiDoc.Provider;

namespace ApiDoc.Controllers
{
    public class WikiController : Controller
    {
        private readonly INodeProvider _nodeProvider;
        private readonly IMethodProvider _methodProvider;

        public WikiController(INodeProvider nodeProvider, IMethodProvider methodProvider)
        {
            _nodeProvider = nodeProvider;
            _methodProvider = methodProvider;
        }

        public ActionResult Display(string path, bool? showDeleted)
        {
            ViewBag.ShowDeleted = showDeleted ?? false;
            var result = GetStructure(path, showDeleted ?? false).Last();

            if(result is Node)
                return View("DisplayNode", result as Node);

            return View("DisplayMethod", result as Method);
        }
        
        public ActionResult Create(string path)
        {
            VersionedItem parent = GetStructure(path).Last();
            return View("Create", parent);
        }

        [HttpPost]
        public ActionResult CreateNode(int? parentId, Node model)
        {
            var parent = GetStructureForNode(parentId);
            model.Author = "dummy"; // TODO: replace with currently logged in user
            var newId = _nodeProvider.InsertNode(model, parent.Last().Id); 

            if (newId > 0)
                return RedirectToAction("Display", new {path = parent.GetWikiPath()});

            ModelState.AddModelError("Name", "Name already exists");
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult CreateMethod(int parentId, Method model)
        {
            throw new NotImplementedException();
        }
        
        private static readonly Node Root = new Node {Name = "ROOT", Id = 0};

        // TODO: replace with some (cached!) path lookup
        private IList<VersionedItem> GetStructure(string path, bool showDeleted = false)
        {
            IList<VersionedItem> result = new List<VersionedItem>{Root};
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
                            result.Add(_nodeProvider.GetByName(name, current.Id));
                        else
                            result.Add(_methodProvider.GetByName(name, current.Id));
                    }
                }
            }
            GetChildren(result.Last(), showDeleted);
            return result;
        }

        // TODO: replace with some (cached!) path lookup
        private IList<VersionedItem> GetStructureForNode(int? nodeId, bool showDeleted = false)
        {
            IList<VersionedItem> result = new List<VersionedItem> { Root };
            while (nodeId != null && nodeId != 0)
            {
                var node = _nodeProvider.GetById((int)nodeId);
                result.Add(node);
                nodeId = node.ParentId;
            }
            result = result.Reverse().ToList();

            GetChildren(result.Last(), showDeleted);
            return result;
        }

        private void GetChildren(VersionedItem element, bool showDeleted)
        {
            var node = element as Node;
            if (node != null)
            {
                var children = _nodeProvider.GetNodes(node.Id, showDeleted).Cast<VersionedItem>().ToList();
                children.AddRange(_methodProvider.GetMethods(node.Id));
                node.Children = children;
            }
        }
    }

    internal static class ItemListExtensions
    {
        public static string GetWikiPath(this IList<VersionedItem> items)
        {
            var output = new StringBuilder();
            foreach (var item in items)
            {
                if(item.Id != 0) {
                    output.Append(item.GetWikiUrlString());
                }
            }

            return output.ToString();
        }
    }
}
