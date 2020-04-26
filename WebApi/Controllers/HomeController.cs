using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebApi.Models;
using System.Threading.Tasks;
namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); } }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [Authorize]
        public async Task<ActionResult> CartIndex(string returnUrl)
        {
            var user =await UserManager.FindByNameAsync(User.Identity.Name);
            ViewBag.returnUrl = returnUrl;
            return View(user);
        }
    }
}
