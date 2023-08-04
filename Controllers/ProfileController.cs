using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;


namespace ITGoShop_F_Ver2.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult profile()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            if (customerId != 0) // Nếu customer đã đăng nhập
            {
                /*===Cái này để load layout ===*/
                ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
                ViewBag.AllCategory = context.getAllCategory();
                ViewBag.AllBrand = context.getAllBrand();
                ViewBag.AllSubBrand = context.getAllSubBrand();
                /*======*/

                ViewBag.DefaultShippingAddress = context.getDefaultShippingAddress(customerId);
                ViewBag.Profile = context.getProfile(customerId);

                return View();
            }
            return RedirectToAction("login", "Home");
            
        }
    }
}
