using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiDoc.Provider;

namespace ApiDoc.Controllers
{
    public class MethodsController : Controller
    {
        private readonly IContextProvider _contextProvider;
        private readonly IMethodProvider _methodProvider;

        public MethodsController(IContextProvider contextProvider, IMethodProvider methodProvider)
        {
            _contextProvider = contextProvider;
            _methodProvider = methodProvider;
        }

        public ActionResult List(string apiName, string moduleName)
        {
            var module = _contextProvider.GetModuleByName(apiName, moduleName);
            ViewBag.Api = module.Item1;
            ViewBag.Module = module.Item2;
            ViewBag.Methods = _methodProvider.GetMethods(module.Item2.Id);
            return View("ListMethods");
        }

        public ActionResult Create()
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
