using System;
using System.Web.Mvc;
using ApiDoc.Models;
using ApiDoc.Provider;

namespace ApiDoc.Controllers
{
    public class ModulesController : Controller
    {
        private readonly IModulesProvider _modulesProvider;
        private readonly IContextProvider _contextProvider;

        public ModulesController(IModulesProvider modulesProvider, IContextProvider contextProvider)
        {
            _modulesProvider = modulesProvider;
            _contextProvider = contextProvider;
        }


        public ActionResult List(string apiName)
        {
            var api = _contextProvider.GetApiByName(apiName);

            ViewBag.ApiDescription = api;
            ViewBag.ModuleDescriptions = _modulesProvider.GetModules(api.Id);
            return View("ListModules");
        }

        public ActionResult Edit(int id)
        {
            return View("EditModule", _modulesProvider.GetModule(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, ModuleDescription model)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Create()
        {
            return View("CreateModule",new ModuleDescription());
        }
        
        [HttpPost]
        public ActionResult Create(ModuleDescription model)
        {
            throw new NotImplementedException();
        }
    }
}
