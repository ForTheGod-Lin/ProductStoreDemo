using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class OrderDTO
    {
        public class Detail
        {
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public int ProductId { get; set; }
        }
        
        // Navigation property
        // 导航属性
        public IEnumerable<Detail> Details { get; set; }
    }
}