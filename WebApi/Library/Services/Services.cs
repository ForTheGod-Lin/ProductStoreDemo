using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using WebApi.Repositries;
namespace WebApi.Services
{
    public class ProductService:ServiceBase<Product, ApplicationDbContext>,IProductService
    {
        public ProductService() : base(RepositryProvider.ProductRepositry) { }
    }
    public class OrderService : ServiceBase<Order, ApplicationDbContext>, IOrderService
    {
        public OrderService() : base(RepositryProvider.OrderRepositry) { }
    }
    public class CartItemService : ServiceBase<CartItem, ApplicationDbContext>, ICartItemService
    {
      
        public CartItemService():base(RepositryProvider.CartItemRepositry) { }

        public bool Delete(int id)
        {
            return Delete(Find(i => i.Id == id));
        }
        public int DeleteAllToOrder(string userId)
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
            Repositry.Context.SaveChanges();
            return order.Id;
        }

        public IEnumerable<CartItem> Get(string id)
        {
            return FindList(i => i.UserId == id).ToList();
        }
        public bool AddCartItemByName(CartItem item)
        {
            item.UserId = Repositry.Context.Users.Where(u => u.UserName == item.UserId).FirstOrDefault().Id;
            var i = Find(m => m.UserId == item.UserId && m.ProductId == item.ProductId);
            if (i != null)
            {
                i.Quantity += item.Quantity;
                return Update(i);
            }
            else return Add(item);
        }
    }
}