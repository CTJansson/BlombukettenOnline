//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlombukettenOnlineAPI.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeliveryMethod
    {
        public DeliveryMethod()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
        public string Method { get; set; }
    
        public virtual ICollection<Order> Orders { get; set; }
    }
}
