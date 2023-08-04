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
    public class BrandManagementController : Controller
    {
        [Obsolete]
        private IHostingEnvironment Environment;

        [Obsolete]
        public BrandManagementController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult add_brand()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            return RedirectToAction("view_subbrand");
        }
        public IActionResult add_subbrand()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllBrand = context.getAllBrand();
            return View();
        }
        //[Obsolete]
        //public IActionResult SaveBrand(Brand newBrand)
        //{

        //    var context = new ITGoShopLINQContext();
        //    context.saveBrand(newBrand);
        //    return RedirectToAction("add_brand");

        //}

        public IActionResult SaveSubBrand(SubBrand newSubBrand)
        {

            var context = new ITGoShopLINQContext();
            context.saveSubBrand(newSubBrand);
            return RedirectToAction("add_subbrand");

        }
        public IActionResult view_brand()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllBrand = context.getAllBrandForBrandManagement();
            ViewBag.AllCategory = context.getAllCategory();
            return View();
        }

        public IActionResult view_subbrand()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllBrand = context.getAllBrandForBrandManagement();
            ViewBag.AllSubBrand = context.getAllSubBrandForBrandManagement();
            return View();
        }
        public IActionResult update_brand(int BrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            ViewBag.BrandInfo = linqContext.getBrand(BrandId);
            ViewBag.AllCategory = context.getAllCategory();
            return View();
        }

        public void unactive_brand(int BrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.updateBrandStatus(BrandId, 0);
        }
        public void active_brand(int BrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.updateBrandStatus(BrandId, 1);
        }

        public void delete_brand(int BrandId)
        {
            var context = new ITGoShopLINQContext();
            context.deleteBrand(BrandId);
        }

        [Obsolete]
        public IActionResult save_brand(Brand newBrand, List<IFormFile> BrandLogo)
        {
            // Lưu ảnh sản phẩm vào trước
            string path = Path.Combine(this.Environment.WebRootPath, "public/images_upload/brand");
            foreach (IFormFile postedFile in BrandLogo)
            {
                // Lấy tên file
                newBrand.BrandLogo = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName;
                // Lưu file vào project
                string fileName = Path.GetFileName(DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
            }

            var context = new ITGoShopLINQContext();
            context.saveBrand(newBrand);
            return RedirectToAction("view_brand");
        }
        
        [Obsolete]
        public IActionResult save_update_brand(Brand brand, List<IFormFile> BrandLogo)
        {
            if (BrandLogo.Count != 0)
            {
                
                string path = Path.Combine(this.Environment.WebRootPath, "public/images_upload/brand");
                foreach (IFormFile postedFile in BrandLogo)
                {
                    // Lấy tên file
                    brand.BrandLogo = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName;
                    // Lưu file vào project
                    string fileName = Path.GetFileName(DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                }
            }
            var context = new ITGoShopLINQContext();
            context.updateBrand(brand);
            return RedirectToAction("view_brand");

        }

        public IActionResult save_update_subbrand(SubBrand subBrandInfo)
        {

            var context = new ITGoShopLINQContext();
            context.updateSubBrand(subBrandInfo);
            return RedirectToAction("view_subbrand");

        }

        public IActionResult update_subbrand(string SubBrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            ViewBag.SubBrandInfo = linqContext.getSubBrandInfo(SubBrandId);
            ViewBag.AllBrand = context.getAllBrand();
            return View();
        }
        public void unactive_subbrand(string SubBrandId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.updateSubBrandStatus(SubBrandId, 0);
        }
        public void active_subbrand(string SubBrandId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.updateSubBrandStatus(SubBrandId, 1);
        }

        public void delete_subbrand(string SubBrandId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.deleteSubBrand(SubBrandId);
        }
    }
}
