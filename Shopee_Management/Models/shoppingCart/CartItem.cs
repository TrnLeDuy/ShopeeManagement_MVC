using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Management.Models.shoppingCart
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImg { get; set; }
        public string ProductSize { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }     
    }
    
}
