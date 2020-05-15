using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Data.Entity.Migrations;
namespace WebApi.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public enum Status
    {
        Normal,
        Forbidden
    }
    public enum Sex
    {
        Boy,
        Girl
    }
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

        }
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "注册时间")]
        public Nullable<DateTime> RegisterTime { get; set; }
        [Display(Name = "注册IP")]
        public string RegisterIP { get; set; }
        [Display(Name = "上次登录时间")]
        public Nullable<DateTime> LastLoginTime { get; set; }
        [Display(Name = "上次登录IP")]
        public string LastLoginIP { get; set; }
        [Display(Name = "出生日期")]
        public Nullable<DateTime> BirthDate { get; set; }
        [Display(Name = "性别")]
        public Sex Sex { get; set; }
        [Display(Name = "账号状态")]
        public Status Status { get; set; }
        [NotMapped]
        public string StatusString
        {
            get { return Status.ToString(); }
        }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderDetail> Orders { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class RoleMenuGroup
    {
        [Key,Column(Order=0)]
        public int MenuGroupId { get; set; }
        [Key, Column(Order = 1)]
        public string ApplicationRoleId { get; set; }
    }
    public class MenuGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<RoleMenuGroup> RoleMenus { get; set; }
    }
    public class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int MenuGroupId { get; set; }
        public virtual ICollection<MenuItem> Items { get; set; }
    }
    public class MenuItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Href { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name) : base(name) { }
        public virtual ICollection<RoleMenuGroup> RoleMenus { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext()
            : base("name=ProductStore", throwIfV1Schema: false) {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<MenuGroup> MenuGroups { get; set; }
        public DbSet<RoleMenuGroup> RoleMenuGroups { get; set; }
        static ApplicationDbContext() {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            InitializeIdentityForEF(context,userManager,roleManager);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db,ApplicationUserManager userManager,ApplicationRoleManager roleManager)
        {
    
            const string name = "admin@example.com";
            const string password = "123456";
            const string roleName = "Admin";
            var Context = new CommonContext();

            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
                var mg = new MenuGroup()
                {
                    Name = "基础管理",
                    Menus = new[]{
                        new Menu(){
                    Title="后台用户系统管理",
                    Items=new[]{
                        new MenuItem() {Href="/Admin/Home/UserIndex" ,Text="用户管理"},
                    new MenuItem(){ Href="/Admin/Home/RoleIndex",Text="角色管理"}}
                          },
                        new Menu()
                        {
                        Title="后台产品系统管理",
                        Items=new[]{ new MenuItem() { Href= "/Admin/Home/ProductIndex" ,Text="产品管理"} } } },
                };
                Context.MenuGroupRepositry.Add(mg);
                var roleMg = new RoleMenuGroup() { ApplicationRoleId = role.Id, MenuGroupId = mg.Id };
                Context.RoleMenuGroupRepositry.Add(roleMg);
            }
          
            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = name,
                    Email = name,
                    EmailConfirmed = true,
                };
                var result = userManager.Create(user, password);

                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
            var products = new List<Product>()
            {
                new Product() { Name = "Tomato Soup", Price = 1.39M, ActualCost = .99M ,Id=0},
                new Product() { Name = "Hammer", Price = 16.99M, ActualCost = 10,Id=1 },
                new Product() { Name = "Yo yo", Price = 6.99M, ActualCost = 2.05M ,Id=2}
            };

            products.ForEach(p => db.Products.AddOrUpdate(p));
            db.SaveChanges();
        }
    }

}