using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult print_revenue_report(DateTime tu_ngay, DateTime den_ngay)
        {
            if (tu_ngay == DateTime.MinValue || den_ngay == DateTime.MinValue) // DateTime is null
            {
                tu_ngay = DateTime.Now.AddDays(-14);
                den_ngay = DateTime.Now;
            }
            ITGoShopLINQContext context = new ITGoShopLINQContext();
            ViewBag.tuNgay = tu_ngay;
            ViewBag.denNgay = den_ngay;
            ViewBag.statisticInfo = context.getStatistic(tu_ngay, den_ngay);
            return View();
        }

        public IActionResult print_product_report(DateTime tu_ngay, DateTime den_ngay)
        {
            if (tu_ngay == DateTime.MinValue || den_ngay == DateTime.MinValue) // DateTime is null
            {
                tu_ngay = DateTime.Now.AddDays(-14);
                den_ngay = DateTime.Now;
            }
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            ViewBag.tuNgay = tu_ngay;
            ViewBag.denNgay = den_ngay;
            ViewBag.topProduct = context.getTopProductNotLimit(tu_ngay, den_ngay);
            ViewBag.top5Product = context.getTopProduct(tu_ngay, den_ngay);
            ViewBag.sanPhamKhongBanDuoc = context.getSanPhamKhongBanDuoc(tu_ngay, den_ngay);
            return View();
        }
        public List<object> load_product_chart(DateTime tu_ngay, DateTime den_ngay)
        {
            if (tu_ngay == DateTime.MinValue || den_ngay == DateTime.MinValue) // DateTime is null
            {
                tu_ngay = DateTime.Now.AddDays(-14);
                den_ngay = DateTime.Now;
            }
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            List<object> topProduct = context.getTopProductNotLimit(tu_ngay, den_ngay);
            List<object> top5Product = context.getTopProduct(tu_ngay, den_ngay);
            int total_revenue = topProduct.Sum(item => ((int)item.GetType().GetProperty("Price").GetValue(item, null) - (int)item.GetType().GetProperty("Cost").GetValue(item, null)) * (int)item.GetType().GetProperty("NumberSolded").GetValue(item, null));
            int revenue_san_phan_con_lai = total_revenue;

            List<object> revenueInfo = new List<object>();
            var obj = new object();
            int cost = 0, price = 0, numberSolded = 0;
            foreach (var item in top5Product)
            {
                cost = (int)item.GetType().GetProperty("Cost").GetValue(item, null);
                price = (int)item.GetType().GetProperty("Price").GetValue(item, null);
                numberSolded = (int)item.GetType().GetProperty("NumberSolded").GetValue(item, null);
                revenue_san_phan_con_lai -= (price - cost) * numberSolded;
                obj = new
                {
                    label = item.GetType().GetProperty("ProductName").GetValue(item, null),
                    value = (price - cost) * numberSolded,
                };
                revenueInfo.Add(obj);
            }
            obj = new
            {
                label = "Sản phẩm còn lại",
                value = revenue_san_phan_con_lai,
            };
            revenueInfo.Add(obj);
            return revenueInfo;
        }

        public IActionResult print_customer_report(DateTime tu_ngay, DateTime den_ngay)
        {
            if (tu_ngay == DateTime.MinValue || den_ngay == DateTime.MinValue) // DateTime is null
            {
                tu_ngay = DateTime.Now.AddDays(-14);
                den_ngay = DateTime.Now;
            }
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.tuNgay = tu_ngay;
            ViewBag.denNgay = den_ngay;
            ViewBag.customerRevenueStatistic = context.getCustomerRevenueStatistic(tu_ngay, den_ngay);
            ViewBag.customerLoginStatistic = context.getCustomerLoginStatistic(tu_ngay, den_ngay);
            ViewBag.khachHangTiemNang = context.getKhachHangTiemNang(tu_ngay, den_ngay);
            List<object> luotTruyCap = context.countLoginByDate(tu_ngay, den_ngay);
            double luotTruyCapTb = luotTruyCap.Average(item => (int)item.GetType().GetProperty("number_access").GetValue(item, null));
            ViewBag.luotTruyCapTb = luotTruyCapTb;
            return View();
        }
        public List<object> load_customer_chart(DateTime tu_ngay, DateTime den_ngay)
        {
            if (tu_ngay == DateTime.MinValue || den_ngay == DateTime.MinValue) // DateTime is null
            {
                tu_ngay = DateTime.Now.AddDays(-14);
                den_ngay = DateTime.Now;
            }
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            return context.countLoginByDate(tu_ngay, den_ngay);
        }
        public IActionResult print_invoice(int orderId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            var OrderInfo = context.getOrderInfo(orderId);
            ViewBag.DefaultShippingAddress = context.getDefaultShippingAddress((int)OrderInfo.GetType().GetProperty("UserId").GetValue(OrderInfo, null));
            ViewBag.OrderInfo = OrderInfo;
            ViewBag.OrderDetail = context.getOrderDetail(orderId);
            return View();
        }
    }
}
