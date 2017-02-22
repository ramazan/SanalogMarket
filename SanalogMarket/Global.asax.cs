using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SanalogMarket.Models;
using SanalogMarket.Models.Theme;

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

                int ThemecategoryCount = db.ThemeCategories.Count();
                int ThemeSubcayegorycount = db.ThemeSubCategories.Count();

                ThemeCategory thcat = new ThemeCategory
                {
                    Name = "Site Templates",
                    Description = "Site Templates"
                };
                ThemeCategory thcat1 = new ThemeCategory
                {
                    Name = "CMS Themes",
                    Description = "CMS Themes"
                };
                ThemeCategory thcat2 = new ThemeCategory
                {
                    Name = "Wordpress",
                    Description = "Wordpress"
                };
                ThemeSubCategory tsub = new ThemeSubCategory
                {
                    Name = "Photography",
                    Description = "Photography",
                    Category=thcat
                };

                ThemeSubCategory tsub1 = new ThemeSubCategory
                {
                    Name = "Education",
                    Description = "Education",
                    Category = thcat
                };
                ThemeSubCategory tsub2 = new ThemeSubCategory
                {
                    Name = "Deneme",
                    Description = "Deneme",
                    Category = thcat1
                };
                ThemeSubCategory tsub3 = new ThemeSubCategory
                {
                    Name = "Deneme2",
                    Description = "Deneme2",
                    Category = thcat1
                };
                ThemeSubCategory tsub4 = new ThemeSubCategory
                {
                    Name = "Camera",
                    Description = "Camera",
                    Category = thcat2
                };
                ThemeSubCategory tsub5 = new ThemeSubCategory
                {
                    Name = "Computer",
                    Description = "Computer",
                    Category = thcat2
                };


                Category cat = new Category
                {
                    Name = "Site Templates",
                    Description = "Site Templates"
                };

                Category cat1 = new Category
                {
                    Name = "CMS Themes",
                    Description = "CMS Themes"
                };

                Category cat2 = new Category
                {
                    Name = "Wordpress",
                    Description = "Wordpress"
                };

                Category cat3 = new Category
                {
                    Name = "eCommerce",
                    Description = "eCommerce"
                };

                SubCategory sub = new SubCategory()
                {
                    Name = "Creative",
                    Description = "Creative",
                    Category = cat
                };

                SubCategory scat1 = new SubCategory
                {
                    Name = "Portfolio",
                    Description = "Portfolio",
                    Category = cat
                };

                SubCategory scat2 = new SubCategory
                {
                    Name = "Photography",
                    Description = "Photography",
                    Category = cat
                };


                SubCategory sub1 = new SubCategory()
                {
                    Name = "Art",
                    Description = "Art",
                    Category = cat
                };

                SubCategory sub12 = new SubCategory()
                {
                    Name = "Business",
                    Description = "Business",
                    Category = cat
                };

                SubCategory sub13 = new SubCategory()
                {
                    Name = "Marketing",
                    Description = "Marketing",
                    Category = cat
                };

                SubCategory sub14 = new SubCategory()
                {
                    Name = "Food",
                    Description = "Food",
                    Category = cat
                };

                SubCategory sub15 = new SubCategory()
                {
                    Name = "Children",
                    Description = "Children",
                    Category = cat
                };

                SubCategory cat12 = new SubCategory
                {
                    Name = "Blog",
                    Description = "Blog",
                    Category = cat1
                };

                SubCategory cat23 = new SubCategory
                {
                    Name = "Personal",
                    Description = "Personal",
                    Category = cat1
                };

                SubCategory sub16 = new SubCategory()
                {
                    Name = "Creative",
                    Description = "Creative",
                    Category = cat1
                };

                SubCategory cat16 = new SubCategory
                {
                    Name = "Portfolio",
                    Description = "Portfolio",
                    Category = cat1
                };
                SubCategory sub17 = new SubCategory()
                {
                    Name = "Food",
                    Description = "Food",
                    Category = cat1
                };

                SubCategory cat123 = new SubCategory
                {
                    Name = "Webflow",
                    Description = "Webflow",
                    Category = cat2
                };

                SubCategory scat3 = new SubCategory
                {
                    Name = "Webly",
                    Description = "Joomla",
                    Category = cat2
                };
                SubCategory cat4 = new SubCategory
                {
                    Name = "Drupal",
                    Description = "Drupal",
                    Category = cat2
                };

                SubCategory cat5 = new SubCategory
                {
                    Name = "Sanalog",
                    Description = "Sanalog",
                    Category = cat2
                };

                SubCategory cat6 = new SubCategory
                {
                    Name = "Magento",
                    Description = "Magento",
                    Category = cat3
                };
                SubCategory cat7 = new SubCategory
                {
                    Name = "Shopify",
                    Description = "Shopify",
                    Category = cat3
                };

                SubCategory cat8 = new SubCategory
                {
                    Name = "CS-Cart",
                    Description = "CS-Cart",
                    Category = cat3
                };

                
                if (SubCategoryCount <= 0)
                {
                    db.SubCategories.Add(sub);
                    db.SubCategories.Add(scat1);
                    db.SubCategories.Add(scat2);
                    db.SubCategories.Add(sub1);
                    db.SubCategories.Add(sub12);
                    db.SubCategories.Add(sub13);
                    db.SubCategories.Add(sub14);
                    db.SubCategories.Add(sub15);
                    db.SubCategories.Add(cat12);
                    db.SubCategories.Add(cat23);
                    db.SubCategories.Add(cat5);
                    db.SubCategories.Add(cat4);
                    db.SubCategories.Add(scat3);
                    db.SubCategories.Add(sub16);
                    db.SubCategories.Add(sub17);
                    db.SubCategories.Add(cat16);
                    db.SubCategories.Add(cat123);
                    db.SubCategories.Add(cat6);
                    db.SubCategories.Add(cat7);
                    db.SubCategories.Add(cat8);

                    if (categoryCount <= 0)
                    {
                        db.Categories.Add(cat);
                        db.Categories.Add(cat2);
                        db.Categories.Add(cat1);
                        db.Categories.Add(cat3);

                        db.SaveChanges();
                    }

                    if (ThemecategoryCount <= 0)
                    {
                        db.ThemeSubCategories.Add(tsub);
                        db.ThemeSubCategories.Add(tsub1);
                        db.ThemeSubCategories.Add(tsub2);
                        db.ThemeSubCategories.Add(tsub3);
                        db.ThemeSubCategories.Add(tsub4);
                        db.ThemeSubCategories.Add(tsub5);
                    }
                    if (ThemecategoryCount <= 0)
                        {
                            db.ThemeCategories.Add(thcat);
                            db.ThemeCategories.Add(thcat1);
                            db.ThemeCategories.Add(thcat2);
                         

                            db.SaveChanges();
                        }

                        User usr = new User
                    {
                        Name = "admin",
                        Surname = "admin",
                        Username = "admin",
                        Email = "admin@sanalog.org",
                        Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                        RegisterTime = DateTime.Now,
                        LastLoginTime = new DateTime(1953, 1, 1),
                        Avatar = ""
                    };


                    Admin admin = new Admin()
                    {
                        Name = "admin",
                        Surname = "admin",
                        Username = "admin",
                        Email = "admin@sanalog.org",
                        Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                        RegisterTime = DateTime.Now,
                        LastLoginTime = new DateTime(1953, 1, 1),
                        Role = "Admin"
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