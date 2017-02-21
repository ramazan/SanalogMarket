using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class ProductCode
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Proje Başlığı Boş bırakılamaz")]
        public string Title { get; set; }
        [Display(Name = "Thumbnail")]
        public string Icon { get; set; }
        [Display(Name = "Screenshots")]
        public string Screenshot { get; set; }
        [Display(Name = "Main File(s)")]
        public string Filepath { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Product Price")]
        public int Price { get; set; }
        public int SalesPrice { get; set; }
        [DisplayName("Creating Date")]
        public DateTime CreateDate { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }

        [DisplayName("Yayında mı")]
        public int IsValid { get; set; }

        public string Comment { get; set; } 
        public virtual List<Comment> Comments { get; set; }
        public string Tags { get; set; }
        public virtual User User { get; set; }
    }

  }