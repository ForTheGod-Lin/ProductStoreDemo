using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
namespace WebApi.Services
{
    public interface IProductService : IServiceBase<Product> { }
    public interface IOrderService : IServiceBase<Order> { }
    public interface ICartItemService : IServiceBase<CartItem> {
        bool Delete(int id);
       int DeleteAllToOrder(string userId);
        IEnumerable<CartItem> Get(string id);
        bool AddCartItemByName(CartItem item);
    }
}