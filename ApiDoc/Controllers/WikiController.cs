using System;
using System.Linq;
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
            var result = GetElement(path, showDeleted ?? false);
            if(result is Node)
                return View("DisplayNode", result as Node);

            return View("DisplayMethod", result as Method);
        }
        
        public ActionResult Create(string path)
        {
            VersionedItem parent = GetElement(path);
            return View("Create", parent);
        }

        [HttpPost]
        public ActionResult CreateNode(string path, Node model)
        {
            VersionedItem parent = GetElement(path);
            model.Author = "dummy"; // TODO: replace with currently logged in user
            var newId = _nodeProvider.InsertNode(model, parent.Id); 

            if (newId > 0)
                return RedirectToAction("Display", new {path});

            ModelState.AddModelError("Name", "Name already exists");
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult CreateMethod(string path, Method model)
        {
            throw new NotImplementedException();
        }

        /*
        public ActionResult Edit(string path)
        {
            return View("EditApi", _nodeProvider.GetById(apiId));
        }

        [HttpPost]
        public ActionResult Edit(int apiId, Node model)
        {
            try
            {
                _nodeProvider.UpdateNode(model, "dummy"); // TODO: replace with currently logged in user
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ModelError", ex);
                return View("EditApi", model);
            }
            return RedirectToAction("List");
        }
        
        public ActionResult Create()
        {
            return View("CreateApi", new Node());
        }

        [HttpPost]
        public ActionResult Create(Node model)
        {
            var newId = _nodeProvider.InsertNode(model, "dummy"); // TODO: replace with currently logged in user
            if (newId > 0)
                return RedirectToAction("List");
            
            ModelState.AddModelError("Name", "Name already exists");
            return View("CreateApi", model);
        }

        public ActionResult Delete(int apiId)
        {
            _nodeProvider.DeleteNode(apiId);
            return RedirectToAction("List");
        }

        public ActionResult Revisions(int apiId)
        {
            var historyItems = _nodeProvider.GetRevisions(apiId);
            ViewBag.History = historyItems;
            HistoryViewModel model;
            switch (historyItems.Count)
            {
                case 0:
                    model = new HistoryViewModel(0, 0);
                    break;
                case 1:
                    model = new HistoryViewModel(historyItems[0].RevisionNumber, historyItems[0].RevisionNumber);
                    break;
                default:
                    model = new HistoryViewModel(historyItems[1].RevisionNumber, historyItems[0].RevisionNumber);
                    break;
            }

            return View("ApiRevisions", model);
        }

        [HttpPost]
        public ActionResult Revisions(int apiId, HistoryViewModel revisions)
        {
            ViewBag.Comparison = _nodeProvider.CompareRevisions(apiId, revisions.Rev1, revisions.Rev2);
            ViewBag.History = _nodeProvider.GetRevisions(apiId);
            // TODO: use better diff, the current one is not perfect
            return View("ApiRevisions", revisions);
        }*/



        private VersionedItem GetElement(string path, bool showDeleted = false)
        {
            VersionedItem result = new Node { Name = "ROOT", Id = null };
            var isNode = string.IsNullOrEmpty(path) || path.EndsWith("/");

            if (path != null)
            {
                var chunks = path.Split('/');
                for (var i = 0; i < chunks.Length; i++)
                {
                    // fetch next deeper chunk
                    var name = chunks[i];
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (isNode || chunks.Length < i - 1)
                            result = _nodeProvider.GetByName(name, result.Id);
                        else
                            result = _methodProvider.GetByName(name, result.Id);
                    }
                }
            }

            if (result is Node)
            {
                var children = _nodeProvider.GetNodes(result.Id, showDeleted).Cast<VersionedItem>().ToList();
                children.AddRange(_methodProvider.GetMethods(result.Id));
                result.Children = children;
            }

            return result;
        }
    }
}
