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

        public ActionResult Display(string path, bool showDeleted)
        {
            ViewBag.ShowDeleted = showDeleted;
            var result = GetStructure(path, showDeleted).Last();

            if(result is Node)
                return View("DisplayNode", result as Node);

            return View("DisplayMethod", result as Method);
        }

        #region Create
        public ActionResult Create(string path)
        {
            VersionedItem parent = GetStructure(path).Last();
            return View("Create", parent);
        }

        [HttpPost]
        public ActionResult CreateNode(int parentId, Node model)
        {
            model.Author = "dummy"; // TODO: replace with currently logged in user
            model.ParentId = parentId;
            var newId = _nodeProvider.InsertNode(model); 

            if (newId > 0)
                return RedirectToAction("Display", new {path = GetStructureForNode(parentId).GetWikiPath()});

            ModelState.AddModelError("Name", "Name already exists");
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult CreateMethod(int parentId, Method model)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Edit
        public ActionResult Edit(string path)
        {
            var item = GetStructure(path).Last();
            if (item is Node)
                return RedirectToAction("EditNode", new{id=item.Id});

            return View("EditMethod", new { id = item.Id });
        }

        public ActionResult EditNode(int id)
        {
            return View("EditNode", _nodeProvider.GetById(id));
        }

        [HttpPost]
        public ActionResult EditNode(int id, Node model)
        {
            model.Author = "dummy"; // TODO: replace with currently logged in user
            model.Id = id;
            if(ModelState.IsValid){
                try
                {
                    _nodeProvider.UpdateNode(model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ex);
                }
            }
            
            if(ModelState.IsValid)
                return RedirectToAction("Display", new { path = GetStructureForNode(model.Id).GetWikiPath() });

            return View("CreateNode", model);
        }

        #endregion

        #region Ugly pathing work
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
        #endregion
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
