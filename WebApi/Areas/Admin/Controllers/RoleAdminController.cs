using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebApi.Models;
using WebApi.Areas.Admin.Models;
using System.Threading.Tasks;
namespace WebApi.Areas.Admin.Controllers
{
    public class RoleAdminController : Controller
    {
        [ChildActionOnly]
        public ActionResult Index()
        {
            return View();
        }
        public ApplicationRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
        }
        // GET: Admin/roleAdmin
        public ActionResult GetAll(SearchModel search, int index = 1, int pageSize = 10)
        {
            var model = RoleManager.Roles;

            return Json(new { total = model.Count(), rows = model.OrderBy(u => u.Id).Skip(index - 1).Take(pageSize) }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Get(string roleId)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                return Json(role, JsonRequestBehavior.AllowGet);
            }
            return Content("ERROR");
        }
        public async Task<ActionResult> Create(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await RoleManager.CreateAsync(role);
                if (result.Succeeded) return Content("OK");
                else ModelState.AddModelError("", result.Errors.First());
            }
            return Content(String.Join(",", ModelState.Where(m => m.Value.Errors.Count() != 0).Select(m => String.Join(",", m.Value.Errors.Select(e => e.ErrorMessage)))));
        }
        public async Task<ActionResult> Update(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var roleInfo = await RoleManager.FindByIdAsync(role.Id);
                if (roleInfo != null)
                {
                    roleInfo.Name = role.Name;
                }
                else return Content("未找到");
                var result = await RoleManager.UpdateAsync(roleInfo);
                if (result.Succeeded) return Content("OK");
                else ModelState.AddModelError("", result.Errors.First());
            }
            return Content(String.Join(",", ModelState.Where(m => m.Value.Errors.Count() != 0).Select(m => String.Join(",", m.Value.Errors.Select(e => e.ErrorMessage)))));
        }
        public async Task<ActionResult> Delete(string roleId)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await RoleManager.DeleteAsync(role);
                return Content("OK");
            }
            return Content("删除错误");
        }
    }
}