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
      public ActionResult Text()
        {
            return View();
        }
    
        private CommonContext _context;
        public CommonContext Context
        {
            get
            {
                if (_context == null) _context = new CommonContext(Request.GetOwinContext());
                return _context;
            }
        }
   
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuGroup()
        {
            var menuGroups = new List<MenuGroup>();
            foreach(var menus in Context.UserManager.FindByName(User.Identity.Name).Roles.Select(r=> Context.RoleManager.FindById(r.RoleId).RoleMenus))
            {
                foreach(var m in menus)
                {
                    var mgr = Context.MenuGroupRepositry.Find(mg => mg.Id == m.MenuGroupId);
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
        public ActionResult UserDialog(string id)
        {
            if (id != null)
            {
                var user = Context.UserManager.FindByIdAsync(id).Result;
                    var userRoles = Context.UserManager.GetRoles(id);
                    var selectedRoles = Context.RoleManager.Roles.Select(r => new
                    {
                        Selected = userRoles.Contains(r.Name),
                        r.Name
                    });
                    ViewBag.selectedRoles = selectedRoles;
                    return View(user);
            }
            else
            {
                var selectedRoles = Context.RoleManager.Roles.Select(r => new SelectListItem
                {
                    Selected = false,
                    Text=r.Name
                });
                ViewBag.selectedRoles = selectedRoles;
                return View();
            }
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