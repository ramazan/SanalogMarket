﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanalogMarket.Models
{
    public class User
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
        [Remote("IsUserNameExist", "User", ErrorMessage = "Username already exist")]
        public string Username { get; set; }

        [EmailAddress]
        [DisplayName("E-mail")]
        [Required]
        [Remote("IsEmailExist", "User", ErrorMessage = "E-mail already exist,forget your password?")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegisterTime { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime LastLoginTime { get; set; }

//        public Boolean Active { get; set; }
//        
//        public int Sales { get; set; }
//
//        public int Items { get; set; }
//
//        public int Buy { get; set; }

//      [DataType(DataType.ImageUrl)]
        public string Avatar { get; set; }
    }

    public class UserConfirm
    {
        [Key]
        public int Id { get; set; }

        public virtual User User { get; set; }
        public string Token { get; set; }
    }
}