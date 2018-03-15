using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineAPI.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Comment { get; set; }
        public bool IsDelivered { get; set; }
        public int PaymentMethod { get; set; }
        public int DeliveryMethod { get; set; }
        public int StoreId { get; set; }

        public PaymentModel Payment { get; set; }
        public DeliveryModel Delivery { get; set; }
        public StoreModel Store { get; set; }
        public AddressModel Address { get; set; }
        public ICollection<BoquetteModel> Buquettes { get; set; }
        public EmailModel Email { get; set; }

    }
}