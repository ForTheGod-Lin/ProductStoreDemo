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
        public ActionResult Menu(int id)
        {
            var menus = Context.MenuRepositry.FindList(m => m.Id == id);
            return View(menus);
        }
        public ActionResult UserIndex()
        {
            ViewBag.RoleNames = HttpContext.GetOwinContext().Get<ApplicationRoleManager>().Roles.Select(r => r.Name);
            return View();
        }
        public ActionResult UserDialog()
        {
            return View();
        }
        public ActionResult RoleIndex()
        {
            return View();
        }
        public ActionResult RoleDialog()
        {
            return View();
        }
        public ActionResult ProductIndex()
        {
            return View();
        }
        public ActionResult ProductDialog()
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