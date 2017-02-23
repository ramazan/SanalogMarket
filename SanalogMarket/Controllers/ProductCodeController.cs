﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SanalogMarket.Models;


namespace SanalogMarket.Controllers
{
    public class ProductCodeController : Controller
    {
        DbBaglantisi dbBaglantisi = new DbBaglantisi();
        public ProductCode product;
        public static string softwareVersion;
        public static string fileinculeded;
        public static string browser;
        public static int gelenID;
        public List<string> fileList;
        // GET: ProductCode
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "User");


            var categories = dbBaglantisi.Categories.ToList();
            ViewBag.DropDownAOOs = new SelectList(categories);


            return View();
        }
        public ActionResult GetComments()
        {

            return Json(dbBaglantisi.Comments.Where(p => p.Product.ID == gelenID).ToList(),
                    JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategories()
        {
            return Json(dbBaglantisi.Categories.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProducts(string catName)
        {
            Category category = dbBaglantisi.Categories.Where(p => p.Name == catName).FirstOrDefault();
            int intCatID = category.ID;
            var products = dbBaglantisi.SubCategories.ToList().Where(p => p.Category.ID == intCatID);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult New(ProductCode gelenCode, HttpPostedFileBase file, Dosyalar upFile,
            HttpPostedFileBase fileproject, HttpPostedFileBase fileIcon, string category, string subcategory, string Gender, Boolean imza)
        {
            if (ModelState.IsValid && imza)
            {
                foreach (var i in upFile.files )
                {
                    if (i.ContentLength> 0)
                    {
                        var filename = Path.GetFileName(i.FileName);
                        
                       var upload = Path.Combine(Server.MapPath("/Project_File/"), filename);
                        i.SaveAs(upload);
                       //fileList.Add(filename);
                    }
                }

                ViewBag.Dosyalar = fileList;

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
                    var path = Server.MapPath("/Screenshots/" + dosya.Name);
                    file.SaveAs(path);
                    var yol = "/Screenshots/" + dosya.Name;
                    @ViewBag.Screen = yol;
                    gelenCode.Screenshot = yol;
                }
//                else
//                {
//                    @ViewBag.Screenshot = "Lutfen dosya yukleyinizz.";
//                    return View();
//                }
                if (fileproject != null)
                {
                    FileInfo dosya = new FileInfo(fileproject.FileName);
                    // extract only the filename
                    var fileName = Path.GetFileName(fileproject.FileName);
                    var path = Server.MapPath("/Project_File/" + dosya.Name);
                    fileproject.SaveAs(path);
                    var yol = "/Project_File/" + dosya.Name;
                    @ViewBag.imageProject = yol;
                    gelenCode.Filepath = yol;
                }
//                else
//                {
//                    @ViewBag.filepath = "Lutfen dosya yukleyinizz.";
//                    return View();
//                }
                if (fileIcon != null)
                {
                    FileInfo dosya = new FileInfo(fileIcon.FileName);
                    // extract only the filename
                    var fileName = Path.GetFileName(fileIcon.FileName);
                    var path = Server.MapPath("/Project_Icon/" + dosya.Name);
                    fileIcon.SaveAs(path);
                    var yol = "/Project_Icon/" + dosya.Name;
                    @ViewBag.imageIcon = yol;
                    gelenCode.Icon = yol;
                }
//                else
//                {
//                    @ViewBag.icon = "Lutfen dosya yukleyinizz.";
//                    return View();
//                }

                gelenCode.CreateDate = DateTime.Now;
                gelenCode.Category = category;
                gelenCode.SubCategory = subcategory;
                gelenCode.HighResolution = Gender;
                gelenCode.imza = imza.ToString();
                gelenCode.SoftwarVersion = softwareVersion;
                gelenCode.FilesIncluded = fileinculeded;
                gelenCode.Browsers = browser;
                gelenCode.IsValid = 0;

                int UserID = (int)Session["UserId"];
                User EkleyenUser = dbBaglantisi.Users.Single(u => u.Id == UserID);

                if (EkleyenUser != null)
                {
                    gelenCode.User = EkleyenUser;
                }
                dbBaglantisi.Codes.Add(gelenCode);
                dbBaglantisi.SaveChanges();

                return RedirectToAction("Success", new { returnUrl = Request.RawUrl });
            }
            else
            {
                @ViewBag.Imza = "Failed :  Accept the Privacy Policy !! .. ";

                return View();
            }

        }

        public ActionResult Details(int? id)
        {
            if (Session["UserID"]==null)
            {
                ViewBag.Control = "0";
            }
            gelenID = Convert.ToInt32(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product = dbBaglantisi.Codes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Success(string returnUrl)
        {

            //Bir onceki url'i alarak kontrol ediyorum.    
            string oncekiUrl = returnUrl;
            if (oncekiUrl != "/ProductCode/New")  // Eğer yönlendirme ürün yükleme sayfasından değilse anasayfaya yönlendir.
            {
                RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult AddComment(string yorum)
        {
            Comment cm = new Comment();
            int id = Convert.ToInt32(Session["UserId"]);
            User EkleyenUser = dbBaglantisi.Users.Where(u => u.Id == id).FirstOrDefault();
            product = dbBaglantisi.Codes.Find(gelenID);
            cm.CommentTime = DateTime.Now;
            cm.User = EkleyenUser;
            cm.Product = product;
            cm.Content = yorum;
            dbBaglantisi.Comments.Add(cm);
            dbBaglantisi.SaveChanges();


            return RedirectToAction("Details", "ProductCode", new { id = gelenID });
        }

        public string getVersion(List<String> values)
        {
            if (values != null)
            {
                softwareVersion = values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    softwareVersion = softwareVersion + "," + values[i];
                }
            }
            else
            {
                softwareVersion = null;
            }
            return softwareVersion;
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

       
    }
}