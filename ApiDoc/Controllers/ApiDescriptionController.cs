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
            ViewBag.ApiDescriptions = _apiDescriptionProvider.GetApiDescriptions();
            return View("ListApis");
        }

        public ActionResult Edit(int id)
        {
            return View("EditApi", _apiDescriptionProvider.GetApiDescription(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, ApiDescription model)
        {
            throw new System.NotImplementedException();
        }
        
        public ActionResult Create()
        {
            return View("CreateApi", new ApiDescription());
        }

        [HttpPost]
        public ActionResult Create(ApiDescription model)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
