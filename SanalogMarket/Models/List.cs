using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class List
    {
        [Key]
        public int ID { get; set; }
        public String uzanti { get; set; }
    }
}