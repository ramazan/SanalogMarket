using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class Extension
    {
        [Key]
        public int ID { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public string uzanti { get; set; }
    }
}