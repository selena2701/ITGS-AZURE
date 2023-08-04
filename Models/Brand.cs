using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class Brand
    {
        private int brandId;
        private string brandName, description;
        private string categoryId;
        private int status;
        private string brandLogo;

        public Brand()
        {
        }

        public Brand(int brandId, string brandName, string description, string categoryId, int status, string brandLogo)
        {
            this.brandId = brandId;
            this.brandName = brandName;
            this.description = description;
            this.categoryId = categoryId;
            this.status = status;
            this.brandLogo = brandLogo;
        }

        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandName { get => brandName; set => brandName = value; }
        public string Description { get => description; set => description = value; }
        public string CategoryId { get => categoryId; set => categoryId = value; }
        public int Status { get => status; set => status = value; }
        public string BrandLogo { get => brandLogo; set => brandLogo = value; }
    }
}
