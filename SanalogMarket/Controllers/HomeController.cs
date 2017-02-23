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
        DbBaglantisi db = new DbBaglantisi();

        public ActionResult Index()
        {
//          Bir sonraki adımımız aşağıdaki gibi sadece onaylanmış olan ürünleri göstermek olacak.
          var productCode = db.Codes.Where(p => p.IsValid == 1).ToList();
          var productTheme = db.Themes.Where(p => p.IsValid == 1).ToList();


//            var product = db.Codes.ToList();
            ViewBag.TProduct = productCode;
            ViewBag.TProductTheme = productTheme;

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

    }
}