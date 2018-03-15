using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineAPI.Models
{
    public class BoquetteModel
    {
        public int Id { get; set; }
        public decimal Sum { get; set; }
        public int Quantity { get; set; }

        public OrderModel Order { get; set; }
        public ICollection<ProductModel> Products { get; set; }
    }
}