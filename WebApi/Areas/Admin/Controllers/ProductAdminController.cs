using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }

    }
}