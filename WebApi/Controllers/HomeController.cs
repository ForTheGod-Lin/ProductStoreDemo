using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Admin()
        {
            var appUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Admin" });
            ViewBag.Url = new Uri(Request.Url, appUrl).AbsoluteUri.ToString();
            
            return View();
        }
    }
}
