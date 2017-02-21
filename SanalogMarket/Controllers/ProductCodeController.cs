using System;
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
        public static int gelenID;
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

        public ActionResult GetCategories()
        {
            var a = dbBaglantisi.Categories.ToList();

            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComments()
        {
            
            return Json(dbBaglantisi.Comments.Where(p => p.Product.ID == gelenID).ToList(),
                    JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetProducts(string catName)
        {
            Category category = dbBaglantisi.Categories.Where(p => p.Name == catName).FirstOrDefault();
            int intCatID = category.ID;
            var products = dbBaglantisi.SubCategories.ToList().Where(p => p.Category.ID == intCatID);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult New(ProductCode gelenCode, HttpPostedFileBase file,
            HttpPostedFileBase fileproject, HttpPostedFileBase fileIcon, string category, string subcategory)
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
                    var supportedTypes = new[] {"jpg", "jpeg", "png"};
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

                gelenCode.CreateDate = DateTime.Now;
                gelenCode.Category = category;
                gelenCode.SubCategory = subcategory;
                gelenCode.IsValid = 0;

                int UserID = (int) Session["UserId"];
                User EkleyenUser = dbBaglantisi.Users.Single(u => u.Id == UserID);

                if (EkleyenUser != null)
                {
                    gelenCode.User = EkleyenUser;
                }
                dbBaglantisi.Codes.Add(gelenCode);
                dbBaglantisi.SaveChanges();

                return RedirectToAction("Success", new {returnUrl = Request.RawUrl});
            }

            return View();
        }

        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null)
            {
                @ViewBag.Control = "0";
            }

            gelenID = (int) id;
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
            if (oncekiUrl != "/ProductCode/New")
                // Eğer yönlendirme ürün yükleme sayfasından değilse anasayfaya yönlendir.
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


            return RedirectToAction("Details", "ProductCode", new {id = gelenID});
        }
    }
}