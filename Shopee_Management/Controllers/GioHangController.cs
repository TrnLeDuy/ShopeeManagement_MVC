using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Policy;
using System.Security.Cryptography;
using System.Data.Common;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.ComponentModel;
using Shopee_Management.Models;
using Shopee_Management.Models.shoppingCart;


namespace Shopee_Management.Controllers
{
    public class GioHangController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SanPhamGH()
        {
            var cart = Cart.GetCart();
            var cartItems = cart.Items?.GroupBy(item => new { item.ProductId, item.ProductSize });
            if (cartItems == null || cartItems.Count() == 0)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }

            var cartViewModel = new List<CartItemViewModel>();
            foreach (var itemGroup in cartItems)
            {
                var product = db.CHITIETSPs.Find(itemGroup.Key.ProductId);
                var cartItemViewModel = new CartItemViewModel
                {
                    ProductId = itemGroup.Key.ProductId,
                    ProductName = product.SANPHAM.ten_sp,
                    ProductImg = product.hinh_sp,
                    ProductSize = itemGroup.Key.ProductSize,
                    Quantity = itemGroup.Sum(i => i.Quantity),
                    Price = itemGroup.First().Price,
                    Subtotal = itemGroup.Sum(i => i.Quantity * i.Price)
                };
                cartViewModel.Add(cartItemViewModel);
            }

            return View(cartViewModel);
        }


        public ActionResult AddToCart(int productId, int quantity, string size)
        {
            if (Session["ID"] == null)
                return RedirectToAction("DangNhap", "KhachHang");

            var product = db.CHITIETSPs.Find(productId);

            CartItem cartItem = new CartItem
            {
                ProductId = product.SANPHAM.id_sp,
                ProductName = product.SANPHAM.ten_sp,
                ProductImg = product.hinh_sp,
                ProductSize = size,
                Quantity = quantity,
                Price = (decimal)product.SANPHAM.gia_sp
            };

            var cart = Cart.GetCart();
            var existingCartItem = cart.Items != null ? cart.Items.FirstOrDefault(item => item.ProductId == productId && item.ProductSize == size) : null;

            if (existingCartItem != null)
            {
                // Item already exists in cart, update its quantity
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Item does not exist in cart, add it
                cart.AddItem(cartItem);
            }

            return RedirectToAction("SanPhamGH");
        }




        public ActionResult ClearCart()
        {
            var cart = Cart.GetCart();
            cart.Clear();

            return RedirectToAction("SanPhamGH");
        }

        public int GetCartTotal()
        {
            var cart = Cart.GetCart();
            int total = 0;
            if (cart != null && cart.Items != null)
            {
                total = cart.Items.Sum(item => item.Quantity);
            }
            return total;
        }

        public ActionResult RemoveFromCart(int productId)
        {
            var cart = Cart.GetCart();
            var itemToRemove = cart.Items.SingleOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                Cart.GetCart();
            }

            return RedirectToAction("SanPhamGH");
        }

        //public ActionResult SummaryOrder()
        //{
        //    fashionDBEntities db = new fashionDBEntities();
        //    var cart = Cart.GetCart();
        //    var cartItems = cart.Items.GroupBy(item => new { item.ProductId, item.ProductSize });

        //    string tenKH = Session["Fullname"].ToString().Trim();
        //    string userID = Session["ID"].ToString().Trim();
        //    var user = db.KHACHHANGs.FirstOrDefault(k => k.MaKH == userID);
        //    var phoneNum = user.SDT.ToString().Trim();  
        //    var diaChi = user.DiaChi.ToString().Trim();     
        //    ViewBag.TenKH = tenKH;
        //    ViewBag.PhoneNumber = phoneNum;
        //    ViewBag.Address = diaChi;
        //    var paymentViewModel = new PaymentViewModel
        //    {

        //        Fullname = tenKH,
        //        PhoneNumber = phoneNum,
        //        Address = diaChi,
        //        TotalAmount = cartItems.Sum(g => g.Sum(item => item.Quantity * item.Price))
        //    };
        //    return View(paymentViewModel);
        //}    

        public ActionResult ThanhToan(string paymentMethod)
        {
            if (Session["ID"] == null)
                return RedirectToAction("DangNhap", "KhachHang");

            var cart = Cart.GetCart();
            var cartItems = cart.Items.GroupBy(item => new { item.ProductId, item.ProductSize });

            string maKH = Session["ID"].ToString().Trim();
            string tenKH = Session["HoTen"].ToString().Trim();
            var user = db.KHACHHANGs.FirstOrDefault(k => k.id_kh == maKH);
            var phoneNum = user.sdt.ToString().Trim();
            var diaChi = user.dia_chi.ToString().Trim();

            ViewBag.TenKH = tenKH;
            ViewBag.PhoneNumber = phoneNum;
            ViewBag.Address = diaChi;

            DONHANG donHang = new DONHANG
            {
                ngay_dat = DateTime.Now,
                ngay_giao = DateTime.Now.AddDays(1),
                trang_thai_dh = 0,
                tong_cong = cartItems.Sum(i => i.Sum(item => item.Quantity * item.Price)),
                id_kh = maKH
            };

            try
            {
                // Lưu dữ liệu vào cơ sở dữ liệu
                db.DONHANGs.Add(donHang);
                db.SaveChanges();
                Session["DonHang"] = donHang;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var validationError in error.ValidationErrors)
                    {
                        Debug.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                    }
                }
            }

            var cartViewModel = new List<CartItemViewModel>(); // Create this list and populate it as needed
            foreach (var itemGroup in cartItems)
            {
                var product = db.CHITIETSPs.Find(itemGroup.Key.ProductId);
                var cartItemViewModel = new CartItemViewModel
                {
                    ProductId = itemGroup.Key.ProductId,
                    ProductName = product.SANPHAM.ten_sp,
                    ProductImg = product.hinh_sp,
                    ProductSize = itemGroup.Key.ProductSize,
                    Quantity = itemGroup.Sum(i => i.Quantity),
                    Price = itemGroup.First().Price,
                    Subtotal = itemGroup.Sum(i => i.Quantity * i.Price)
                };
                cartViewModel.Add(cartItemViewModel);
            }

            return View(cartViewModel);
        }

        public ActionResult XacNhanDonDat(string paymentMethod)
        {
            string cod = "thanh toán khi nhận hàng";
            if (paymentMethod == "paypal")
            {
                var dhang = Session["DonHang"] as DONHANG;
                var dh = db.DONHANGs.SingleOrDefault(ma => ma.id_don == dhang.id_don);
                if (dh != null)
                {
                    dh.PTTT.ten_pttt = paymentMethod;
                    db.SaveChanges();
                }
                return RedirectToAction("PaymentWithPaypal", "PayPal");
            }
            else if (paymentMethod == cod)
            {
                var dhang = Session["DonHang"] as DONHANG;
                var dh = db.DONHANGs.SingleOrDefault(ma => ma.id_don == dhang.id_don);
                if (dh != null)
                {
                    dh.PTTT.ten_pttt = paymentMethod;
                    db.SaveChanges();
                }
                return RedirectToAction("HoanTatThanhToan");
            }
            return RedirectToAction("HoanTatThanhToan");
        }

        public ActionResult HoanTatThanhToan()
        {
            return View();
        }

    }
}