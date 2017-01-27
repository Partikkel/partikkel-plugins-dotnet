using System.Web.Mvc;
using Partikkel.Validator;

namespace PartikkelDemo.Infrastructure
{
    public class PartikkelValidatorAttribute : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var token = filterContext.RequestContext.HttpContext.Request["partikkel"];
            var requestUrl = filterContext.RequestContext.HttpContext.Request.Url.ToString();
            var validator = new PartikkelValidator();
            try
            {
                var claims = validator.Validate(token);
                var url = claims["url"] as string;
                var path = requestUrl.Substring(0, requestUrl.IndexOf("?"));
                if (url.Equals(path))
                {
                    if (filterContext.HttpContext.Session != null)
                    {
                        filterContext.HttpContext.Session.Add(url, claims);
                        filterContext.Controller.ViewBag.HasBought = true;
                    }
                        
                }
                base.OnActionExecuting(filterContext);
            }
            catch
            {
            }
        }
    }
}