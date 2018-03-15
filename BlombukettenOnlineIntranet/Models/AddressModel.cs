using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineIntranet.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public ICollection<OrderModel> Orders { get; set; }
    }
}