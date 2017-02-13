using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class ProductCode
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Project Title")]
        [Required(ErrorMessage = "Proje Başlığı Boş bırakılamaz")]
        public string Title { get; set; }

        public string Icon { get; set; }
        public string Screenshot { get; set; }
        public string Filepath { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Product Price")]
        public int Price { get; set; }

        public virtual int Category { get; set; }
        public virtual int SubCategory { get; set; }

        public int IsValid { get; set; }

        public virtual User User { get; set; }
    }

    public class ProductCodeCategory
    {
        public ProductCode Code { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }

    }
}