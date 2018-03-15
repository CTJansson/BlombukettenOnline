//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlombukettenOnline.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.Boquetts = new HashSet<Boquett>();
            this.Categories = new HashSet<Category>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ColorId { get; set; }
    
        public virtual Color Color { get; set; }
        public virtual ICollection<Boquett> Boquetts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
