using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineAPI.Models
{
    public class CreateProductModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public int ColorId { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public bool Active { get; set; }

    }
}