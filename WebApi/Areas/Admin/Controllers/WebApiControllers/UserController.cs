using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using Microsoft.AspNet.Identity;
using System.IO;
namespace WebApi.Controllers
{
    public class UserController : ApiController
    {
      
        public ApplicationRoleManager RoleManager
        {

            get { return Request.GetOwinContext().Get<ApplicationRoleManager>();}
        }
        public ApplicationUserManager UserManager
        {
            get { return Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }
        public object GetAll(int page=1,int rows = 10)
        {
            var model = UserManager.Users;
            return new { total = model.Count(), rows = model.OrderBy(u => u.Id).Skip((page - 1) * rows).Take(rows) };
        }
        public object Get(string id)
        {
            var user =UserManager.FindByIdAsync(id).Result;
            if (user != null)
            {
                var userRoles =  UserManager.GetRolesAsync(user.Id).Result;
                var Roles = RoleManager.Roles.Select(r => new
                {
                    Selected = userRoles.Contains(r.Name),
                    Name = r.Name
                });
                return new { User=user,Roles=Roles};
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
        public HttpResponseMessage Post(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result =  UserManager.Create(user);
                if (result.Succeeded)
                {
                    Stream stream =  Request.Content.ReadAsStreamAsync().Result;
                    stream.Seek(0, SeekOrigin.Begin);
                    string[] Roles = Request.Content.ReadAsFormDataAsync().Result.GetValues("RoleNames");
                   
                    if (Roles != null)
                    {
                        var result1 = UserManager.AddToRoles(user.Id, Roles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result1.Errors.First());
                        }
                    }
                    var response = Request.CreateResponse(HttpStatusCode.Created, user);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.Id,Controller="User" }));
                    return response;
                }
                else ModelState.AddModelError("", result.Errors.First());
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", ModelState.Where(m => m.Value.Errors.Count() != 0).Select(m => string.Join(",", m.Value.Errors.Select(e => e.ErrorMessage)))));
        }
        public void Put(ApplicationUser user,string id)
        {
            if (ModelState.IsValid)
            {
                var userInfo = UserManager.FindById(id);
                if (userInfo != null)
                {
                    userInfo.UserName = user.UserName;
                    userInfo.Email = user.Email;
                    userInfo.EmailConfirmed = user.EmailConfirmed;
                    userInfo.PhoneNumber = user.PhoneNumber;
                    userInfo.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    userInfo.Status = user.Status;
                    var userRoles = UserManager.GetRoles(id);
                    Stream stream = Request.Content.ReadAsStreamAsync().Result;
                    stream.Seek(0, SeekOrigin.Begin);
                    string[] Roles = Request.Content.ReadAsFormDataAsync().Result.GetValues("RoleNames");
                    Roles = Roles ?? new string[] { };

                    var result1 = UserManager.AddToRoles(id, Roles.Except(userRoles).ToArray());

                    if (!result1.Succeeded)
                    {
                        ModelState.AddModelError("", result1.Errors.First());
                    }
                    else
                    {
                        result1 = UserManager.RemoveFromRoles(id, userRoles.Except(Roles).ToArray());

                        if (!result1.Succeeded)
                        {
                            ModelState.AddModelError("", result1.Errors.First());
                        }
                        else
                        {
                           result1 = UserManager.Update(userInfo);
                            if (result1.Succeeded) return;
                            else ModelState.AddModelError("", result1.Errors.First());
                        }

                    }
                }
                else throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound,"未找到此用户"));
            }
            var message=string.Join(",", ModelState.Where(m => m.Value.Errors.Count() != 0).Select(m => String.Join(",", m.Value.Errors.Select(e => e.ErrorMessage))));
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
        }
        public void Delete(string id)
        {
            var user =  UserManager.FindById(id);
            if (user != null)
            {
                if (User.Identity.Name == user.UserName) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "不能删除自己"));
                 UserManager.Delete(user);
                return ;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}