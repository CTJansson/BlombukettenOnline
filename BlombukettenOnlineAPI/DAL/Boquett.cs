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
    
    public partial class Boquett
    {
        public Boquett()
        {
            this.ProductBoquetts = new HashSet<ProductBoquett>();
        }
    
        public int Id { get; set; }
        public decimal Sum { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual ICollection<ProductBoquett> ProductBoquetts { get; set; }
    }
}
