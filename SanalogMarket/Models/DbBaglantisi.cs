using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SanalogMarket.Models
{
    public class DbBaglantisi : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ProductCode> Codes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserConfirm> UserConfirms { get; set; }
        public DbSet<Extension> Extensions { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Page> Pages { get; set; }
    }
}