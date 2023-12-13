using Shopee_Management.Config;
using Shopee_Management.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace Shopee_Management.Controllers
{
    public class PayPalController : Controller
    {
        TMDTdbEntities db = new TMDTdbEntities();   

        // GET: PayPal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/PayPal/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() == "approved")
                    {
                        TMDTdbEntities _db = new TMDTdbEntities();

                        // Retrieve the DONHANG entity from the database
                        DONHANG sessionDonHang = Session["DonHang"] as DONHANG;
                        DONHANG dbDonHang = _db.DONHANGs.Find(sessionDonHang.id_don);

                        // Update tt_thanhtoan to 2
                        dbDonHang.tt_thanh_toan = 2;

                        // Save changes to the database
                        _db.SaveChanges();
                    }
                    // If executed payment failed, redirect to an appropriate page
                    else
                    {
                        // return RedirectToAction("Message", "Cart", new { mess = "Lỗi" });
                        return RedirectToAction("Index", "TrangChu");
                    }
                }
            }
            catch (Exception ex)
            {
                //return RedirectToAction("Message", "Cart", new
                //{
                //    mess = ex.Message
                //});
                return RedirectToAction("Index", "TrangChu");
            }
            
            TMDTdbEntities db = new TMDTdbEntities();

            DONHANG donHang = Session["DonHang"] as DONHANG;

            var khachHang = db.KHACHHANGs.SingleOrDefault(k => k.id_kh == donHang.id_kh);

            new Shopee_Management.Models.mapContactEmail.mapContactEmail().SendEmail(khachHang.email, "Thanh toán thành công", "<p style=\"font-size:20px\">Cảm ơn bạn đã đặt sản phẩm của chúng tôi <br/>Mã đơn hàng của bạn là: " + donHang.id_don);

            return RedirectToAction("PaymentSuccess", "ShoppingCart");
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectURl)
        {
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            if (Session["DonHang"] != null)
            {
                decimal usdToVndRate = 23465m;

                DONHANG donHang = Session["DonHang"] as DONHANG;

                string strThanhTien = Math.Round(donHang.thanh_tien.Value / usdToVndRate).ToString();

                List<CHITIETDONHANG> ctdon = Session["ChiTietDonHang"] as List<CHITIETDONHANG>;
                if (ctdon != null)
                {
                    foreach (var ct in ctdon)
                    {
                        // Check if CHITIETSP is null
                        if (ct.CHITIETSP != null)
                        {
                            var sanPham = ct.CHITIETSP.SANPHAM;

                            // Check if SANPHAM is null
                            if (sanPham != null)
                            {
                                var ten_sp = sanPham.ten_sp ?? "Unknown Product";
                                var id_sp = sanPham.id_sp;

                                itemList.items.Add(new Item()
                                {
                                    name = ten_sp,
                                    currency = "USD",
                                    price = (Math.Round(ct.gia_tien.Value / usdToVndRate)).ToString(),
                                    quantity = ct.so_luong.ToString(),
                                    sku = id_sp.ToString(),
                                });
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where the Session["CTDH"] is null
                }

                var payer = new Payer()
                {
                    payment_method = "paypal"
                };

                var redirUrl = new RedirectUrls()
                {
                    cancel_url = redirectURl + "&Cancel=true",
                    return_url = redirectURl
                };

                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = strThanhTien,
                };

                var amount = new Amount()
                {
                    currency = "USD",
                    total = details.subtotal,
                    details = details,
                };

                var transactionList = new List<Transaction>();
                // Adding description about the transaction  
                var paypalOrderId = DateTime.Now.Ticks;
                transactionList.Add(new Transaction()
                {
                    description = $"Invoice #{paypalOrderId}",
                    invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                    amount = amount,
                    item_list = itemList
                });


                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrl
                };

                try
                {
                    return this.payment.Create(apiContext);
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return null;
        }

        public Decimal GetCurrencyExchange(String localCurrency, String foreignCurrency)
        {
            var code = $"{localCurrency}_{foreignCurrency}";
            var newRate = FetchSerializedData(code);
            return newRate;
        }

        private Decimal FetchSerializedData(String code)
        {
            var url = "https://free.currconv.com/api/v7/convert?q=" + code + "&compact=y&apiKey=7cf3529b5d3edf9fa798";
            var webClient = new WebClient();
            var jsonData = String.Empty;

            var conversionRate = 1.0m;
            try
            {
                jsonData = webClient.DownloadString(url);
                var jsonObject = new JavaScriptSerializer().Deserialize<Dictionary<string, Dictionary<string, decimal>>>(jsonData);
                var result = jsonObject[code];
                conversionRate = result["val"];
            }
            catch (Exception) { }

            return conversionRate;
        }
    }
}