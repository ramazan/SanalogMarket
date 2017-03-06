using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models.Theme
{
    public class ProductTheme
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

        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string CompatibleBrowsers { get; set; }
        public string FilesIncluded { get; set; }
        public string CompatibleWith { get; set; }
        public int IsValid { get; set; }
        public Boolean Support { get; set; }

        [DisplayName("Creating Date")]
        public DateTime CreateDate { get; set; }

       [DataType(DataType.DateTime)]
        public DateTime UpdateDate { get; set; }

        public virtual User User { get; set; }

        public string imza { get; set; }

        public string Tags { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Review> Reviews { get; set; }

        public string Comment { get; set; }
        public string Columns { get; set; }
        public string Layout { get; set; }
        public string DemoURL { get; set; }
        public string HighResolution { get; set; }
        public string RejectMessage { get; set; }
        public virtual Admin LastProcessAdmin { get; set; }
    }
}