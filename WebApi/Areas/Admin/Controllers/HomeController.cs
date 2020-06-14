using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
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
   public ApplicationDbContext DbContext
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
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
                    var selectedRoles = Context.RoleManager.Roles.Select(r => new SelectListItem
                    {
                        Selected = userRoles.Contains(r.Name),
                        Text=r.Name
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
        public ActionResult RoleDialog(string id)
        {
            if (id != null)
            {
                var role = Context.RoleManager.FindById(id);
                return View(role);
            }
            else
            {
                return View();
            }
        }
        public ActionResult ProductIndex()
        {
            return View();
        }
        public ActionResult ProductDialog(int? id)
        {
            if (id != null)
            {
                var product = Context.ProductRepositry.FindById(id);
                return View(product);
            }
            else
            {
                return View();
            }
        }
        public ActionResult MenuGroups()
        {
            var menuGroups = new List<MenuGroup>();
            var role1 = Context.UserManager.FindByName(User.Identity.Name).Roles.Select(r => Context.RoleManager.FindById(r.RoleId));
            var role = role1.FirstOrDefault();
            foreach (var menus in role.MenuGroups)
            {
                var mg = Context.MenuGroupRepositry.Find(m => m.Id == menus.MenuGroupId);
                foreach (var me in mg.Menus.Except(role.Menus).ToList())
                {
                    mg.Menus.Remove(me);
                }

                foreach (var m in mg.Menus)
                {
                    foreach (var me in m.MenuItems.Except(role.MenuItems).ToList())
                    {
                        m.MenuItems.Remove(me);
                    }
                }
                menuGroups.Add(mg);
            }
            JsonSerializerSettings jsSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var converted = JsonConvert.SerializeObject(menuGroups, null, jsSettings);
            return  Content(converted, "application/json");
        }

        public ActionResult MenuDistribution()
        {
            DbContext.Configuration.LazyLoadingEnabled = false;
            var model = Context.RoleManager.Roles;
            return View(model);
        }
        public ActionResult GetMenuTree(string roleName)
        {
            var role = Context.RoleManager.FindByName(roleName);
            var mgList = Context.MenuGroupRepositry.FindList();
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach(var mg in mgList)
            {
                var mgdata = new Dictionary<string, object> { {"id",mg.Id },{"text",mg.Name },{ "attributes", new {type=1 } },{ "children", new List<Dictionary<string, object>>()} };
                foreach (var m in mg.Menus)
                {
                    var mdata = new Dictionary<string, object> { { "id", m.Id }, { "text", m.Title },  { "attributes", new { type = 2 } }, { "children", new List<Dictionary<string, object>>() } };
                    foreach(var i in m.MenuItems)
                    {
                        var idata = new Dictionary<string, object> { { "id", i.Id }, { "text", i.Text }, { "attributes", new { type = 3} }, { "checked", role.MenuItems.Contains(i) } };
                        ((List<Dictionary<string, object>>)mdata["children"]).Add(idata);
                    }
                    ((List<Dictionary<string, object>>)mgdata["children"]).Add(mdata);
                }
                items.Add(mgdata);
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveMenu(string roleName,string nodes)
        {
            var role = Context.RoleManager.FindByName(roleName);
            var ns = JArray.Parse(nodes);
            var mgL = new List<int>();
            var mL = new List<int>();
            var iL = new List<int>();
            foreach (var a in ns)
            {
                var id = a["id"].ToObject<int>();
                switch (a["attributes"]["type"].ToObject<int>())
                {
                    case 1: {
                            mgL.Add(id);
                            break; }
                    case 2: {
                            mL.Add(id);
                            break; }
                    case 3: {
                            iL.Add(id);
                            break; }
                }
            }
            foreach (var i in mgL.Except(role.MenuGroups.Select(m => m.MenuGroupId).ToList()))
            {
                     role.MenuGroups.Add(new RoleMenuGroup { ApplicationRoleId = role.Id, MenuGroupId = i
                     });                 
            }
            foreach (var i in role.MenuGroups.Select(m => m.MenuGroupId).Except(mgL).ToList())
            {
                var a=role.MenuGroups.Remove(role.MenuGroups.Where(mg => mg.MenuGroupId == i).First());
            }
            foreach (var i in mL.Except(role.Menus.Select(m => m.Id)).ToList())
            {
                var b=DbContext.Set<Menu>().Where(mg => mg.Id == i).First();
                role.Menus.Add(b);
            }
            foreach (var i in role.Menus.Select(m => m.Id).Except(mL).ToList())
            {
                role.Menus.Remove(role.Menus.Where(mg => mg.Id == i).First()); 
            }
            foreach (var i in iL.Except(role.MenuItems.Select(m => m.Id)).ToList())
            {
                role.MenuItems.Add(DbContext.Set<MenuItem>().Where(mg => mg.Id == i).First());
            }
            foreach (var i in role.MenuItems.Select(m => m.Id).Except(iL).ToList())
            {
                role.MenuItems.Remove(role.MenuItems.Where(mg => mg.Id == i).First()); 
            }
            var result=Context.RoleManager.Update(role);
            if (result.Succeeded)
                return Content("分配权限成功");
            else return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
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