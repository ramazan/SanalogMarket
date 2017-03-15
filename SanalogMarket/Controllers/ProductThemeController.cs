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
using jQuery_File_Upload.MVC5.Helpers;

namespace SanalogMarket.Controllers
{
    public class ProductThemeController : Controller
    {
        DbBaglantisi dbBaglantisi = new DbBaglantisi();
        public ProductTheme product;
        public static int gelenID;
        public static string file_screen;
        public static string file_icon;
        public static string file_main;
        public static List<SelectOptions> file_namelist = new List<SelectOptions>();
        FilesHelper filesHelper;
        String tempPath = "~/somefiles/";
        String serverMapPath = "~/ThemeFiles/Project_File/";

        public static string compatiblewith;
        public static string fileinculeded;
        public static string browser;


        public class SelectOptions
        {
            public String Text { get; set; }
        }

        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        private string UrlBase = "/ThemeFiles/Project_File/";
        String DeleteURL = "/ProductTheme/DeleteFile/?file=";
        String DeleteType = "GET";
        public ProductThemeController()
        {
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);
        }
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

        public void creatdir(string UserId, String ProductId)
        {
            string path = Server.MapPath("~/Uploads/User/" + UserId + "/AddItem/ProductTheme/" + ProductId + "/Project_File");
            string path1 = Server.MapPath("~/Uploads/User/" + UserId + "/AddItem/ProductTheme/" + ProductId + "/Project_Icon");
            string path2 = Server.MapPath("~/Uploads/User/" + UserId + "/AddItem/ProductTheme/" + ProductId + "/Screenshots");


            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path1);
            Directory.CreateDirectory(path2);

        }
        public ActionResult Success(string returnUrl)
        {

            //Bir onceki url'i alarak kontrol ediyorum.    
            string oncekiUrl = returnUrl;
            if (oncekiUrl != "/ProductTheme/Create")  // Eğer yönlendirme ürün yükleme sayfasından değilse anasayfaya yönlendir.
            {
                RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductTheme gelenTheme, string category, string subcategory, Boolean imza, string Gender, Boolean support)
        {
            gelenTheme.Screenshot = "";
            gelenTheme.Filepath = "";
            gelenTheme.Icon = "";

            if (ModelState.IsValid)
            {
                gelenTheme.CreateDate=DateTime.Now;
                gelenTheme.UpdateDate=new DateTime(1996,01,21);
                gelenTheme.Category = category;
                gelenTheme.SubCategory = subcategory;
                gelenTheme.IsValid = 0;
                gelenTheme.Category = category;
                gelenTheme.SubCategory = subcategory;
                gelenTheme.HighResolution = Gender;
                gelenTheme.imza = imza.ToString();
                gelenTheme.CompatibleWith = compatiblewith;
                gelenTheme.FilesIncluded = fileinculeded;
                gelenTheme.CompatibleBrowsers = browser;
                gelenTheme.Support = support;

                gelenTheme.Screenshot = file_screen;
                gelenTheme.Filepath = file_main;
                gelenTheme.Icon = file_icon;

                int UserID = (int)Session["UserId"];
                User EkleyenUser = dbBaglantisi.Users.Single(u => u.Id == UserID);

                if (EkleyenUser != null)
                {
                    gelenTheme.User = EkleyenUser;
                }
                dbBaglantisi.Themes.Add(gelenTheme);
                dbBaglantisi.SaveChanges();
                creatdir("" + gelenTheme.User.Id, "" + gelenTheme.ID);
                return RedirectToAction("Success", new { returnUrl = Request.RawUrl });

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

        public string getScreenshot(List<String> values)
        {
            if (values != null)
            {
                file_screen = "/ThemeFiles/Project_File/" + values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    file_screen = file_screen + "," + "/ThemeFiles/Project_File/" + values[i];
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
                file_icon = "/ThemeFiles/Project_File/" + values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    file_icon = file_icon + "," + "/ThemeFiles/Project_File/" + values[i];
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
                file_main = "/ThemeFiles/Project_File/" + values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    file_main = file_main + "," + "/ThemeFiles/Project_File/" + values[i];
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
            SelectOptions select = new SelectOptions();
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