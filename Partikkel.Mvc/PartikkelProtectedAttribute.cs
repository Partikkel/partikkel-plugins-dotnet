using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Partikkel.Mvc
{
    public class PartikkelProtectedAttribute : ActionFilterAttribute
    {
        public string CertPath { get; set; }
        public string PaywallUrl { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = filterContext.RequestContext.HttpContext.Request.Url;
            var path = String.Format("{0}{1}{2}{3}", url.Scheme, Uri.SchemeDelimiter, url.Authority, url.AbsolutePath);
            var token = filterContext.RequestContext.HttpContext.Request.QueryString["partikkel"];
            var urlList = HttpContext.Current.Session["Particket"] as List<string> ?? new List<string>();

            if (!urlList.Contains(path))
            {
                ValidateToken(token, path, urlList);
            }
            base.OnActionExecuting(filterContext);
        }

        private void ValidateToken(string token, string path, List<string> urlList)
        {
            var paywallUrlWithArticle = PaywallUrl + "?article=" + path;
            if (string.IsNullOrEmpty(token))
            {
                HttpContext.Current.Response.Redirect(paywallUrlWithArticle);
            }
            else
            {
                //Validate token
                var validator = new PartikkelValidator();
                var result = validator.Validate(token, CertPath);
                if (result == null)
                {
                    HttpContext.Current.Response.Redirect(paywallUrlWithArticle);
                    return;
                }
                
                //Check that the token url matches the current url    
                var purchasedUrl = result["url"] as string;
                //string path = String.Format("{0}{1}{2}{3}", url.Scheme, Uri.SchemeDelimiter, url.Authority, url.AbsolutePath);
                if (purchasedUrl == null || !purchasedUrl.Contains(path))
                {
                    HttpContext.Current.Response.Redirect(paywallUrlWithArticle);
                }
                else
                {
                    urlList.Add(path);
                    HttpContext.Current.Session["Particket"] = urlList;
                }
            }
        }
    }
}
