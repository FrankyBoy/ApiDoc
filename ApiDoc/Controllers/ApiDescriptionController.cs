using System;
using System.Web.Mvc;
using ApiDoc.Models;
using ApiDoc.Provider;

namespace ApiDoc.Controllers
{
    public class ApiDescriptionController : Controller
    {
        private readonly IApiDescriptionProvider _apiDescriptionProvider;

        public ApiDescriptionController(IApiDescriptionProvider apiDescriptionProvider)
        {
            _apiDescriptionProvider = apiDescriptionProvider;
        }

        public ActionResult List(bool showDeleted = false)
        {
            ViewBag.Apis = _apiDescriptionProvider.GetApis(showDeleted);
            ViewBag.ShowDeleted = showDeleted;
            return View("ListApis");
        }
        
        public ActionResult Edit(int apiId)
        {
            return View("EditApi", _apiDescriptionProvider.GetById(apiId));
        }

        [HttpPost]
        public ActionResult Edit(int apiId, ApiDescription model)
        {
            try
            {
                _apiDescriptionProvider.UpdateApi(model, "dummy"); // TODO: replace with currently logged in user
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
            return View("CreateApi", new ApiDescription());
        }

        [HttpPost]
        public ActionResult Create(ApiDescription model)
        {
            var newId = _apiDescriptionProvider.InsertApi(model, "dummy"); // TODO: replace with currently logged in user
            if (newId > 0)
                return RedirectToAction("List");
            
            ModelState.AddModelError("Name", "Name already exists");
            return View("CreateApi", model);
        }

        public ActionResult Delete(int apiId)
        {
            _apiDescriptionProvider.DeleteApi(apiId);
            return RedirectToAction("List");
        }

        public ActionResult Revisions(int apiId)
        {
            var historyItems = _apiDescriptionProvider.GetRevisions(apiId);
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
            ViewBag.Comparison = _apiDescriptionProvider.CompareRevisions(apiId, revisions.Rev1, revisions.Rev2);
            ViewBag.History = _apiDescriptionProvider.GetRevisions(apiId);
            // TODO: use better diff, the current one is not perfect
            return View("ApiRevisions", revisions);
        }
    }
}
