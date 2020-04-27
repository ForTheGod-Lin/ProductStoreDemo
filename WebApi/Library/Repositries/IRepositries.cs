using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
namespace WebApi.Repositries
{
    public  interface IProductRepositry:IRepositryBase<Product, ApplicationDbContext> {}
    public interface IOrderRepositry : IRepositryBase<Order, ApplicationDbContext> { }
    public interface ICartItemRepositry : IRepositryBase<CartItem, ApplicationDbContext> { }
}