using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SanalogMarket.Models;

namespace SanalogMarket.Controllers
{
    public class HomeController : Controller
    {

        public static string Product_kind;

        DbBaglantisi db = new DbBaglantisi();

        public ActionResult Index()
        {
//          Bir sonraki adımımız aşağıdaki gibi sadece onaylanmış olan ürünleri göstermek olacak.
          var product = db.Codes.Where(p => p.IsValid == 1).ToList();
         // var productTheme = db.Themes.Where(p => p.IsValid == 1).ToList();


//            var product = db.Codes.ToList();
            ViewBag.TProduct = product;
         //   ViewBag.TProductTheme = productTheme;

            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SellProduct()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "User");


            return View();
        }
        [HttpPost]
        public ActionResult SellProduct(string codes)
        {
            if (codes.Equals("Upload Code or Plugin!"))
            {
                Product_kind = "code";
                return RedirectToAction("Code", "Product");
            }
            else
            {
                Product_kind = "theme";
                return RedirectToAction("Theme", "Product");

            }
//            return View();
        }
        /*categorylerin layouta gönderilmesi için*/

        public JsonResult Get_Cat()
        {
            var item = db.Categories.ToList();

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        /*Sub Categorilri menüde listelemek için*/

        public JsonResult Get_Sub(int a)
        {
            var item = db.SubCategories.Where(x => x.Category.ID == a).ToList();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

    }
}