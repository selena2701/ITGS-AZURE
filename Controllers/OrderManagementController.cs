using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class OrderManagementController : Controller
    {
        public IActionResult view_order()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllOrder = context.getAllOrder();
            return View();
        }

        public void update_order_status(int OrderId, string OrderStatus, string PaymentStatus)
        {
            var context = new ITGoShopLINQContext();
            context.updateOrderStatus(OrderId, OrderStatus, PaymentStatus);
            context.addOrderTracking(OrderId, OrderStatus);
        }

        public IActionResult order_detail(int orderId)
        {
            /*===Cái này để load layout ===*/
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            /*======*/
           
            var OrderInfo = context.getOrderInfo(orderId);
            ViewBag.DefaultShippingAddress = context.getDefaultShippingAddress((int)OrderInfo.GetType().GetProperty("UserId").GetValue(OrderInfo, null));
            ViewBag.OrderInfo = OrderInfo;
            ViewBag.OrderDetail = context.getOrderDetail(orderId);
            return View();
        }
    }
}
