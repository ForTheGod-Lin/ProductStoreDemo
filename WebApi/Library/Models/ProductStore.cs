namespace WebApi.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ApplicationUser Customer { get; set; }
        // Navigation property
        // 导航属性
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
  
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public string UserId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey("UserId")]
   public  virtual ApplicationUser User { get; set; }
    }

}