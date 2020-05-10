using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
namespace WebApi.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
      
        public ApplicationRoleManager RoleManager
        {

            get { return Request.GetOwinContext().Get<ApplicationRoleManager>(); }
        }
        public ApplicationUserManager UserManager
        {
            get { return Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu()
        {
            var user = HttpContext.GetOwinContext().Get<ApplicationUserManager>().FindByName(User.Identity.Name);
            var list = user.Roles.Select(r => RoleManager.FindById(r.RoleId));
            var menus = new List<Menu>();
          foreach(var r in list)
            {
                menus.AddRange(r.Menus);
            }
            return View(menus);
        }
        public ActionResult UserIndex()
        {
            ViewBag.RoleNames = HttpContext.GetOwinContext().Get<ApplicationRoleManager>().Roles.Select(r => r.Name);
            return View();
        }
        public ActionResult RoleIndex()
        {
            return View();
        }
        public ActionResult ProductIndex()
        {
            return View();
        }
    }
}