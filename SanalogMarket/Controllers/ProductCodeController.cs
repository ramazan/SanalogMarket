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
        // GET: ProductCode
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category_index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "User");


            var categories = dbBaglantisi.Categories.ToList();
            ViewBag.DropDownAOOs = new SelectList(categories);


            return View();
        }

        public ActionResult GetCategories()
        {
            return Json(dbBaglantisi.Categories.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProducts(int intCatID)
        {
            var products = dbBaglantisi.SubCategories.ToList().Where(p => p.Category_ID == intCatID);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Category_index(ProductCode gelenCode, HttpPostedFileBase file,
            HttpPostedFileBase fileproject, HttpPostedFileBase fileIcon, string category, string subcategory)
        {
            if (ModelState.IsValid)
            {
                if (file != null)

                {
                    if (file.ContentLength > 10240*100)
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
                    FileInfo dosya=new FileInfo(fileIcon.FileName);
                    // extract only the filename
                    var fileName = Path.GetFileName(fileIcon.FileName);
                    var path = Server.MapPath("/Project_Icon/" + dosya.Name);
                    fileIcon.SaveAs(path);
                    var yol = "/Project_Icon/" + dosya.Name;
                    @ViewBag.imageIcon = yol;
                    gelenCode.Icon = yol;
                }

                gelenCode.Category = Convert.ToInt32(category);
                gelenCode.SubCategory = Convert.ToInt32(subcategory);
                gelenCode.IsValid = 0;

                int UserID = (int) Session["UserId"];
                User EkleyenUser = dbBaglantisi.Users.Single(u => u.Id == UserID);

                if (EkleyenUser != null)
                {
                    gelenCode.User = EkleyenUser;
                }
                dbBaglantisi.Codes.Add(gelenCode);
                dbBaglantisi.SaveChanges();
            }


            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product = dbBaglantisi.Codes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.product = product;
            }
            return View(product);
           
        }
        [HttpPost]
        public ActionResult Details()
        {


            return View();

        }
    }
}