using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class SubBrand
    {
        private string subBrandId;
        private string subBrandName;
        private int brandId, status;

        public SubBrand()
        {
        }

        public SubBrand(string subBrandId, string subBrandName, int brandId, int status)
        {
            this.subBrandId = subBrandId;
            this.subBrandName = subBrandName;
            this.brandId = brandId;
            this.status = status;
        }

        public string SubBrandId { get => subBrandId; set => subBrandId = value; }
        public string SubBrandName { get => subBrandName; set => subBrandName = value; }
        public int BrandId { get => brandId; set => brandId = value; }
        public int Status { get => status; set => status = value; }
    }
}
