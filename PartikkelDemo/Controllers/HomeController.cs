using System.Web.Mvc;
using PartikkelDemo.Infrastructure;

namespace PartikkelDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [PartikkelValidator]
        public ActionResult Artikkel1()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [PartikkelValidator]
        public ActionResult Artikkel2()
        { 
            ViewBag.Message = "Artikkel 2";
            return View();
        }
    }
}