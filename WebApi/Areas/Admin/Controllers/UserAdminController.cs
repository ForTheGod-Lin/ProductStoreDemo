using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
namespace WebApi.Areas.Admin.Controllers
{
    public class UserAdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }
        public ActionResult GetAll(SearchModel search,int index=1,int pageSize = 10)
        {
            var model = UserManager.Users;
           
            return Json(new { total = model.Count(), rows = model.OrderBy(u => u.Id).Skip(index - 1).Take(pageSize) });
        }
        public async Task<ActionResult> Get(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            return Content("ERROR");
        }
        public async Task<ActionResult>  Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded) return Content("OK");
                else ModelState.AddModelError("", result.Errors.First());
            }
            return Content(string.Join(",", ModelState.Where(m=>m.Value.Errors.Count()!=0).Select(m=> string.Join(",", m.Value.Errors.Select(e=>e.ErrorMessage)))));
        }
        public async Task<ActionResult> Update(ApplicationUser user,string id)
        {
            if (ModelState.IsValid)
            {
                var userInfo = await UserManager.FindByIdAsync(id);
                if (userInfo != null)
                {
                    userInfo.UserName = user.UserName;
                    userInfo.Email = user.Email;
                    userInfo.EmailConfirmed = user.EmailConfirmed;
                    userInfo.PhoneNumber = user.PhoneNumber;
                    userInfo.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    userInfo.Status = user.Status;
                    ;
                }
                else return Content("未找到此用户");
                var result = await UserManager.UpdateAsync(userInfo);
                if (result.Succeeded) return Content("OK");
                else ModelState.AddModelError("", result.Errors.First());
            }
            return Content(String.Join(",", ModelState.Where(m => m.Value.Errors.Count() != 0).Select(m => String.Join(",", m.Value.Errors.Select(e => e.ErrorMessage)))));
        }
        public async Task<ActionResult> Delete(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (User.Identity.Name == user.UserName) return Content("不能删除自己");
                await UserManager.DeleteAsync(user);
                return Content("OK");
            }
            return Content("删除错误");
        }
    }
}