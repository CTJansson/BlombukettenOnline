using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineAPI.Models
{
    public class ProductBouqetModel
    {
        public int ProductId { get; set; }
        public int BoquettId { get; set; }
        public int Quantity { get; set; }

        public BoquetteModel Boquett { get; set; }
        public ProductModel Product { get; set; }
    }
}