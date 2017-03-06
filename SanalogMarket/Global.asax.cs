﻿using System;
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
                    Category = thcat
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


                    User usr = new User
                    {
                        Name = "İsmail Reşat",
                        Surname = "Akcan",
                        Username = "admin",
                        Email = "admin@sanalog.org",
                        Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                        RegisterTime = DateTime.Now,
                        LastLoginTime = new DateTime(1953, 1, 1),
                        Avatar = "/Project_Icon/user.png"
                    };


                    Admin admin = new Admin()
                    {
                        Name = "İsmail Reşat",
                        Surname = "Akcan",
                        Username = "admin",
                        Email = "admin@sanalog.org",
                        Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                        RegisterTime = DateTime.Now,
                        LastLoginTime = new DateTime(1953, 1, 1),
                        Role = "Admin",
                        Avatar = "/Project_Icon/user.png"
                    };

                    Admin editor = new Admin()
                    {
                        Name = "Ramazan",
                        Surname = "Demir",
                        Username = "editor",
                        Email = "editor@sanalog.org",
                        Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                        RegisterTime = DateTime.Now,
                        LastLoginTime = new DateTime(1953, 1, 1),
                        Role = "Editor",
                        Avatar = "/Project_Icon/user.png"
                    };


                    ProductCode productCode = new ProductCode
                    {
                        Title = "Php plugin",
                        Screenshot = "/Project_Icon/350x260.png",
                        Description = "This field is used for search",
                        Price = 13,
                        SalesPrice = 45,
                        CreateDate = DateTime.Now,
                        HighResolution = "yes",
                        imza = "True",
                        SoftwarVersion = "AngularJs,ReactJS",
                        FilesIncluded = ".html,.js",
                        Browsers = "Firefox,Safari",
                        Category = "Site Templates",
                        SubCategory = "Creative",
                        IsValid = 1,
                        Tags = "Maximum of 15 key",
                        User = usr
                    };

                    ProductCode productCode2 = new ProductCode
                    {
                        Title = "Sanalog plugin",
                        Screenshot = "/Project_Icon/350x260.png",
                        Description = "This field is used for search",
                        Price = 57,
                        SalesPrice = 45,
                        CreateDate = DateTime.Now,
                        HighResolution = "yes",
                        imza = "True",
                        SoftwarVersion = "AngularJs,Jquery",
                        FilesIncluded = ".html,.js",
                        Browsers = "Firefox,Safari",
                        Category = "CMS",
                        SubCategory = "Creative",
                        IsValid = 1,
                        Tags = "Maximum of 15 key",
                        User = usr
                    };


                    ProductCode productCode3 = new ProductCode
                    {
                        Title = "Android App",
                        Screenshot = "/Project_Icon/350x260.png",
                        Description = "This field is used for search",
                        Price = 10,
                        SalesPrice = 45,
                        CreateDate = DateTime.Now,
                        HighResolution = "yes",
                        imza = "True",
                        SoftwarVersion = "AngularJs,Jquery",
                        FilesIncluded = ".html,.js",
                        Browsers = "Firefox,Safari",
                        Category = "Mobile",
                        SubCategory = "Android",
                        IsValid = 1,
                        Tags = "Maximum of 15 key",
                        User = usr
                    };


                    ProductTheme productTheme = new ProductTheme
                    {
                        Title = "Bootstrap Theme",
                        //                        Icon = "/Project_Icon/350x260.png",
                        Screenshot = "/Project_Icon/350x260.png",
                        //                        Filepath = "/Project_Icon/350x260.png",
                        Description = "This search",
                        Price = 13,
                        Category = "Site Templates",
                        SubCategory = "Creative",
                        //                        CompatibleBrowsers = "Firefox,Safari",
                        //                        FilesIncluded = ".html,.js",
                        IsValid = 1,
                        Support = true,
                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        imza = "True",
                        Tags = "Maximum key",
                        HighResolution = "yes",
                        CompatibleWith = "AngularJs",
                        User = usr,
                    };

                    ProductTheme productTheme2 = new ProductTheme
                    {
                        Title = "Bootstrap Login Template",
                        Screenshot = "/Project_Icon/350x260.png",
                        Description = "This for search",
                        Price = 13,
                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        HighResolution = "yes",
                        imza = "True",
                        CompatibleWith = "AngularJs,ReactJS",
                        FilesIncluded = ".html,.js",
                        CompatibleBrowsers = "Firefox,Safari",
                        Category = "Site Templates",
                        SubCategory = "Creative",
                        IsValid = 1,
                        Tags = "Maximum of 15 key",
                        User = usr,
                        Support = true
                    };


                    db.Admins.Add(admin);
                    db.Admins.Add(editor);
                    db.Users.Add(usr);
                    db.Codes.Add(productCode);
                    db.Codes.Add(productCode2);
                    db.Codes.Add(productCode3);
                    db.Themes.Add(productTheme);
                    db.Themes.Add(productTheme2);
                    db.SaveChanges();



                    Comment cm = new Comment();
                    cm.User = usr;
                    cm.CommentTime = DateTime.Now;
                    cm.Content = "Amazing Product!";
                    cm.Product = productCode;
                    cm.ThemeProduct = null;

                    Comment cm1 = new Comment();
                    cm1.User = usr;
                    cm1.CommentTime = DateTime.Now;
                    cm1.Content = "Great Product for all of us!";
                    cm1.Product = productCode2;
                    cm1.ThemeProduct = null;

                    Comment cm2 = new Comment();
                    cm2.User = usr;
                    cm2.CommentTime = DateTime.Now;
                    cm2.Content = "Amazing Product!";
                    cm2.Product = productCode3;
                    cm2.ThemeProduct = null;


                    Comment cm3 = new Comment();
                    cm3.User = usr;
                    cm3.CommentTime = DateTime.Now;
                    cm3.Content = "Amazing Product!";
                    cm3.ThemeProduct = productTheme;
                    cm3.Product = null;

                    Comment cm4 = new Comment();
                    cm4.User = usr;
                    cm4.CommentTime = DateTime.Now;
                    cm4.Content = "Amazing Product!";
                    cm4.ThemeProduct = productTheme2;
                    cm4.Product = null;


                    db.Comments.Add(cm);
                    db.Comments.Add(cm1);
                    db.Comments.Add(cm2);
                    db.Comments.Add(cm3);
                    db.Comments.Add(cm4);





                    Review rv = new Review();
                    rv.ReviewAutor = usr;
                    rv.ReviewDate = DateTime.Now;
                    rv.ReviewDescription = "Amazing Product!";
                    rv.ReviewCode = productCode;
                    rv.ReviewTheme= null;

                    Review rv1 = new Review();
                    rv1.ReviewAutor = usr;
                    rv1.ReviewDate = DateTime.Now;
                    rv1.ReviewDescription = "Amazing Product!";
                    rv1.ReviewCode = productCode2;
                    rv1.ReviewTheme = null;

                    Review rv2 = new Review();
                    rv2.ReviewAutor = usr;
                    rv2.ReviewDate = DateTime.Now;
                    rv2.ReviewDescription = "Amazing Product!";
                    rv2.ReviewCode = productCode3;
                    rv2.ReviewTheme = null;

                    Review rv3 = new Review();
                    rv3.ReviewAutor = usr;
                    rv3.ReviewDate = DateTime.Now;
                    rv3.ReviewDescription = "Amazing Product!";
                    rv3.ReviewCode = null;
                    rv3.ReviewTheme = productTheme;

                    Review rv4 = new Review();
                    rv4.ReviewAutor = usr;
                    rv4.ReviewDate = DateTime.Now;
                    rv4.ReviewDescription = "Amazing Product!";
                    rv4.ReviewCode = null;
                    rv4.ReviewTheme = productTheme2;


                    db.Reviews.Add(rv);
                    db.Reviews.Add(rv1);
                    db.Reviews.Add(rv2);
                    db.Reviews.Add(rv3);
                    db.Reviews.Add(rv4);

                }

                if (ThemecategoryCount <= 0)
                {
                    db.ThemeSubCategories.Add(tsub);
                    db.ThemeSubCategories.Add(tsub1);
                    db.ThemeSubCategories.Add(tsub2);
                    db.ThemeSubCategories.Add(tsub3);
                    db.ThemeSubCategories.Add(tsub4);
                    db.ThemeSubCategories.Add(tsub5);
                    // db.SaveChanges();

                    if (ThemecategoryCount <= 0)
                    {
                        db.ThemeCategories.Add(thcat);
                        db.ThemeCategories.Add(thcat1);
                        db.ThemeCategories.Add(thcat2);


                        db.SaveChanges();
                    }
                }
            }
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}