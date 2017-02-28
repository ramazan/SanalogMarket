using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using jQuery_File_Upload.MVC5.Helpers;
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
        List<String> Filess = new List<String>();
        public static  List<String> file_name = new List<String>();
        FilesHelper filesHelper;
        String tempPath = "~/somefiles/";
        String serverMapPath = "~/Project_File/somefiles/";
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        private string UrlBase = "/Project_File/somefiles/";
        String DeleteURL = "/ProductCode/DeleteFile/?file=";
        String DeleteType = "GET";
        public ProductCodeController()
        {
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);
        }

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
        public ActionResult New(ProductCode gelenCode,  Dosyalar upFile,string category, string subcategory, string Gender, Boolean imza)
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
                        string path = HostingEnvironment.MapPath("~/Project_File/");
                        System.Diagnostics.Debug.WriteLine(path);
                        if (Directory.Exists(path))
                        {
                            DirectoryInfo di = new DirectoryInfo(path);
                            foreach (FileInfo fi in di.GetFiles())
                            {
                                Filess.Add(fi.Name);
                                System.Diagnostics.Debug.WriteLine(fi.Name);
                            }

                        }
                        //fileList.Add(filename);
                    }
                }

                
              

                ViewBag.Dosyalar = Filess;

              

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
        [HttpPost]
        public JsonResult Upload()
        {
            var resultList = new List<ViewDataUploadFilesResult>();
            
            var CurrentContext = HttpContext;
            var httpRequest = CurrentContext.Request;
            foreach (String inputTagName in httpRequest.Files)
            {

                var headers = httpRequest.Headers;

                var file = httpRequest.Files[inputTagName];
                file_name.Add(file.FileName);
            }
            filesHelper.UploadAndShowResults(CurrentContext, resultList);
            JsonFiles files = new JsonFiles(resultList);

            bool isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ");
            }
            else
            {
                return Json(files);
              
            }
          
        }
        public JsonResult GetFileList()
        {
            var list = filesHelper.FilesList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFilename()
        {
           
            return Json(file_name, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            filesHelper.DeleteFile(file);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }


    }
}