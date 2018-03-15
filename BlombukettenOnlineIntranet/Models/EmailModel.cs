using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineIntranet.Models
{
    public class EmailModel
    {
        public int Id { get; set; }
        public string Email1 { get; set; }
        public bool Subscribe { get; set; }

        public ICollection<OrderModel> Orders { get; set; }
    }
}