using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class Order
    {
        private int orderId, userId;
        private long total;
        private int shippingAddressId, shipFee;
        private DateTime estimatedDeliveryTime;
        private string shipMethod;
        private DateTime orderDate;
        private string orderStatus;
        private string paymentMethod;
        private string paymentStatus;
        private string description;
        private DateTime? dateUpdate;

        public Order()
        {
        }

        public Order(int orderId, int userId, long total, int shippingAddressId, int shipFee, DateTime estimatedDeliveryTime, string shipMethod, DateTime orderDate,  string orderStatus, string paymentMethod, string paymentStatus, string description, DateTime dateUpdate)
        {
            this.orderId = orderId;
            this.userId = userId;
            this.total = total;
            this.shippingAddressId = shippingAddressId;
            this.shipFee = shipFee;
            this.estimatedDeliveryTime = estimatedDeliveryTime;
            this.shipMethod = shipMethod;
            this.orderDate = orderDate;
            this.orderStatus = orderStatus;
            this.paymentMethod = paymentMethod;
            this.paymentStatus = paymentStatus;
            this.description = description;
            this.dateUpdate = dateUpdate;
        }

        public int OrderId { get => orderId; set => orderId = value; }
        public int UserId { get => userId; set => userId = value; }
        public long Total { get => total; set => total = value; }
        public int ShippingAddressId { get => shippingAddressId; set => shippingAddressId = value; }
        public int ShipFee { get => shipFee; set => shipFee = value; }
        public DateTime EstimatedDeliveryTime { get => estimatedDeliveryTime; set => estimatedDeliveryTime = value; }
        public string ShipMethod { get => shipMethod; set => shipMethod = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
        public string OrderStatus { get => orderStatus; set => orderStatus = value; }
        public string PaymentMethod { get => paymentMethod; set => paymentMethod = value; }
        public string PaymentStatus { get => paymentStatus; set => paymentStatus = value; }
        public string Description { get => description; set => description = value; }
        public DateTime? DateUpdate { get => dateUpdate; set => dateUpdate = value; }
    }
}
