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

        public ActionResult List()
        {
            ViewBag.Apis = _apiDescriptionProvider.GetApis();
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

        public ActionResult Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult History(int apiId)
        {
            throw new System.NotImplementedException();
        }
        
        public ActionResult History(int apiId, int rev1, int rev2)
        {
            throw new System.NotImplementedException();
        }
    }
}
