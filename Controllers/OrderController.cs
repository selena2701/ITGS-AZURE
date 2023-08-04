using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using MyCardSession.Helpers;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace ITGoShop_F_Ver2.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult my_orders()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            if (customerId != 0) // Nếu customer đã đăng nhập
            {
                /*===Cái này để load layout ===*/
                ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
                var linqContext = new ITGoShopLINQContext();
                ViewBag.AllCategory = linqContext.getAllCategory();
                ViewBag.AllBrand = linqContext.getAllBrand();
                ViewBag.AllSubBrand = linqContext.getAllSubBrand();
                /*======*/

                ViewBag.OrderList = linqContext.getOrderListOfCustomer(customerId);
                return View();
            }
            return RedirectToAction("login", "Home");
        }
        public IActionResult order_detail(int orderId)
        {
            /*===Cái này để load layout ===*/
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            /*======*/
            var linqContext = new ITGoShopLINQContext();
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            ViewBag.DefaultShippingAddress = context.getDefaultShippingAddress(customerId);
            ViewBag.OrderInfo = linqContext.getOrderInfo(orderId);
            ViewBag.OrderDetail = context.getOrderDetail(orderId);
            return View();
        }

        public void cancel_order(int orderId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.updateOrderStatus(orderId, "Đã hủy");
            List<object> orderDetail = context.getOrderDetail(orderId);

            // Cập nhật số lượng tồn kho và đã bán của các sản phẩm trong đơn hàng
            linqContext.updateSoldProduct(orderDetail);
        }
        
        public int is_rating_exit(int ProductId)
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            return linqContext.isRatingeExit(ProductId, customerId);
        }

        public void add_rating(int ProductId, string Title, string Content, int Rating)
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.addRating(ProductId, Title, Content, Rating, customerId);
        }

        public string get_product(int ProductId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            Product product = context.getProductInfo(ProductId);
            string output = $"<p><b style='font-size:20px'>ĐÁNH GIÁ SẢN PHẨM #{product.ProductId}</b></p><img src='/public/images_upload/product/{product.ProductImage}'  style='margin: auto; max-width: 100px; max-height: 80px; width: auto; height: auto;'/>";
            output += $"<p style='display:inline-block; margin-left:10px'>{product.ProductName}</p>";
            output += $"<input type='hidden' value='{product.ProductId}</p>";
            return output;
        }
    }
}
