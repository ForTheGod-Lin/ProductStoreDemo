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
       bool DeleteAllToOrder(string userId);
    }
}