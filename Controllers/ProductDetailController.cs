using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCardSession.Helpers;
using Microsoft.AspNetCore.Http;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductDetailController : Controller
    {
        public IActionResult product_detail(int productId)
        {
            //System.Diagnostics.Debug.WriteLine("Chạy product detail");
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            var linqContext = new ITGoShopLINQContext();
            ViewBag.AllCategory = linqContext.getAllCategory();
            ViewBag.AllBrand = linqContext.getAllBrand();
            ViewBag.AllSubBrand = linqContext.getAllSubBrand();

            var productDetail = context.getProductDetail(productId);
            string categoryId = (string)productDetail.GetType().GetProperty("CategoryId").GetValue(productDetail, null);
            int brandId = (int)productDetail.GetType().GetProperty("BrandId").GetValue(productDetail, null);

            ViewBag.ProductDetail = productDetail;
            ViewBag.RelatedProduct = linqContext.getRelatedProduct(productId, categoryId, brandId);
            ViewBag.ProductGallary = linqContext.getProductGallary(productId);
            List<object> ratingList = context.getRating(productId);
            ViewBag.RatingList = ratingList;
            if(ratingList.Count > 0)
                ViewBag.AvgRating = Math.Round(ratingList.Average(item => (int)item.GetType().GetProperty("Rating").GetValue(item, null)), 1);
            return View("Index");
        }

        public IActionResult product_detail2()
        {
            //System.Diagnostics.Debug.WriteLine("Chạy product detail");
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            return View();
        }

        public IActionResult save_cart(int productId, int quantity)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;

            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                List<CartItem> cart = new List<CartItem>(); //mảng các item
                CartItem newCartItem = new CartItem { Product = context.getProductInfo(productId), Quantity = quantity };
                cart.Add(newCartItem);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = isExist(productId);
                if (index != -1)
                {
                    cart[index].Quantity += quantity;
                }
                else
                {
                    cart.Add(new CartItem { Product = context.getProductInfo(productId), Quantity = quantity });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("show_cart", "Cart");
        }

        private int isExist(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public string load_comment(int ProductId)
        {
            string output = "";
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            List<object> parentCommentList = context.getCommentParentCommentForProductDetail(ProductId);
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            int adminId = Convert.ToInt32(HttpContext.Session.GetInt32("adminId"));
            foreach (var item in parentCommentList)
            {
                string created_at = ((DateTime)item.GetType().GetProperty("CreatedAt").GetValue(item, null)).ToString("HH:mm dd/MM/yyyy"); 
                string userImage = item.GetType().GetProperty("UserImage").GetValue(item, null).ToString();
                if (string.IsNullOrEmpty(userImage))
                {
                    userImage = "default-user-icon.png";
                }
                output += @$"
                <div class='o-comment'><div class='single-comment'>
                <img src = '/public/images_upload/user/{userImage}' alt='#'>
                <div class='content'><input type = 'text' class='CommentId' value='{item.GetType().GetProperty("CommentId").GetValue(item, null)}' hidden>";
                int userCommentId = (int)item.GetType().GetProperty("UserId").GetValue(item, null);
                if (userCommentId == customerId || userCommentId == adminId)
                {
                    output += @"<div class='button' style='float:right'>
                                    <a href='javascript:void(0)' class='btn btn-xoa-comment'><i class='fa fa-times' aria-hidden='true'></i></a>
                                </div>";
                }
                output += @$"<h4>{item.GetType().GetProperty("LastName").GetValue(item, null)} {item.GetType().GetProperty("FirstName").GetValue(item, null)}<span>{created_at}</span></h4>
                    <p> {item.GetType().GetProperty("CommentContent").GetValue(item, null)}</p>
                    <input type = 'text' class='ParentCommentId' value='{item.GetType().GetProperty("CommentId").GetValue(item, null)}' hidden>
                    <div class='button'>
                        <a href = 'javascript:void(0)' class='btn btn-reply'><i class='fa fa-reply' aria-hidden='true'></i>Trả lời</a>
                    </div>
                </div>
            </div>";
                //============Phần load sub comment============
                List<object> subCommentList = context.getSubCommentForProductDetail((int)item.GetType().GetProperty("CommentId").GetValue(item, null));
                foreach(var subItem in subCommentList)
                {
                    created_at = ((DateTime)subItem.GetType().GetProperty("CreatedAt").GetValue(subItem, null)).ToString("HH:mm dd/MM/yyyy"); 
                    userImage = subItem.GetType().GetProperty("UserImage").GetValue(subItem, null).ToString();
                    if (string.IsNullOrEmpty(userImage))
                    {
                        userImage = "default-user-icon.png";
                    }
                    output += $@"<div class='single-comment left'>
                    <img src = '/public/images_upload/user/{userImage}' alt='#'>
                    <div class='content'><input type = 'text' class='CommentId' value='{subItem.GetType().GetProperty("CommentId").GetValue(subItem, null)}' hidden>";

                    if (userCommentId == customerId || userCommentId == adminId)
                    {
                        output +=@"<div class='button' style='float:right'>
                                        <a href = 'javascript:void(0)' class='btn btn-xoa-comment'><i class='fa fa-times' aria-hidden='true'></i></a>
                                    </div>";
                    }    
                    output += $"<h4>{subItem.GetType().GetProperty("LastName").GetValue(subItem, null)} {subItem.GetType().GetProperty("FirstName").GetValue(subItem, null)}";
                    if ((int)subItem.GetType().GetProperty("Admin").GetValue(subItem, null) == 1)
                    {
                        output += "<span><i> Nhân viên ITGoShop</i></span>";
                    }
                    output += @$"<span>{created_at}</span></h4>
                        <p>{subItem.GetType().GetProperty("CommentContent").GetValue(subItem, null)}</p>
                          <div class='button'>
                            <a href = 'javascript:void(0)' class='btn btn-reply'><i class='fa fa-reply' aria-hidden='true'></i>Trả lời</a>
                        </div>
                    </div>
                </div>";
                }    
                output += "</div>";
            }
            return output;
        }
        public void send_comment(int ProductId, string CommentContent, int ParentComment)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            int adminId = Convert.ToInt32(HttpContext.Session.GetInt32("adminId"));
            if (customerId != 0 || adminId != 0)
            {
                Comment comment = new Comment();
                comment.CommentContent = CommentContent;
                comment.ProductId = ProductId;
                
                comment.CreatedAt = DateTime.Now;
                comment.UpdatedAt = DateTime.Now;
                if(customerId != 0)
                {
                    comment.UserId = customerId;
                }  
                else
                {
                    comment.UserId = adminId;
                    if (ParentComment != 0)
                    {
                        linqContext.updateCommentStatus(ParentComment);
                    }
                }
                comment.CommentStatus = 1;
                if (ParentComment != 0)
                    comment.ParentComment = ParentComment;
                else
                    comment.ParentComment = null;
                linqContext.addComment(comment);
            }   
        }
        public void delete_comment(int comment_id)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.deleteComment(comment_id);
        }
    }
}
