using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineAPI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ColorId { get; set; }
        public ColorModel Color { get; set; }
        [Required]
        public CategoryModel Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public bool? Active { get; set; }

        public string Image { get; set; }
    }
}