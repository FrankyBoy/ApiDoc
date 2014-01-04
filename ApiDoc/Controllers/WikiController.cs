using System;
using System.Web.Mvc;
using ApiDoc.Models;
using ApiDoc.Provider;
using ApiDoc.Utility;

namespace ApiDoc.Controllers
{
    public class WikiController : Controller
    {
        private readonly INodeProvider _nodeProvider;

        public WikiController(INodeProvider nodeProvider)
        {
            _nodeProvider = nodeProvider;
        }

        [ImportModelState]
        public ActionResult Display(string path, bool? showDeleted, int? revision)
        {
            PrimeViewBag(path, showDeleted ?? false, revision);
            return View();
        }

        #region Create
        [ImportModelState]
        public ActionResult Create(string path)
        {
            Node parent = PrimeViewBag(path).Last();
            return View(parent);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult CreateBranch(string path, Branch model)
        {
            var structure = PrimeViewBag(path);

            model.Author = "dummy"; // TODO: replace with currently logged in user
            model.ParentId = structure.Last().Id;
            var newId = _nodeProvider.InsertNode(model); 

            if (newId > 0)
                return RedirectToAction("Display", new {path = structure.GetWikiPath()});

            ModelState.AddModelError("Name", "Name already exists");
            return RedirectToAction("Create", new { path = structure.GetWikiPath() });
        }

        [HttpPost]
        public ActionResult CreateMethod(string path, Leaf model)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Edit
        [ImportModelState]
        public ActionResult Edit(string path, int? revision)
        {
            PrimeViewBag(path, revision: revision);
            ViewBag.AllNodes = _nodeProvider.GetAllBranches();
            return View("Edit");
        }
        
        [HttpPost]
        [ExportModelState]
        public ActionResult EditBranch(string path, Branch model)
        {
            var structure = PrimeViewBag(path);

            model.Author = "dummy"; // TODO: replace with currently logged in user
            model.Id = structure.Last().Id;

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
                return RedirectToAction("Display", new { path = structure.GetWikiPath() });

            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult EditMethod(int id, Leaf model)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region History

        public ActionResult History(string path)
        {
            var item = PrimeViewBag(path).Last();
            var history = _nodeProvider.GetRevisions(item);
            ViewBag.History = history;

            HistoryViewModel model;
            switch (history.Count)
            {
                case 0:
                    model = new HistoryViewModel(0, 0);
                    break;
                case 1:
                    model = new HistoryViewModel(history[0].RevisionNumber, history[0].RevisionNumber);
                    break;
                default:
                    model = new HistoryViewModel(history[1].RevisionNumber, history[0].RevisionNumber);
                    break;
            }

            return View(model);
        }
        
        [HttpPost]
        public ActionResult History(string path, HistoryViewModel revisions)
        {
            var item = PrimeViewBag(path).Last();
            var history = _nodeProvider.GetRevisions(item);
            ViewBag.History = history;

            ViewBag.Comparison = history.Compare(revisions.Rev1, revisions.Rev2);
            return View(revisions);
        }
        #endregion

        #region Delete
        [HttpPost]
        [ExportModelState]
        public ActionResult Delete(string path, string message)
        {
            var tree = PrimeViewBag(path);
            var item = tree.Last();
            tree.Nodes.Remove(item);

            if (string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("Error", "No message provided");
            }
            else
            {
                item.Author = "dummy"; // todo: replace with logged in user
                item.ChangeNote = message;
                _nodeProvider.Delete(item);
            }

            return RedirectToAction("Display", new { path = tree.GetWikiPath() });
        }
        
        #endregion


        private NodeStructure PrimeViewBag(string path, bool showDeleted = false, int? revision = null)
        {
            var structure = _nodeProvider.GetStructure(path, showDeleted, revision);
            if (structure.AnyNodeDeleted)
                ModelState.AddModelError("Warning", "Warning: An element in the path has been marked as deleted");

            // TODO: figure out how we could redirect in this case
            if (structure.HasPathError)
                ModelState.AddModelError("Error", "Error: The path you tried to access is ambiguous or does not exist. We dropped you off at the deepest working node.");
            else if (structure.AnyNodeRenamed)
                ModelState.AddModelError("Warning", "Warning: An element in the path has been renamed");
                
            ViewBag.Structure = structure;
            ViewBag.ShowDeleted = showDeleted;
            return structure;
        }

    }
}
