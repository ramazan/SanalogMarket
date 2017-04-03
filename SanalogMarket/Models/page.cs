using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class Page
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastChangeTime { get; set; }

//        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual User User { get; set; }
    }
}