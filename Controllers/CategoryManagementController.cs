using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Threading.Tasks;
using System.IO;


namespace ITGoShop_F_Ver2.Controllers
{
    public class CategoryManagementController : Controller
    {
        [Obsolete]
        private IHostingEnvironment Environment;

        [Obsolete]
        public CategoryManagementController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult add_product_category()
        {

            return View();
        }

        [Obsolete]
        public IActionResult SaveCategory(Category newCate)
        {

            var context = new ITGoShopLINQContext();
            context.saveCategory(newCate);
            return RedirectToAction("add_product_category");

        }
        public IActionResult all_product_category()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCateForCateManagement();
            return View();
        }
        public IActionResult update_product_category(string CategoryId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            ViewBag.CateInfo = linqContext.getCate(CategoryId);
            return View();
        }
        public void unactive_category(string CategoryId)
        {

            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.updateCateStatus(CategoryId, 0);
        }
        public void active_category(string CategoryId)
        {

            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.updateCateStatus(CategoryId, 1);
        }

        public void delete_category(string CategoryId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.deleteCategory(CategoryId);
        }

        
        public IActionResult save_update_category(Category cate)
        {
            
            var context = new ITGoShopLINQContext();
            context.updateCate(cate);
            return RedirectToAction("all_product_category");

        }
    }
}
