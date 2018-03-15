using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineIntranet.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsDelivered { get; set; }

        public DeliveryModel Delivery { get; set; }
        public PaymentModel Payment { get; set; }
        public StoreModel Store { get; set; }
        public AddressModel Address { get; set; }
        public EmailModel Email { get; set; }

        public ICollection<BouqetModel> Bouquet { get; set; }
    }
}