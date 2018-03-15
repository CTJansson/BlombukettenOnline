using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineIntranet.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [DisplayName("Namn")]
        public string Name { get; set; }

        [DisplayName("Huvudkategori")]
        public int? ParentId { get; set; }
    }
}