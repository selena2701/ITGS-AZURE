using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ShippingAddressController : Controller
    {
        public IActionResult show_shipping_address()
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


                var linqContext = new ITGoShopLINQContext();
                ViewBag.DefaultShippingAddress = context.getDefaultShippingAddress(customerId);
                ViewBag.ShippingAddressList = context.getShippingAddressOfCustomer(customerId);
                ViewBag.AllThanhPho = linqContext.getAllThanhPho();
                ViewBag.AllQuanHuyen = linqContext.getAllQuanHuyen();
                ViewBag.AllXaPhuong = linqContext.getAllXaPhuong();
            }
            return View("Index");
        }
        public IActionResult add_shipping_address(ShippingAddress shippingAddress)
        {
            var linqContext = new ITGoShopLINQContext();
            shippingAddress.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            linqContext.saveShippingAddress(shippingAddress);
            return RedirectToAction("Index", "Checkout");
        }
        public void delete_shipping_address(int ShippingAddressId)
        {
            var linqContext = new ITGoShopLINQContext();
            linqContext.deleteShippingAddress(ShippingAddressId);
        }

        public string load_xaphuongthitran_dropdownbox(string maqh)
        {
            var linqContext = new ITGoShopLINQContext();
            List<devvn_xaphuongthitran> xaphuong = linqContext.load_xaphuongthitran_dropdownbox(maqh);
            string output = "";
            foreach(var item in xaphuong)
            {
                output += "<option value=" + item.Xaid + ">"+ item.Name +"</option>";
            }
            return output;
        }
        public string load_quanhuyen_dropdownbox(string matp)
        {
            var linqContext = new ITGoShopLINQContext();
            List<devvn_quanhuyen> quanhuyen = linqContext.load_quanhuyen_dropdownbox(matp);
            string output = "";
            foreach (var item in quanhuyen)
            {
                output += "<option value=" + item.Maqh + ">" + item.Name + "</option>";
            }
            return output;
        }

        public IActionResult change_default_shipping_address(int shippingAddressId)
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            var linqContext = new ITGoShopLINQContext();
            linqContext.change_default_shipping_address(shippingAddressId, customerId);
            return RedirectToAction("Index", "Checkout");
        }
        public IActionResult update_shipping_address(ShippingAddress shippingAddress)
        {
            var linqContext = new ITGoShopLINQContext();
            linqContext.update_shipping_address(shippingAddress);
            return RedirectToAction("show_shipping_address");
        }
    }
}
