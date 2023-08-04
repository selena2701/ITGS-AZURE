using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ITGoShop_F_Ver2.Controllers
{
    public class AdminController : Controller
    {
        string adminId = "";
        string adminLastName = "";
        string adminFirstName = "";
        string adminImage = "";

        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string message)
        {
            if(HttpContext.Session.GetInt32("adminId") != null)
            {
                // Nếu admin đăng nhập rồi thì vô thẳng dashboard
                return RedirectToAction("Dashboard");
            }
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            return View();

        }

        public IActionResult check_password(User userInput)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            User userInfo = context.getUserInfo(userInput.Email, userInput.Password, 1);
            if (userInfo != null)
            {
                //System.Diagnostics.Debug.WriteLine("Admin: " + userInfo.UserId);
                HttpContext.Session.SetInt32("adminId", userInfo.UserId);
                HttpContext.Session.SetString("adminLastName", userInfo.LastName);
                HttpContext.Session.SetString("adminFirstName", userInfo.FirstName);
                HttpContext.Session.SetString("adminImage", userInfo.UserImage);
                var LINQContext = new ITGoShopLINQContext();
                // Update last login
                context.updateLastLogin(userInfo.UserId);
                // Thêm lịch sử đăng nhập
                LoginHistory login = new LoginHistory(userInfo.UserId, DateTime.Now, DateTime.Now);
                LINQContext.updateLoginHistory(login);
                
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Index", new { message = "Mật khẩu hoặc tài khoản sai. Xin nhập lại!" });
        }
        public IActionResult Dashboard()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            
            // Chuyển dữ liệu admin qua
            DateTime homNay = DateTime.Now;
            DateTime dauThangNay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int soNgayThangTruoc = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month) - 1;
            DateTime cuoiThangTruoc = ((new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-1)).AddDays(soNgayThangTruoc);
            DateTime dauThangTruoc = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-1);
            DateTime dauNamNay = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddYears(-1);

            ViewBag.numberCustomer = context.countCustomer();
            ViewBag.numberProduct = context.countProduct();
            ViewBag.numberOrder = context.countOrder();
            ViewBag.numberLoginThisYear = context.countLoginThisYear();
            ViewBag.totalRevenueThisMonth = context.getRevenue(dauThangNay, homNay);
            ViewBag.numberOrderToday = context.countOrder(homNay, homNay);
            ViewBag.numberLoginToday = context.countLogin(homNay, homNay);
            ViewBag.topProducts = context.getTopProduct(dauThangTruoc, homNay);
            ViewBag.topBlogView = context.getTopBlogView();
            ViewBag.topProductView = context.getTopProductView();
            ViewBag.inventoryList = context.getInventoryList();
            ViewBag.numberLoginThangNay = context.countLogin(dauThangNay, homNay);
            ViewBag.numberLoginThangTruoc = context.countLogin(dauThangTruoc, cuoiThangTruoc);
            ViewBag.numberLoginNamNay = context.countLogin(dauNamNay, homNay);
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(adminId);
            HttpContext.Session.Remove(adminLastName);
            HttpContext.Session.Remove(adminFirstName);
            HttpContext.Session.Remove(adminImage);
            return View("Index");
        }

        public List<object> load_login_chart()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            return context.countLoginByDate(DateTime.Now.AddDays(-7), DateTime.Now);
        }

        public List<object> load_pie_chart()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            return context.countOrderByDate(DateTime.Now.AddDays(-30), DateTime.Now);
        }
        public List<object> load_default_chart()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            return context.getRevenueByDate(DateTime.Now.AddDays(-14), DateTime.Now);
        }

        public List<object> filter_by_time_span(string time_span)
        {
            DateTime homNay = DateTime.Now;
            DateTime dauThangNay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int soNgayThangTruoc = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month) - 1;
            DateTime cuoiThangTruoc = ((new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-1)).AddDays(soNgayThangTruoc);
            DateTime dauThangTruoc = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-1);
            DateTime dauNamNay = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddYears(-1);

            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            if(time_span == "7ngay")
                return context.getRevenueByDate(DateTime.Now.AddDays(-7), homNay);
            if(time_span == "thangnay")
                return context.getRevenueByDate(dauThangNay, homNay);
            if (time_span == "thangtruoc")
                return context.getRevenueByDate(dauThangTruoc, cuoiThangTruoc);
            return context.getRevenueByDate(DateTime.Now.AddDays(-365), DateTime.Now);
        }

        
        public List<object> filter_by_date(DateTime den_ngay, DateTime tu_ngay)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            //System.Diagnostics.Debug.WriteLine(den_ngay);
            //System.Diagnostics.Debug.WriteLine(tu_ngay);
            return context.getRevenueByDate(tu_ngay, den_ngay);
        }

        public IActionResult customer_register(User newCustomer)
        {
            var context = new ITGoShopLINQContext();
            context.saveCustomer(newCustomer);
            System.Diagnostics.Debug.WriteLine(newCustomer.Email);
            return RedirectToAction("check_password", "Home", new RouteValueDictionary(newCustomer));
        }
    }
}
