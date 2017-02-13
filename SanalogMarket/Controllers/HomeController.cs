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
            var model = db.Codes
                .Join(db.Categories, p => p.Category, n => n.ID,
                    (PCode, Category) => new {PCode, Category})
                .Join(db.SubCategories, r => r.PCode.SubCategory, ro => ro.ID,
                    (r, ro) => new ProductCodeCategory() {Category = r.Category, Code = r.PCode, SubCategory = ro})
                .ToList();
//
//            var product = db.Codes.ToList();
//            ViewBag.TProduct = model;
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}