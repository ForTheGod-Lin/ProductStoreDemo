using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using Microsoft.AspNet.Identity.Owin;

namespace WebApi.Repositries
{
    public class ProductRepositry:RepositryBase<Product,ProductStore>,IProductRepositry
    {
        public ProductRepositry() : base(HttpContext.Current.GetOwinContext().Get<ProductStore>()) { }
    }
    public class OrderRepositry : RepositryBase<Order, ProductStore>,IOrderRepositry
    {
        public OrderRepositry() : base(HttpContext.Current.GetOwinContext().Get<ProductStore>()) { }
    }
    public class CartItemRepositry : RepositryBase<CartItem, ProductStore>,ICartItemRepositry
    {
        public CartItemRepositry():base(HttpContext.Current.GetOwinContext().Get<ProductStore>()) { }
    }
}