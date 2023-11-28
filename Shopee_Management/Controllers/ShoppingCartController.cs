using Antlr.Runtime.Tree;
using Shopee_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Common;
using System.Data.Entity.Validation;
using System.Diagnostics;
using PayPal.Api;
using System.ComponentModel;
using System.Security.Policy;
using System.Security.Cryptography;

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
            var model = new ModelTrangGioHang
            {
                Cart = cart,
                khuyenmai = _db.KHUYENMAIs.ToList()
            };
            return View(model);          
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

        public ActionResult HoanTatThanhToan() 
        { 
            return View();
        }

        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;

                string kh = (string)Session["ID"];

                DONHANG _order = new DONHANG();

                _order.ngay_dat = DateTime.Now;
                _order.trang_thai_dh = 1;
                _order.tt_thanh_toan = 1;
                _order.tong_cong = decimal.Parse(form["TotalMoney"]);
                _order.thanh_tien = decimal.Parse(form["DiscountedTotalMoney"]);
                _order.id_pttt = int.Parse(form["PaymentMethod"]);
                _order.id_kh = kh;
                _order.id_nbh = null;
                _order.id_voucher = int.Parse(form["Voucher"]);
                _order.ngay_giao = DateTime.Now.AddDays(3);

                _db.DONHANGs.Add(_order);

                List<CHITIETDONHANG> orderDetailsList = new List<CHITIETDONHANG>();
                
                foreach (var item in cart.Items)
                {
                    CHITIETDONHANG _order_Detail = new CHITIETDONHANG();

                    string id_nbh = _db.CHITIETSPs
                        .Where(c => c.id_ctsp == item._shopping_product.id_ctsp)
                        .Select(c => c.id_nbh).FirstOrDefault();

                    _order_Detail.id_don = _order.id_don;
                    _order_Detail.id_ctsp = item._shopping_product.id_ctsp;
                    _order_Detail.id_nbh = id_nbh;
                    _order_Detail.so_luong = item._shopping_quantity;

                    string gia = form["SubTotal"];
                    gia = gia.Replace(",", "");                   
                    _order_Detail.gia_tien = decimal.Parse(gia); 

                    _db.CHITIETDONHANGs.Add(_order_Detail);

                    orderDetailsList.Add(_order_Detail);
                }
                _db.SaveChanges();

                Session["DonHang"] = _order;
                Session["ChiTietDonHang"] = orderDetailsList;

                cart.ClearCart();
                return RedirectToAction("Index", "TrangChu");
            } 
            catch (Exception ex)
            { 
                return Content("Error" + ex.Message);
            }
        }  
    }
}