using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SanalogMarket.Models
{
    public class Review
    {
        [Key]
        public int id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReviewDate { get; set; }

        public virtual User ReviewAutor { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string ReviewDescription { get; set; }

        public double ReviewRate { get; set; }

        public virtual ProductCode ReviewCode { get; set; }

       // public virtual ProductTheme ReviewTheme { get; set; }
    }
}