using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class SubCategory
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Product_kind { get; set; }
        public virtual Category Category{ get; set; }
    }
}