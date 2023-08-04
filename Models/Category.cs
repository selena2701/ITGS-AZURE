using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class Category
    {
        private string categoryId;
        private string categoryName;
        private string categoryImage;
        private int status;

        public Category()
        {
        }

        public Category(string categoryId, string categoryName, string categoryImage, int status)
        {
            this.categoryId = categoryId;
            this.categoryName = categoryName;
            this.categoryImage = categoryImage;
            this.status = status;
        }

        public string CategoryId { get => categoryId; set => categoryId = value; }
        public string CategoryName { get => categoryName; set => categoryName = value; }
        public string CategoryImage { get => categoryImage; set => categoryImage = value; }
        public int Status { get => status; set => status = value; }
    }
}
