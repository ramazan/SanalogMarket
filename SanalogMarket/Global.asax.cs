using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SanalogMarket.Models;

namespace SanalogMarket
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            using (DbBaglantisi db = new DbBaglantisi())
            {
                db.Database.CreateIfNotExists();

                int categoryCount = db.Categories.Count();
                int SubCategoryCount = db.SubCategories.Count();

                if (categoryCount <= 0)
                {
                    Category cat = new Category
                    {
                        Name = "Web",
                        Description = "Web"
                    };

                    Category cat1 = new Category
                    {
                        Name = "Mobile",
                        Description = "Mobile"
                    };

                    Category cat2 = new Category
                    {
                        Name = "Wordpress",
                        Description = "Wordpress"
                    };

                    db.Categories.Add(cat);
                    db.Categories.Add(cat1);
                    db.Categories.Add(cat2);
                    db.SaveChanges();
                }

                if (SubCategoryCount <= 0)
                {
                    SubCategory sub = new SubCategory()
                    {
                        Name = "Asp",
                        Description = "Asp",
                        Category_ID = 1
                    };

                    SubCategory cat1 = new SubCategory
                    {
                        Name = "PHP",
                        Description = "PHP",
                        Category_ID = 1
                    };

                    SubCategory cat2 = new SubCategory
                    {
                        Name = "JSP",
                        Description = "JSP",
                        Category_ID = 1
                    };


                    SubCategory sub1 = new SubCategory()
                    {
                        Name = "Android",
                        Description = "Android",
                        Category_ID = 2
                    };

                    SubCategory cat12 = new SubCategory
                    {
                        Name = "IOS",
                        Description = "IOS",
                        Category_ID = 2
                    };

                    SubCategory cat23 = new SubCategory
                    {
                        Name = "Windows",
                        Description = "Windows",
                        Category_ID = 2
                    };


                    SubCategory cat123 = new SubCategory
                    {
                        Name = "Theme",
                        Description = "Theme",
                        Category_ID = 3
                    };

                    SubCategory cat223 = new SubCategory
                    {
                        Name = "Code",
                        Description = "Code",
                        Category_ID = 3
                    };


                    db.SubCategories.Add(sub);
                    db.SubCategories.Add(cat1);
                    db.SubCategories.Add(cat2);
                    db.SubCategories.Add(sub1);
                    db.SubCategories.Add(cat12);
                    db.SubCategories.Add(cat23);
                    db.SubCategories.Add(cat223);
                    db.SubCategories.Add(cat123);

                    User usr = new User
                    {
                        Name = "admin",
                        Surname = "admin",
                        Username = "admin",
                        Email = "admin@sanalog.org",
                        Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                        RegisterTime = DateTime.Now,
                        LastLoginTime = new DateTime(1953, 1, 1),Avatar = ""
                    };


                    Admin admin = new Admin()
                    {
                        Name = "admin",
                        Surname = "admin",
                        Username = "admin",
                        Email = "admin@sanalog.org",
                        Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                        RegisterTime = DateTime.Now,
                        LastLoginTime = new DateTime(1953, 1, 1),Role = "Admin"
                    };

                    db.Admins.Add(admin);
                    db.Users.Add(usr);
                    db.SaveChanges();
                }
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}