using Shopee_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Management.Controllers
{
    public class ShoppingCartController : Controller
    {
        TMDTdbEntities _db = new TMDTdbEntities();
        // GET: ShoppingCart

        public Cart GetCart() 
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        // phương thức add item vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            var pro = _db.CHITIETSPs.SingleOrDefault(s => s.id_ctsp == id);
            if (pro != null)
            {
                GetCart().Add(pro);
            }
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }

        // trang giỏ hàng
        public ActionResult ShowToCart()
        {
            if (Session["Cart"] == null)         
                return RedirectToAction("ShowToCart", "ShoppingCart");
            Cart cart = Session["Cart"] as Cart;
            return View(cart);          
        }

        public ActionResult Update_Quantity_Cart(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["ID_Product"]);
            int quantity = int.Parse(form["Quantity"]);
            cart.Update_Quantity_Shopping(id_pro, quantity);
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }

        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }

        public PartialViewResult BagCart() 
        {
            int _t_item = 0; //total item
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                _t_item = cart.Total_Quantity();
            }
            ViewBag.infoCart = _t_item;
            return PartialView("BagCart");
        }

        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                DONHANG _order = new DONHANG();
                _order.ngay_dat = DateTime.Now;
                _order.trang_thai_dh = 1;
                _order.tt_thanh_toan = 1;
                _order.id_pttt = null;
                _order.ngay_giao = DateTime.Now.AddDays(3);
                foreach(var item in cart.Items)
                {
                    CHITIETDONHANG _order_Detail = new CHITIETDONHANG();
                    _order_Detail.id_don = _order.id_don;
                    _order_Detail.id_ctsp = item._shopping_product.id_ctsp;
                    _order_Detail.so_luong = item._shopping_quantity;
                    _db.CHITIETDONHANGs.Add( _order_Detail );    
                }
            } 
            catch 
            { 
                
            }
        }
    }
}