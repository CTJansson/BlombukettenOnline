using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineIntranet.Models
{
    public class ColorViewModel
    {
        public int Id { get; set; }

        [DisplayName("Namn")]
        public string Name { get; set; }
    }
}