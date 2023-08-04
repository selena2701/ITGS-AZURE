using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Models
{
    public class Product
    {
        private int productId;
        private string productName;
        private int? brandId;
        private string categoryId; 
        private string subBrandId;
        private float discount;
        private int quantity;
        private long price;
        private long cost;
        private int status;
        private string productImage;
        private int sold;
        private string content;
        private int? sliderId;
        private int view;
        private DateTime? startsAt;
        private DateTime? endsAt;
        private DateTime? createdAt;
        private DateTime? updatedAt;

        public Product()
        {
        }

        public Product(int productId, string productName, int brandId, string categoryId, float discount, int quantity, long price, long cost, int status, string productImage, int sold, string content, int slideId, int view, DateTime startsAt, DateTime endsAt, DateTime createdAt, DateTime updatedAt)
        {
            this.productId = productId;
            this.productName = productName;
            this.brandId = brandId;
            this.categoryId = categoryId;
            this.discount = discount;
            this.quantity = quantity;
            this.price = price;
            this.cost = cost;
            this.status = status;
            this.productImage = productImage;
            this.sold = sold;
            this.content = content;
            this.sliderId = slideId;
            this.view = view;
            this.startsAt = startsAt;
            this.endsAt = endsAt;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
        }

        public int ProductId { get => productId; set => productId = value; }
        public string ProductName { get => productName; set => productName = value; }
        public int? BrandId { get => brandId; set => brandId = value; }
        public string CategoryId { get => categoryId; set => categoryId = value; }
        public float Discount { get => discount; set => discount = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public long Price { get => price; set => price = value; }
        public long Cost { get => cost; set => cost = value; }
        public int Status { get => status; set => status = value; }
        public string ProductImage { get => productImage; set => productImage = value; }
        public int Sold { get => sold; set => sold = value; }
        public string Content { get => content; set => content = value; }
        public int? SliderId { get => sliderId; set => sliderId = value; }
        public int View { get => view; set => view = value; }
        public DateTime? StartsAt { get => startsAt; set => startsAt = value; }
        public DateTime? EndsAt { get => endsAt; set => endsAt = value; }
        public DateTime? CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime? UpdatedAt { get => updatedAt; set => updatedAt = value; }
        public string SubBrandId { get => subBrandId; set => subBrandId = value; }
    }
}
