using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using WebApi.Repositries;
namespace WebApi.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
      
      public HomeController()
        {
            Context = new CommonContext(HttpContext.GetOwinContext());
        }
        public CommonContext Context { get; set; }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuGroup()
        {
            var menuGroups = new List<MenuGroup>();
            foreach(var menus in Context.UserManager.FindByName(User.Identity.Name).Roles.Select(r=> Context.RoleManager.FindById(r.RoleId).Menus))
            {
                foreach(var m in menus)
                {
                    var mgr = Context.MenuGroupRepositry.Find(mg => mg.Id == m.Id);
                    mgr.Menus = null;
                    menuGroups.Add(mgr);
                }
            }
            return View(menuGroups);
        }
        public ActionResult Menu()
        {
            var user = HttpContext.GetOwinContext().Get<ApplicationUserManager>().FindByName(User.Identity.Name);
            var list = user.Roles.Select(r => Context.RoleManager.FindById(r.RoleId));
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}