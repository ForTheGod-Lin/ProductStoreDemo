﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebApi.Models;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using System.Net.Http;
namespace WebApi.Areas.Admin.Controllers
{
    public class RoleController : ApiController
    {

        private CommonContext _context;
        public CommonContext Context
        {
            get
            {
                if (_context == null) _context = new CommonContext(Request.GetOwinContext());
                return _context;
            }
        }

        // GET: Admin/roleAdmin
        public object GetAll(int page = 1, int rows = 10)
        {
            var model = Context.RoleManager.Roles;
            return new { total = model.Count(), rows = model.OrderBy(u => u.Id).Skip((page - 1) * rows).Take(rows) };
        }
     
        public object Get(string id)
        {
            var role =  Context.RoleManager.FindById(id);
            if (role != null)
            {
               
                return role;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
        public  HttpResponseMessage Post(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var result =  Context.RoleManager.Create(role);
                if (result.Succeeded)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Created, role);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = role.Id, Controller = "Role" }));
                    return response;
                }
                else ModelState.AddModelError("", result.Errors.First());
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", ModelState.Where(m => m.Value.Errors.Count() != 0).Select(m => String.Join(",", m.Value.Errors.Select(e => e.ErrorMessage)))));
        }
        public void Put(ApplicationRole role,string id)
        {
            if (ModelState.IsValid)
            {
                var roleInfo = Context.RoleManager.FindById(id);
                if (roleInfo != null)
                {
                    roleInfo.Name = role.Name;
                }
                else throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "未找到此用户"));
                var result =  Context.RoleManager.Update(roleInfo);
                if (result.Succeeded) return;
                else ModelState.AddModelError("", result.Errors.First());
            }
            var message = string.Join(",", ModelState.Where(m => m.Value.Errors.Count() != 0).Select(m => String.Join(",", m.Value.Errors.Select(e => e.ErrorMessage))));
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
        }
        public void Delete(string id)
        {
            var role =  Context.RoleManager.FindById(id);
            if (role != null)
            {
                 Context.RoleManager.Delete(role);
                return ;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
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