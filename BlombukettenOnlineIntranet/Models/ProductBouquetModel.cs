using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineIntranet.Models
{
    public class ProductBouquetModel
    {
        public int ProductId { get; set; }
        public int BoquettId { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}