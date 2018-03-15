using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineIntranet.Models
{
    public class BouqetModel
    {

        public int Id { get; set; }
        public decimal Sum { get; set; }
        public int OrderId { get; set; }

        public OrderModel Order { get; set; }
        public ICollection<ProductBouquetModel> ProductBoquetts { get; set; }
    }
}