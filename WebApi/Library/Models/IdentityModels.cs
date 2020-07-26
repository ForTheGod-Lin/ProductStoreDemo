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
using System.Linq;
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
        public MenuGroup()
        {
            Menus = new HashSet<Menu>();
            Roles = new HashSet<RoleMenuGroup>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<RoleMenuGroup> Roles { get; set; }
    }
    public class Menu
    {
        public Menu()
        {
            MenuItems = new HashSet<MenuItem>();
            Roles = new HashSet<ApplicationRole>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int MenuGroupId { get; set; }
        public string ApplicationRoleId { get; set; }
      //  public virtual MenuGroup MenuGroup { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<ApplicationRole> Roles { get; set; }

    }
    public class MenuItem
    {
        public MenuItem()
        {
            Roles = new HashSet<ApplicationRole>();
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public string Href { get; set; }
        public string Icon { get; set; }
        public int MenuId { get; set; }
        public virtual ICollection<ApplicationRole> Roles { get; set; }
    //    public virtual Menu Menu { get; set; }
    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
            Menus = new HashSet<Menu>();
            MenuGroups = new HashSet<RoleMenuGroup>();
            MenuItems = new HashSet<MenuItem>();
        }
        public ApplicationRole(string name) : base(name)
        {
            Menus = new HashSet<Menu>();
            MenuGroups = new HashSet<RoleMenuGroup>();
            MenuItems = new HashSet<MenuItem>();
        }
        public virtual ICollection<RoleMenuGroup> MenuGroups { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
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
           
            InitializeIdentityForEF(context,new CommonContext(HttpContext.Current.GetOwinContext()));
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db, CommonContext context)
        {
    
            const string name = "admin@example.com";
            const string password = "123456";
            const string roleName = "Admin";
          
            ApplicationUserManager userManager = context.UserManager;
            ApplicationRoleManager roleManager = context.RoleManager;
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                List<MenuGroup> mgL = new List<MenuGroup>
                {
                    new MenuGroup()
                    {
                        Name = "基础管理",
                        Icon="icon-large-shapes",
                        Menus = new[]{
                    new Menu(){
                    Title="后台用户系统管理" ,Icon="icon-save",MenuItems=new[]{
                        new MenuItem() {Href="/Admin/Home/UserIndex" ,Text="用户管理",Icon="icon-save"},
                         new MenuItem(){ Href="/Admin/Home/RoleIndex",Text="角色管理",Icon="icon-save"}}},
                    new Menu(){Title="后台产品系统管理",Icon="icon-save",MenuItems=new[]{
                        new MenuItem() { Href= "/Admin/Home/ProductIndex" ,Text="产品管理",Icon="icon-save"}}}},
                    },
                    new MenuGroup()
                    {
                        Name="菜单管理",
                        Menus=new[]{new Menu() { Title = "角色权限管理", Icon = "icon-save", MenuItems = new[] {new MenuItem() { Href= "/Admin/Home/MenuDistribution", Text="权限管理", Icon = "icon-save" } }} },
                        Icon="icon-large-clipart"
                    }
                };
                db.Set<ApplicationRole>().Add(role);
                mgL.ForEach(mg => {
                    db.MenuGroups.Add(mg);
                    db.SaveChanges();
                    var roleMg = new RoleMenuGroup() { ApplicationRoleId = role.Id, MenuGroupId = mg.Id };
                    db.RoleMenuGroups.Add(roleMg);
                    foreach (var m in mg.Menus)
                    {
                        role.Menus.Add(m);
                        foreach(var i in m.MenuItems)
                        {
                            role.MenuItems.Add(i);
                        }
                    }
                });
                db.SaveChanges();
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