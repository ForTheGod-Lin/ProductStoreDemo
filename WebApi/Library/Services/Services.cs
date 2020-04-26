using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using WebApi.Repositries;
namespace WebApi.Services
{
    public class ProductService:ServiceBase<Product, ProductStore>,IProductService
    {
        public ProductService() : base(RepositryProvider.ProductRepositry) { }
    }
    public class OrderService : ServiceBase<Order, ProductStore>, IOrderService
    {
        public OrderService() : base(RepositryProvider.OrderRepositry) { }
    }
    public class CartItemService : ServiceBase<CartItem, ProductStore>, ICartItemService
    {
      
        public CartItemService():base(RepositryProvider.CartItemRepositry) { }

        public bool Delete(int id)
        {
            return Delete(Find(i => i.Id == id));
        }
        public bool DeleteAllToOrder(string userId)
        {
            var list=FindList(i => i.UserId == userId).ToList();
            var order = new Order()
            {
                CustomerId = userId,
                OrderDetails = list.Select(item => new OrderDetail()
                {
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                }).ToList()
            };
            Repositry.Context.CartItems.RemoveRange(list);
            Repositry.Context.Orders.Add(order);
            return Repositry.Context.SaveChanges() > 0;
        }
    }
}