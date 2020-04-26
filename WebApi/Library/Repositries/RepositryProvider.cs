using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WebApi.Repositries
{
    public class RepositryProvider
    {
      public static IProductRepositry ProductRepositry { get { return new ProductRepositry(); } }
       public static IOrderRepositry OrderRepositry { get { return new OrderRepositry(); } }
        public static ICartItemRepositry CartItemRepositry { get { return new CartItemRepositry(); } }
    }
}
