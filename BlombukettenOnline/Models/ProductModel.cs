using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnline.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ColorModel Color { get; set; }
        public ICollection<CategoryModel> Categories { get; set; }
    }
}