using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductGallary
    {
        private int gallaryId;
        private string gallaryImage;
        private int productId, userId, gallaryStatus;
        private DateTime createdAt, updatedAt;

        public ProductGallary()
        {
        }

        public ProductGallary(int gallaryId, string gallaryImage, int productId, int userId, int gallaryStatus, DateTime createdAt, DateTime updatedAt)
        {
            this.gallaryId = gallaryId;
            this.gallaryImage = gallaryImage;
            this.productId = productId;
            this.userId = userId;
            this.gallaryStatus = gallaryStatus;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
        }
        [Key]
        public int GallaryId { get => gallaryId; set => gallaryId = value; }
        public string GallaryImage { get => gallaryImage; set => gallaryImage = value; }
        public int ProductId { get => productId; set => productId = value; }
        public int UserId { get => userId; set => userId = value; }
        public int GallaryStatus { get => gallaryStatus; set => gallaryStatus = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }
    }
}
