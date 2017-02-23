using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class Dosyalar 
    {

        public IEnumerable<HttpPostedFileBase> files { get; set; }
       
    }
}