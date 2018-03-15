using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineIntranet.Models
{
    public class CreateProductModel
    {
        [DisplayName("Namn")]
        public string Name { get; set; }

        [DisplayName("Beskrivning")]
        public string Description { get; set; }

        [DisplayName("Färg")]
        public int ColorId { get; set; }

        [DisplayName("Kategory")]
        public int CategoryId { get; set; }

        [DisplayName("Pris")]
        public decimal? Price { get; set; }

        public bool? Active { get; set; }

        [DisplayName("Bild")]
        public string Image { get; set; }

        [DisplayName("Färg")]
        public ColorViewModel Color { get; set; }
        [DisplayName("Kategory")]
        public CategoryViewModel Category { get; set; }

    }
}