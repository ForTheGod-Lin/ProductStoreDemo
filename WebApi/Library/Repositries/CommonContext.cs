using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
namespace WebApi.Models
{
    public class CommonContext:IDisposable
    {
        public CommonContext(IOwinContext context)
        {

            ProductRepositry = new ProductRepositry(context);
            OrderRepositry = new OrderRepositry(context);
            CartItemRepositry =new CartItemRepositry(context);
            MenuGroupRepositry =new  MenuGroupRepositry(context);
            UserManager = context.GetUserManager<ApplicationUserManager>();
            RoleManager = context.Get<ApplicationRoleManager>();
        }
        public CommonContext()
        {

            ProductRepositry = new ProductRepositry();
            OrderRepositry = new OrderRepositry();
            CartItemRepositry = new CartItemRepositry();
            MenuGroupRepositry = new MenuGroupRepositry();
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
        }
        public static CommonContext Create(IdentityFactoryOptions<CommonContext> options,
            IOwinContext context)
        {
            return new CommonContext(context);
        }
        public IProductRepositry ProductRepositry;
        public IOrderRepositry OrderRepositry;
        public ICartItemRepositry CartItemRepositry;
        public IMenuGroupRepositry MenuGroupRepositry;
        public ApplicationUserManager UserManager;
        public ApplicationRoleManager RoleManager;
        public IMenuRepositry MenuRepositry;
        public IRoleMenuGroupRepositry RoleMenuGroupRepositry;
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~CommonContext() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}