﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public  interface IProductRepositry:IRepositryBase<Product, ApplicationDbContext> {}
    public interface IOrderRepositry : IRepositryBase<Order, ApplicationDbContext> { }
    public interface ICartItemRepositry : IRepositryBase<CartItem, ApplicationDbContext> {
        bool Delete(int id);
        int DeleteAllToOrder(string userId);
        IEnumerable<CartItem> Get(string id);
        bool AddCartItemByName(CartItem item);
    }
    public interface IMenuGroupRepositry : IRepositryBase<MenuGroup, ApplicationDbContext> { }
    public interface IMenuRepositry : IRepositryBase<Menu, ApplicationDbContext> { }
    public interface IRoleMenuGroupRepositry : IRepositryBase<RoleMenuGroup, ApplicationDbContext> { }
}