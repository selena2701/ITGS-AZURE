using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ShipMethodManagementController : Controller
    {
        public IActionResult view_shipmethod()
        {
            var context = new ITGoShopLINQContext();
            ViewBag.AllShipMethod = context.getAllShipMethod();
            return View();
        }
        public void update_shipmethod_status(int ShipMethodId, int Status)
        {
            var context = new ITGoShopLINQContext();
            context.updateShipMethodStatus(ShipMethodId, Status);
        }

        public void delete_shipmethod(int ShipMethodId)
        {
            var context = new ITGoShopLINQContext();
            context.deleteShipMethod(ShipMethodId);
        }
        public IActionResult save_shipmethod(ShipMethod newShipMethod)
        {
            var context = new ITGoShopLINQContext();
            context.saveShipMethod(newShipMethod);
            return RedirectToAction("view_shipmethod");
        }
        public IActionResult add_shipmethod()
        {
            return View();
        }
        public IActionResult view_extra_shipfee()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.ExtraShipfee = context.getAllExtraShipfee();
            return View();
        }
        public void update_extra_shipfee(string maqh, int ExtraShippingFee)
        {
            var context = new ITGoShopLINQContext();
            context.updateExtraShipfee(maqh, ExtraShippingFee);
        }
    }
}
