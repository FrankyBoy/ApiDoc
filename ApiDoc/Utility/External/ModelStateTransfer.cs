using System.Web.Mvc;

namespace ApiDoc.Utility.External
{
    public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTempDataTransfer).FullName;
    }

    public class ExportModelState : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //export only when model state is invalid
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                //export if it is redirecting to other actions
                if (filterContext.Result is RedirectResult || filterContext.Result is RedirectToRouteResult)
                {
                    filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }

    public class ImportModelState : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;
            if (modelState != null)
            {
                //import if we are viewing. Otherwise, remove it
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    filterContext.Controller.TempData.Remove(Key);
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}