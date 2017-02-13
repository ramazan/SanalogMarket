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
        DbBaglantisi db = new DbBaglantisi();

        protected void Application_Start()
        {
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
                    Description = "PHP",Category_ID = 1
                };

                SubCategory cat2 = new SubCategory
                {
                    Name = "JSP",
                    Description = "JSP",Category_ID = 1
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
                db.SaveChanges();
            }

            

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}