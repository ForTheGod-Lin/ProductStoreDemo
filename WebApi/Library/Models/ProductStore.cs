namespace WebApi.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    public class ProductStore : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“ProductStore”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“WebApi.Models.ProductStore”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“ProductStore”
        //连接字符串。
        public ProductStore()
            : base("name=ProductStore")
        {
            Database.SetInitializer(new InitSerializer());
        }
        public static ProductStore Create()
        {
            return new ProductStore();
        }
       public DbSet<Product> Products { get; set; }
       public DbSet<Order> Orders { get; set; }
       public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
   public class InitSerializer : DropCreateDatabaseIfModelChanges<ProductStore>
    {
        protected override void Seed(ProductStore context)
        {
            var products = new List<Product>()
            {
                new Product() { Name = "Tomato Soup", Price = 1.39M, ActualCost = .99M },
                new Product() { Name = "Hammer", Price = 16.99M, ActualCost = 10 },
                new Product() { Name = "Yo yo", Price = 6.99M, ActualCost = 2.05M }
            };

            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();

        }
    }
    public class Product
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal ActualCost { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public ApplicationUser Customer { get; set; }
        // Navigation property
        // 导航属性
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
  
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public string UserId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("UserId")]
   public ApplicationUser User { get; set; }
    }

}