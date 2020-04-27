using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using Microsoft.AspNet.Identity.Owin;

namespace WebApi.Repositries
{
    public class ProductRepositry:RepositryBase<Product,ApplicationDbContext>,IProductRepositry
    {
        public ProductRepositry() : base(HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>()) { }
    }
    public class OrderRepositry : RepositryBase<Order, ApplicationDbContext>,IOrderRepositry
    {
        public OrderRepositry() : base(HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>()) { }
    }
    public class CartItemRepositry : RepositryBase<CartItem, ApplicationDbContext>,ICartItemRepositry
    {
        public CartItemRepositry():base(HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>()) { }
    }
}