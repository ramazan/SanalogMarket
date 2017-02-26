using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SanalogMarket.Models;
using SanalogMarket.Models.Theme;

namespace SanalogMarket.Controllers
{
    public class AdminController : Controller
    {
        DbBaglantisi dbBaglantisi = new DbBaglantisi();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }


        public ActionResult Register()
        {
            if (Session["AdminUserName"] == null)
                return RedirectToAction("Login", "Admin");

            if (Session["AdminRole"].ToString() != "Admin")
                return RedirectToAction("Index", "Admin");


            return View();
        }


        [HttpPost]
        public ActionResult Register(Admin createAdmin)
        {
            if (ModelState.IsValid)
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(createAdmin.Password));
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    createAdmin.Password = hash;
                }


                createAdmin.RegisterTime = DateTime.Now;
                createAdmin.LastLoginTime = new DateTime(1953, 1, 1);
                createAdmin.Avatar = "/Project_Icon/user.png";

                dbBaglantisi.Admins.Add(createAdmin);


                dbBaglantisi.SaveChanges();

                ModelState.AddModelError("", "Kullanıcı Başarıyla kaydedildi");
                return RedirectToAction("Admins", "Admin");
            }

            ViewBag.HataMesjı = "Kayıt başarısız.";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if (!(admin.Username == null || admin.Password == null))
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(admin.Password));
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    admin.Password = hash;
                }


                try
                {
                    var admn =
                        dbBaglantisi.Admins.Single(u => u.Username == admin.Username && u.Password == admin.Password);

                    if (admn != null)
                    {
                        Session["AdminId"] = admn.Id;
                        Session["AdminUserName"] = admn.Username;
                        Session["AdminName"] = admn.Name + " " + admn.Surname;
                        Session["AdminRole"] = admn.Role;
                        Session["AdminAvatar"] = admn.Avatar;

                        admn.LastLoginTime = DateTime.Now;

                        dbBaglantisi.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ViewBag.Error = "Kullanıcı Adı veya Şifre Yanlış";
                }
            }


            return View();
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }


        public ActionResult Profile()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public ActionResult Profile(Admin admn)
        {
            if (admn.Name != null && admn.Surname != null && admn.Password != null)
            {
                Admin kullanici = dbBaglantisi.Admins.Find(Session["AdminId"]);
                kullanici.Name = admn.Name;
                kullanici.Surname = admn.Surname;

                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(admn.Password));
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    kullanici.Password = hash;
                }

                dbBaglantisi.SaveChanges();

                var admnusr = dbBaglantisi.Admins.Find(Session["AdminId"]);

                if (admnusr != null)
                {
                    Session["AdminName"] = admnusr.Name + " " + admnusr.Surname;

                    return RedirectToAction("Index");
                }
            }


            return View();
        }

        public ActionResult Admins()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var list = dbBaglantisi.Admins.ToList();
            return View(list);
        }

        public ActionResult Users()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var list = dbBaglantisi.Users.ToList();
            return View(list);
        }

        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(User createUser)
        {
            if (ModelState.IsValid)
            {
                // SHA256 is disposable by inheritance.  
                using (var sha256 = SHA256.Create())
                {
                    // Send a sample text to hash.  
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(createUser.Password));
                    // Get the hashed string.  
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    // Print the string.   

                    createUser.Password = hash;
                }


                createUser.RegisterTime = DateTime.Now;
                createUser.LastLoginTime = new DateTime(1953, 1, 1);


                dbBaglantisi.Users.Add(createUser);


                dbBaglantisi.SaveChanges();

                return RedirectToAction("Users", "Admin");
            }

            ViewBag.HataMesjı = "Kayıt başarısız.";
            return View();
        }


        public ActionResult ProductWaiting()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");


            var productCode = dbBaglantisi.Codes.Where(p => p.IsValid == 0).ToList();

            ViewBag.productTheme = dbBaglantisi.Themes.Where(p => p.IsValid == 0).ToList();

            
            return View(productCode);
        }


        public ActionResult ProductApproved()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");


            var product = dbBaglantisi.Codes.Where(p => p.IsValid == 1).ToList();

            ViewBag.productTheme = dbBaglantisi.Themes.Where(p => p.IsValid == 1).ToList();

            return View(product);
        }


        public ActionResult ProductCodeDetails(int? id)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = dbBaglantisi.Codes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult ProductCodeEdit(int? id)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = dbBaglantisi.Codes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewData["CheckState"] = product.IsValid;

            return View(product);
        }

        [HttpPost]
        public ActionResult ProductCodeEdit(ProductCode editedProductCode, bool IsValid)
        {
            try
            {
                ProductCode product = dbBaglantisi.Codes.Find(editedProductCode.ID);

                if (IsValid == true)
                {
                    product.IsValid = 1;
                }
                else
                {
                    product.IsValid = 0;

                }
                //                product.IsValid = editedProductTheme.IsValid;
                product.Price = editedProductCode.Price;

                dbBaglantisi.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Bi hata oldu kardeş :(");
                Console.WriteLine(e);
                throw;
            }

            ViewBag.Succes = "Düzenleme başarılı";
            return View();
        }


        public ActionResult ProductThemeDetails(int? id)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = dbBaglantisi.Themes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult ProductThemeEdit(int? id)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = dbBaglantisi.Themes.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewData["CheckState"] = product.IsValid;

            return View(product);
        }

        [HttpPost]
        public ActionResult ProductThemeEdit(ProductTheme editedProductTheme, bool IsValid)
        {
            try
            {
                ProductTheme product = dbBaglantisi.Themes.Find(editedProductTheme.ID);

                if (IsValid == true)
                {
                    product.IsValid = 1;
                }
                else
                {
                    product.IsValid = 0;

                }
                //                product.IsValid = editedProductTheme.IsValid;
                product.Price = editedProductTheme.Price;

                dbBaglantisi.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Bi hata oldu kardeş :(");
                Console.WriteLine(e);
                throw;
            }

            ViewBag.Succes = "Düzenleme başarılı";
            return View();
        }

        public ActionResult ProductRejected()
        {
            var productCode = dbBaglantisi.Codes.Where(p => p.IsValid == 3).ToList();
            ViewBag.productTheme = dbBaglantisi.Themes.Where(p => p.IsValid == 3).ToList();
            return View(productCode);
        }


        public ActionResult Category()
        {
            var item = dbBaglantisi.Categories.ToList();
            return View(item);
        }
    }
}