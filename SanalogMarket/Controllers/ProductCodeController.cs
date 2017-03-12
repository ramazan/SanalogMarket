using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
        public static string file_screen;
        public static string file_icon;
        public static string file_main;
        public static int gelenID;
        public static int product_user_id;
        public static  List<SelectOptions> file_namelist = new List<SelectOptions>();
        FilesHelper filesHelper;
        String tempPath = "~/somefiles/";
        String serverMapPath = "~/Project_File/somefiles/";

        public class SelectOptions
        {
            public String Text { get; set; }
        }

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
        public ActionResult New(ProductCode gelenCode, string Gender, Boolean imza,string category,string subcategory)
        {
            if ( imza)
            {
               
                

                gelenCode.CreateDate = DateTime.Now;
                gelenCode.Category = category;
                gelenCode.SubCategory = subcategory;
                gelenCode.HighResolution = Gender;
                gelenCode.imza = imza.ToString();
                gelenCode.SoftwarVersion = softwareVersion;
                gelenCode.FilesIncluded = fileinculeded;
                gelenCode.Browsers = browser;
                gelenCode.IsValid = 0;
                gelenCode.Screenshot = file_screen;
                gelenCode.Filepath = file_main;
                gelenCode.Icon = file_icon;

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
            var urun = dbBaglantisi.Codes.Find(Convert.ToInt32(id));
           
            product_user_id = urun.User.Id;
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

        [HttpPost]
        public JsonResult NewReview(string ReviewDescription,string NewReviewRate)
        {
            Thread.Sleep(1000);
            Review rw = new Review();
            List<String> values = new List<string>();

            try
            {
                int UserID = Convert.ToInt32(Session["UserId"]);
                if (UserID == 0)
                {
                    values.Add("MustLogin");
                    return Json(values, JsonRequestBehavior.AllowGet);
                }
                if (String.IsNullOrWhiteSpace(NewReviewRate))
                {
                    values.Add("RateProduct");
                    return Json(values, JsonRequestBehavior.AllowGet);
                }
                User EkleyenUser = dbBaglantisi.Users.Where(u => u.Id == UserID).FirstOrDefault();
                product = dbBaglantisi.Codes.Find(gelenID);
                Review check =
                    dbBaglantisi.Reviews.Where(u => u.ReviewAutor.Id == UserID && u.ReviewCode.ID == gelenID).FirstOrDefault();
                if (check != null)
                {
                    values.Add("alreadyAdded");
                    return Json(values, JsonRequestBehavior.AllowGet);
                }
                rw.ReviewAutor = EkleyenUser;
                rw.ReviewDate = DateTime.Now;
                rw.ReviewCode = product;
                NewReviewRate =  NewReviewRate.Replace('.', ',');
                rw.ReviewRate = Convert.ToDouble(NewReviewRate);
                rw.ReviewDescription = ReviewDescription;
                dbBaglantisi.Reviews.Add(rw);
                dbBaglantisi.SaveChanges();
                
                values.Add("success");
                values.Add(EkleyenUser.Name.ToString());
                values.Add(DateTime.Now.ToString("dd-MM-yyyy"));
                values.Add(ReviewDescription);
                NewReviewRate = NewReviewRate.Replace(',', '.');
                values.Add(NewReviewRate);
                
            }
            catch (Exception ex)
            {
                values.Add("failed");
            }
            return Json(values, JsonRequestBehavior.AllowGet);

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
        public string getScreenshot(List<String> values)
        {
            if (values != null)
            {
                file_screen = "/Project_File/somefiles/"+values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    file_screen = file_screen + "," + "/Project_File/somefiles/" + values[i];
                }
            }
            else
            {
                file_screen = null;
            }
            return file_screen;
        }
        public string getIcon(List<String> values)
        {
            if (values != null)
            {
                file_icon = "/Project_File/somefiles/"+ values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    file_icon = file_icon + "," + "/Project_File/somefiles/" + values[i];
                }
            }
            else
            {
                file_icon = null;
            }
            return file_icon;
        }
        public string getMainFiles(List<String> values)
        {
            if (values != null)
            {
                file_main = "/Project_File/somefiles/"+ values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    file_main =  file_main + "," + "/Project_File/somefiles/" + values[i];
                }
            }
            else
            {
                file_main = null;
            }
            return file_main;
        }
     

        [HttpPost]
        public JsonResult Upload()
        {
            SelectOptions select=new SelectOptions();
            var resultList = new List<ViewDataUploadFilesResult>();
            
            var CurrentContext = HttpContext;
            var httpRequest = CurrentContext.Request;
            foreach (String inputTagName in httpRequest.Files)
            {

                var headers = httpRequest.Headers;

                var file = httpRequest.Files[inputTagName];
              
                select.Text = file.FileName;

                file_namelist.Add(select);

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
            ViewBag.Liste = file_namelist;
        }
        public JsonResult GetFileList()
        {
            var list = filesHelper.FilesList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFilename()
        {
           
            return Json(file_namelist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            SelectOptions select = new SelectOptions();
            foreach (var item in file_namelist)
            {
                if (item.Text.Equals(file))
                {
                    file_namelist.Remove(item);
                    break;
                }
            }
            select.Text = file;
           
            filesHelper.DeleteFile(file);
            
            return Json("OK", JsonRequestBehavior.AllowGet);
        }


    }
}