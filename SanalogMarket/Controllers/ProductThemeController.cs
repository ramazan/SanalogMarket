using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using SanalogMarket.Models;
using SanalogMarket.Models.Theme;
namespace SanalogMarket.Controllers
{
    public class ProductThemeController : Controller
    {
        DbBaglantisi dbBaglantisi = new DbBaglantisi();
        public ProductTheme product;
        public static int gelenID;

    
        public static string compatiblewith;
        public static string fileinculeded;
        public static string browser;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetComments()
        {

            return Json(dbBaglantisi.Comments.Where(p => p.Product.ID == gelenID).ToList(),
                    JsonRequestBehavior.AllowGet);
        }


       
      
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "User");


            var categories = dbBaglantisi.ThemeCategories.ToList();
            ViewBag.DropDownAOOs = new SelectList(categories);

            return View();
        }

        public ActionResult GetCategories()
        {
            return Json(dbBaglantisi.ThemeCategories.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProducts(string catName)
        {
            ThemeCategory category = dbBaglantisi.ThemeCategories.Where(p => p.Name == catName).FirstOrDefault();
            int intCatID = category.ID;
            var products = dbBaglantisi.ThemeSubCategories.ToList().Where(p => p.CategoryID == intCatID);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(ProductTheme gelenCode, HttpPostedFileBase file,
            HttpPostedFileBase fileproject, HttpPostedFileBase fileIcon, string category, string subcategory, Boolean imza, string Gender, Boolean support)
        {
            if (ModelState.IsValid)
            {
                if (file != null)

                {
                    if (file.ContentLength > 10240 * 100)
                    {
                        ModelState.AddModelError("photo", "The size of the file should not 2 MB");
                        ViewBag.FotoError = "The size of the file should not exceed  2 MB";
                        return View();
                    }
                    var supportedTypes = new[] { "jpg", "jpeg", "png" };
                    var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("photo",
                            "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                        return View();
                    }
                    // extract only the filename
                    FileInfo dosya = new FileInfo(file.FileName);
                    // extract only the filename
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Server.MapPath("/ThemeFiles/Screenshots/" + dosya.Name);
                    file.SaveAs(path);
                    var yol = "/ThemeFiles/Screenshots/" + dosya.Name;
                    @ViewBag.Screen = yol;
                    gelenCode.Screenshot = yol;
                }
                if (fileproject != null )
                {
                    FileInfo dosya = new FileInfo(fileproject.FileName);
                    // extract only the filename
                    var fileName = Path.GetFileName(fileproject.FileName);
                    var path = Server.MapPath("/ThemeFiles/Project_File/" + dosya.Name);
                    fileproject.SaveAs(path);
                    var yol = "/ThemeFiles/Project_File/" + dosya.Name;
                    @ViewBag.imageProject = yol;
                    gelenCode.Filepath = yol;
                }
                if (fileIcon != null)
                {
                    FileInfo dosya = new FileInfo(fileIcon.FileName);
                    // extract only the filename
                    var fileName = Path.GetFileName(fileIcon.FileName);
                    var path = Server.MapPath("/ThemeFiles/Project_Icon/" + dosya.Name);
                    fileIcon.SaveAs(path);
                    var yol = "/ThemeFiles/Project_Icon/" + dosya.Name;
                    @ViewBag.imageIcon = yol;
                    gelenCode.Icon = yol;
                }



                gelenCode.Category = category;
                gelenCode.SubCategory = subcategory;
                gelenCode.IsValid = 0;
                gelenCode.Category = category;
                gelenCode.SubCategory = subcategory;
              // gelenCode.HighResolution = Gender;
                gelenCode.imza = imza.ToString();
                gelenCode.CompatibleWith = compatiblewith;
                gelenCode.FilesIncluded = fileinculeded;
                gelenCode.CompatibleBrowsers = browser;
                gelenCode.Support = support;
                int UserID = (int)Session["UserId"];
                User EkleyenUser = dbBaglantisi.Users.Single(u => u.Id == UserID);

                if (EkleyenUser != null)
                {
                    gelenCode.User = EkleyenUser;
                }
                dbBaglantisi.Themes.Add(gelenCode);
                dbBaglantisi.SaveChanges();
            }


            return View();
        }

        public string getCompatibleWith(List<String> values)
        {
            if (values != null)
            {
                compatiblewith = values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    compatiblewith = compatiblewith + "," + values[i];
                }
            }
            else
            {
                compatiblewith = null;
            }
            return compatiblewith;
        }
        public string getBrowser(List<String> values)
        {
            if (values != null)
            {
                browser = values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    browser = browser + "," + values[i];
                }
            }
            else
            {
                browser = null;
            }
            return browser;
        }
        public string getFileInculeded(List<String> values)
        {
            if (values != null)
            {
                fileinculeded = values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    fileinculeded = fileinculeded + "," + values[i];
                }
            }
            else
            {
                fileinculeded = null;
            }
            return fileinculeded;
        }
        public ActionResult Details(int? id)
        {
            if (Session["UserID"] == null)
            {
                ViewBag.Control = "0";
            }
            gelenID = Convert.ToInt32(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product = dbBaglantisi.Themes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
        [HttpPost]
        public ActionResult AddComment(string yorum)
        {
            
            Comment cm = new Comment();
            int id = Convert.ToInt32(Session["UserId"]);
            User EkleyenUser = dbBaglantisi.Users.Where(u => u.Id == id).FirstOrDefault();
            product = dbBaglantisi.Themes.Find(gelenID);
            cm.CommentTime = DateTime.Now;
            cm.User = EkleyenUser;
            cm.ThemeProduct = product;
            cm.Content = yorum;
            dbBaglantisi.Comments.Add(cm);
            dbBaglantisi.SaveChanges();


            return RedirectToAction("Details", "ProductTheme", new { id = gelenID });
//            return ViewBag ile listele ajax'a' bas
        }

    }
}