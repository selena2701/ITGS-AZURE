using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class OrderDetail
    {
        private int orderId, productId, orderQuantity;
        private long unitPrice;

        public OrderDetail()
        {
        }

        public OrderDetail(int orderId, int productId, int orderQuantity, long unitPrice)
        {
            this.orderId = orderId;
            this.productId = productId;
            this.orderQuantity = orderQuantity;
            this.unitPrice = unitPrice;
        }
        public int OrderId { get => orderId; set => orderId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public int OrderQuantity { get => orderQuantity; set => orderQuantity = value; }
        public long UnitPrice { get => unitPrice; set => unitPrice = value; }
    }
}
