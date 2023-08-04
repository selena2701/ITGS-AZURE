using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class SliderManagementController : Controller
    {

        [Obsolete]
        private IHostingEnvironment Environment;

        [Obsolete]
        public SliderManagementController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult view_slider()
        {
            var context = new ITGoShopLINQContext();
            ViewBag.AllBannerSlider = context.getAllBannerSliders();
            return View();
        }
        

        public void update_slider_status(int SliderId, int Status)
        {
            var context = new ITGoShopLINQContext();
            context.updateSliderStatus(SliderId, Status);
        }

        public void delete_slider(int SliderId)
        {
            var context = new ITGoShopLINQContext();
            context.deleteSlider(SliderId);
        }

        public IActionResult add_slider()
        {
            var context = new ITGoShopLINQContext();
            return View();
        }

        [Obsolete]
        public IActionResult save_slider(BannerSlider newSlider, List<IFormFile> Image)
        {
            // Lưu ảnh sản phẩm vào trước
            string path = Path.Combine(this.Environment.WebRootPath, "public/images_upload/banner-slider");
            foreach (IFormFile postedFile in Image)
            {
                // Lấy tên file
                newSlider.SliderImage = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName;
                // Lưu file vào project
                string fileName = Path.GetFileName(DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
            }

            var context = new ITGoShopLINQContext();
            context.saveSlider(newSlider);
            return RedirectToAction("view_slider");

        }
    }
}
