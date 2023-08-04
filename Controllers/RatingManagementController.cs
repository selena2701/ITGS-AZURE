using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class RatingManagementController : Controller
    {
        public IActionResult Index()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.allRating = context.getAllRating();
            return View();
        }
        public void update_rating_status(int UserId, int ProductId, int Status)
        {
            var context = new ITGoShopLINQContext();
            context.updateRatingStatus(UserId, ProductId, Status);
        }

        public void delete_rating(int UserId, int ProductId)
        {
            var context = new ITGoShopLINQContext();
            context.deleteRating(UserId, ProductId);
        }
    }
}
