﻿using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyCardSession.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace ITGoShop_F_Ver2.Controllers
{
    public class HomeController : Controller
    {
        string customerId = "";
        string customerLastName = "";
        string customerFirstName = "";
        string customerImage = "";
        string customerEmail = "";
        string total1 = "" , total2 = "";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            var linqContext = new ITGoShopLINQContext();
            ViewBag.AllCategory = linqContext.getAllCategory();
            ViewBag.AllBrand = linqContext.getAllBrand();
            ViewBag.AllSubBrand = linqContext.getAllSubBrand();
            ViewBag.AllBlog = context.getAllBlog();
            ViewBag.AllBannerSlider = context.getAllBannerSlider();
            ViewBag.Top3ProductView = context.getTop3ProductView();
            ViewBag.Top3Product = context.get3Product();
            ViewBag.Blog = context.getBlog();
            ViewBag.New = context.get2Blog();
            ViewBag.LTProduct = context.getLTProduct();
            ViewBag.PCProduct = context.getPCProduct();
            ViewBag.PKProduct = context.getPKProduct();

            ViewBag.SliderForHomePage = context.getSliderForHomePage();
            ViewBag.GiamGiaSoc = context.getGiamGiaSoc();
            return View();
        }

        public IActionResult login(string message)
        {
            if (!string.IsNullOrEmpty(ViewBag.message))
            {
                ViewBag.message = message;
            }
            return View();
        }
        public IActionResult check_password(User userInput)
        {
            System.Diagnostics.Debug.WriteLine(userInput.Email);
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            User userInfo = context.getUserInfo(userInput.Email, userInput.Password, 0);
            if (userInfo != null)
            {
                HttpContext.Session.SetInt32("customerId", userInfo.UserId);
                HttpContext.Session.SetString("customerLastName", userInfo.LastName);
                HttpContext.Session.SetString("customerFirstName", userInfo.FirstName);
                HttpContext.Session.SetString("customerImage", userInfo.UserImage);
                HttpContext.Session.SetString("customerEmail", userInfo.Email);

                var LINQContext = new ITGoShopLINQContext();
                LoginHistory login = new LoginHistory(userInfo.UserId, DateTime.Now, DateTime.Now);
                LINQContext.updateLoginHistory(login);
                // Update last login
                context.updateLastLogin(userInfo.UserId);
                return RedirectToAction("Index");
            }
            return RedirectToAction("login", new { message = "Mật khẩu hoặc tài khoản sai. Xin nhập lại!" });
        }
        public ActionResult search_result(string kw_submit)
        {
            
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.AllBlog = context.getAllBlog();
            var linqContext = new ITGoShopLINQContext();
            ViewData["CurrentFilter"] = kw_submit;
            var blog = from s in linqContext.Blog select s;
            if (!String.IsNullOrEmpty(kw_submit))
            {
                blog = blog.Where(s => s.Author.Contains(kw_submit)
                               || s.Title.Contains(kw_submit)
                               || s.Summary.Contains(kw_submit)); 
            }
            if (blog.Count() == 0)
            {
                total2 = "NOTNULL";
            }
            
            ViewBag.Result2 = blog;
            var product = from s in linqContext.Product select s;
            if (!String.IsNullOrEmpty(kw_submit))
            {
                product = product.Where(s => s.ProductName.Contains(kw_submit));
            }
            if (product.Count() == 0)
            {
                total1 = "NOTNULL";
            }
            ViewBag.Result1 = product;
            
            

            return View();
        }

        private IActionResult View(List<Blog> blogs, List<Product> products)
        {
            throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int load_cart_quantity()
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                return 0;
            }
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            return cart.Sum(item => item.Quantity);
        }

        public string load_cart()
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                return @"<img style='display: block; width: auto; height: 150px; margin-left: auto; margin-right:auto;' src='/public/client/Images/empty-cart.png'>
                <p> Bạn chưa có sản phẩm nào trong giỏ hàng</p>";
            }

            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int numberProduct = cart.Sum(item => item.Quantity);
            if(numberProduct == 0)
            {
                return @"<img style='display: block; width: auto; height: 150px; margin-left: auto; margin-right:auto;' src='/public/client/Images/empty-cart.png'>
                <p> Bạn chưa có sản phẩm nào trong giỏ hàng</p>";
            }    

            string output = "<div class='ps-cart__content'>";
            foreach (var cartItem in cart)
            {
                string image = cartItem.Product.ProductImage;
                output += @$"<div class='ps-cart-item'>
                <input type = 'hidden' class='item-id-for-cart' value='{cartItem.Product.ProductId}'/>
                <a class='ps-cart-item__close delete-button-in-nav' href='javascript:void(0)'></a>
                <div class='ps-cart-item__thumbnail'>
                    <a href = '/ProductDetail?productId={cartItem.Product.ProductId}'></a><img src='/public/images_upload/product/{cartItem.Product.ProductImage}' alt=''>
                </div>
                <div class='ps-cart-item__content'>
                    <a class='ps-cart-item__title' href='/ProductDetail?productId={cartItem.Product.ProductId}'>{cartItem.Product.ProductName}</a>
                    <p>{cartItem.Product.Price.ToString("#,###", cul.NumberFormat)}₫ x{cartItem.Quantity}</p>
                </div>
            </div>";
            }
            output += @$"</div><div class='ps-cart__total'>
            <p>Số sản phẩm:<span>{numberProduct}</span></p>
            <p>Tổng tiền:<span>{(cart.Sum(item => item.Product.Price * item.Quantity)).ToString("#,###", cul.NumberFormat)} ₫</span></p></div>
            <div class='ps-cart__footer'>
            <a href = 'javascript:void(0)' class='ps-btn btn-thanh-toan'>THANH TOÁN</a>";
            return output;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("customerId", "");
            HttpContext.Session.SetString("customerLastName", "");
            HttpContext.Session.SetString("customerFirstName", "");
            HttpContext.Session.SetString("customerImage", "");
            return RedirectToAction("Index");
        }
    }
}
