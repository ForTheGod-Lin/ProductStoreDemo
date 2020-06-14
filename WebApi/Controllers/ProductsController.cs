using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
namespace WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public IEnumerable<ProductDTO> GetProducts()
        {
           

            var a= context.Products.OrderBy(p=>p.Id).Select(p => new ProductDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
            return a;
        }
        public ProductDTO GetProduct(int id)
        {
            var product = context.Products.Where(p => p.Id == id).FirstOrDefault();
            if (product == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return new ProductDTO() { Id = product.Id, Name = product.Name, Price = product.Price };
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
