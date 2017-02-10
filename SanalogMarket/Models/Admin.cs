using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("First name")]
        [Required(ErrorMessage = "First name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last name")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Username is required")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [EmailAddress]
        [DisplayName("E-mail")]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegisterTime { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime LastLoginTime { get; set; }

        [Required]
        public string Role { get; set; }
    }
}