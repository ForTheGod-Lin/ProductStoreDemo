using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class OrdersController : ApiController
    {
        private ProductStore db = new ProductStore();

        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public OrderDTO GetOrder(int id)
        {
            var order = db.Orders.Include("OrderDetails.Product").First(o => o.Id == id);
            if (order == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return new OrderDTO()
            {
                Details = order.OrderDetails.Select(d => new OrderDTO.Detail()
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    ProductName = d.Product.Name,
                    Price = d.Product.Price
                })
            };
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]


        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public HttpResponseMessage PostOrder(OrderDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var order = new Order()
            {
                Customer = "Lin",
                OrderDetails = dto.Details.Select(d => new OrderDetail()
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                }).ToList()
            };
            db.Orders.Add(order);
            db.SaveChanges();
            var url = Url.Link("DefaultApi", new { controller = "Orders",id=order.Id });
            var response = Request.CreateResponse(HttpStatusCode.Created, order);
            response.Headers.Location = new Uri(url);
            return response;
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}