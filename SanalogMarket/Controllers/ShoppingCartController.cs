using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SanalogMarket.Models;

namespace SanalogMarket.Controllers
{
    public class ShoppingCartController : Controller
    {
        DbBaglantisi dbBaglantisi = new DbBaglantisi();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult OrderNow(int id)
        {
            if (Session["cart"] == null)
            {
                List<ProductCode> cart = new List<ProductCode>();

                cart.Add(dbBaglantisi.Codes.Find(id));

                Session["cart"] = cart;
            }
            else
            {
                List<ProductCode> cart = (List<ProductCode>) Session["cart"];
                if (isExisting(id) == -1)
                {
                    cart.Add(dbBaglantisi.Codes.Find(id));
                    Session["cart"] = cart;
                }
                else
                {
                    TempData["infoMessage"] = "Krdş nbysn seçilen ürün sepette zaten var!";
                }
            }


            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            List<ProductCode> cart = (List<ProductCode>) Session["cart"];
            cart.RemoveAt(isExisting(id));
            if (cart.Count == 0)
            {
                Session["cart"] = null;
            }
            else
            {
                Session["cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        private int isExisting(int id)
        {
            List<ProductCode> cart = (List<ProductCode>) Session["cart"];

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].ID == id)
                    return i;
            }

            return -1;

            throw new NotImplementedException();
        }
    }
}