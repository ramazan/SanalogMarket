using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SanalogMarket.Models;

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
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(admin.Password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                admin.Password = hash;
            }


            try
            {
                var admn = dbBaglantisi.Admins.Single(u => u.Username == admin.Username && u.Password == admin.Password);

                if (admn != null)
                {
                    Session["AdminId"] = admn.Id;
                    Session["AdminUserName"] = admn.Username;
                    Session["AdminName"] = admn.Name + " " + admn.Surname;
                    Session["AdminRole"] = admn.Role;

                    admn.LastLoginTime = DateTime.Now;

                    dbBaglantisi.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Yanlış");
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
            return  View();
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
    }
}