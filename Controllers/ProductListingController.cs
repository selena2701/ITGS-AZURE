using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using X.PagedList;
using System.Linq.Dynamic; // nhúng vào tập tin 
using System.Linq.Expressions; // nhúng vào tập tin 

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductListingController : Controller
    {
        
        private ITGoShopContext db = new ITGoShopContext();
        public object product_listing(int brandId)
        {
            

            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.Brand = context.getBrand(brandId);

            var linqContext = new ITGoShopLINQContext();
            var BProduct = from s in linqContext.getBrandProduct(brandId)
                           select s;
            
            ViewBag.OnePageOfProducts = BProduct;
            ViewBag.SubBrand = linqContext.getSubBrand(brandId);
            var minp = from s in linqContext.getBrandProduct(brandId)
                       group s by s.BrandId into g
                       select new { mn = g.Min(s=>s.Price)};

            var maxp = from s in linqContext.getBrandProduct(brandId)
                       group s by s.BrandId into g
                       select new { mx = g.Max(s => s.Price) };
            ViewBag.min_price = Convert.ToInt32((minp.FirstOrDefault()).mn);
            ViewBag.max_price = Convert.ToInt32((maxp.FirstOrDefault()).mx);
            ViewBag.min_price_range = ViewBag.min_price - 400000;
            ViewBag.max_price_range = ViewBag.max_price + 40000000;

            return View();
        }
        public object product_listing2(int? page,string categoryId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.Cate = context.getCate(categoryId);

            var linqContext = new ITGoShopLINQContext();
            var CateProduct = linqContext.getCateProduct(categoryId);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = CateProduct.ToPagedList(pageNumber, 6); // will only contain 25 products max because of the pageSize

            var SubBrand = linqContext.getB(categoryId);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            ViewBag.SubBrand = linqContext.getB(categoryId);

            var minp = from s in linqContext.getCateProduct(categoryId)
                       group s by s.BrandId into g
                       select new { mn = g.Min(s => s.Price) };

            var maxp = from s in linqContext.getCateProduct(categoryId)
                       group s by s.BrandId into g
                       select new { mx = g.Max(s => s.Price) };
            ViewBag.min_price = Convert.ToInt32((minp.FirstOrDefault()).mn);
            ViewBag.max_price = Convert.ToInt32((maxp.FirstOrDefault()).mx);
            ViewBag.min_price_range = ViewBag.min_price - 400000;
            ViewBag.max_price_range = ViewBag.max_price + 40000000;

            return View();
        }
        public object product_listing3(int? page,string subbrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();

            var linqContext = new ITGoShopLINQContext();
            var SubProduct = linqContext.getSubProduct(subbrandId);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = SubProduct.ToPagedList(pageNumber, 6); // will only contain 25 products max because of the pageSize

            ViewBag.OnePageOfProducts = onePageOfProducts;

            ViewBag.Brand = context.getSub(subbrandId);
            ViewBag.SubBrand = linqContext.getSB(subbrandId);

            var minp = from s in linqContext.getSubProduct(subbrandId)
                       group s by s.BrandId into g
                       select new { mn = g.Min(s => s.Price) };

            var maxp = from s in linqContext.getSubProduct(subbrandId)
                       group s by s.BrandId into g
                       select new { mx = g.Max(s => s.Price) };
            ViewBag.min_price = Convert.ToInt32((minp.FirstOrDefault()).mn);
            ViewBag.max_price = Convert.ToInt32((maxp.FirstOrDefault()).mx);
            ViewBag.min_price_range = ViewBag.min_price - 400000;
            ViewBag.max_price_range = ViewBag.max_price + 40000000;
            return View();
        }
        public IActionResult product_listing4(int? page,string brandName)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();

            var linqContext = new ITGoShopLINQContext();
            var BNProduct = linqContext.getBNProduct(brandName);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = BNProduct.ToPagedList(pageNumber, 6); // will only contain 25 products max because of the pageSize

            ViewBag.OnePageOfProducts = onePageOfProducts;
            ViewBag.Brand = context.getBrand(brandName);
            ViewBag.SubBrand = linqContext.getSBN(brandName);

            var minp = from s in linqContext.getBNProduct(brandName)
                       group s by s.BrandId into g
                       select new { mn = g.Min(s => s.Price) };

            var maxp = from s in linqContext.getBNProduct(brandName)
                       group s by s.BrandId into g
                       select new { mx = g.Max(s => s.Price) };
            ViewBag.min_price = Convert.ToInt32((minp.FirstOrDefault()).mn);
            ViewBag.max_price = Convert.ToInt32((maxp.FirstOrDefault()).mx);
            ViewBag.min_price_range = ViewBag.min_price - 400000;
            ViewBag.max_price_range = ViewBag.max_price + 40000000;
            return View();
        }
    }
}
