using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;

namespace ITGoShop_F_Ver2.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
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

                ViewBag.WishList = context.getWishList(customerId);
                return View();
            }
            return RedirectToAction("login", "Home");
        }
        public void remove_product_from_wishlist(int ProductId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            linqContext.remove_product_from_wishlist(customerId, ProductId);
        }

        public int add_product_to_wishlist(int ProductId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            if (linqContext.isProductExistInWishlist(customerId, ProductId) != 0)
            {
                linqContext.add_product_to_wishlist(customerId, ProductId);
                return 1;
            }
            return 0; // Sản phẩm đã tồn tại trong wishlist
        }
    }
}
