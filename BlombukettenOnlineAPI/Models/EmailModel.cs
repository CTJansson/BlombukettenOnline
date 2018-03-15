using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlombukettenOnlineAPI.Models
{
    public class EmailModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Subscribe { get; set; }
    }
}