using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using Microsoft.AspNet.Identity.Owin;
using WebApi.Services;
namespace WebApi.Controllers
{
    public class CartController : ApiController
    {
        private ICartItemService context = new CartItemService();
        // GET: api/Cart
     

        // POST: api/Cart
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Cart/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Cart/5
        public HttpResponseMessage Delete(int id)
        {
            if (context.Delete(id)) return Request.CreateResponse(HttpStatusCode.NoContent);
            else throw new HttpRequestException();
        }
        public HttpResponseMessage DeleteAll([FromBody]string userId)
        {
            if (context.DeleteAllToOrder(userId)) return Request.CreateResponse(HttpStatusCode.NoContent);
            else throw new HttpRequestException();
        }
    }
}
