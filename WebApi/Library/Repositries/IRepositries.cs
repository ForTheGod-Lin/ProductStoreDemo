using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
namespace WebApi.Repositries
{
    public  interface IProductRepositry:IRepositryBase<Product,ProductStore> {}
    public interface IOrderRepositry : IRepositryBase<Order, ProductStore> { }
    public interface ICartItemRepositry : IRepositryBase<CartItem, ProductStore> { }
}