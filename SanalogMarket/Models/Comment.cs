using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace SanalogMarket.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CommentTime { get; set; }

        public virtual User User { get; set; }

        //public virtual Product Product;{get;set;}
    }
}