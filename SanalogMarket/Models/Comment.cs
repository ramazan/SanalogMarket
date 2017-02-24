using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;
using SanalogMarket.Models.Theme;

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

        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual User User { get; set; }

        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual ProductCode Product {get;set;}

        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual ProductTheme ThemeProduct{ get; set; }
    }
}