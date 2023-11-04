using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Management.Models.shoppingCart
{
    public class Cart
    {
        public List<CartItem> Items { get; set; }

        public static Cart GetCart()
        {
            var cart = HttpContext.Current.Session["Cart"] as Cart;
            
            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Current.Session["Cart"] = cart;
            }

            return cart;
        }

        public void AddItem(CartItem item)
        {
            if (Items == null)
                Items = new List<CartItem>();

            var existingItem = Items.FirstOrDefault(x => x.ProductId == item.ProductId && x.ProductSize == item.ProductSize);

            if (existingItem == null)
            {
                Items.Add(item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;
            }
        }


        public void RemoveItem(int productId)
        {
            Items.RemoveAll(x => x.ProductId == productId);
        }

        public void Clear()
        {
            Items = null;
            HttpContext.Current.Session.Remove("Cart");
        }

        

    }
}