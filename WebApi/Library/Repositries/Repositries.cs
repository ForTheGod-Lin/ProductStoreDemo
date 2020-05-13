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
        public ProductRepositry() : base(new ApplicationDbContext()) { }
    }
    public class OrderRepositry : RepositryBase<Order, ApplicationDbContext>,IOrderRepositry
    {
        public OrderRepositry() : base(new ApplicationDbContext()) { }
    }
    public class CartItemRepositry : RepositryBase<CartItem, ApplicationDbContext>,ICartItemRepositry
    {
        public CartItemRepositry():base(new ApplicationDbContext()) { }
        public bool Delete(int id)
        {
            return Delete(Find(i => i.Id == id));
        }
        public int DeleteAllToOrder(string userId)
        {
            var list = FindList(i => i.UserId == userId).ToList();
            var order = new Order()
            {
                CustomerId = userId,
                OrderDetails = list.Select(item => new OrderDetail()
                {
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                }).ToList()
            };
            Context.CartItems.RemoveRange(list);
            Context.Orders.Add(order);
            Context.SaveChanges();
            return order.Id;
        }

        public IEnumerable<CartItem> Get(string id)
        {
            return FindList(i => i.UserId == id).ToList();
        }
        public bool AddCartItemByName(CartItem item)
        {
            item.UserId = Context.Users.Where(u => u.UserName == item.UserId).FirstOrDefault().Id;
            var i = Find(m => m.UserId == item.UserId && m.ProductId == item.ProductId);
            if (i != null)
            {
                i.Quantity += item.Quantity;
                return Update(i);
            }
            else return Add(item);
        }
    }
    public class MenuGroupRepositry : RepositryBase<MenuGroup, ApplicationDbContext>, IMenuGroupRepositry
    {
        public MenuGroupRepositry() : base(new ApplicationDbContext()) { }
    }
}