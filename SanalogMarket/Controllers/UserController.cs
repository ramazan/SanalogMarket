using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SanalogMarket.Models;

namespace SanalogMarket.Controllers
{
    public class UserController : Controller
    {
        DbBaglantisi dbBaglantisi = new DbBaglantisi();
        //
        // GET: /User/
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(User createUser)
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
                createUser.LastLoginTime = new DateTime(1953,1,1);


                dbBaglantisi.Users.Add(createUser);


                dbBaglantisi.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            ViewBag.HataMesjı = "Kayıt başarısız.";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(User user)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                user.Password = hash;
            }


            try
            {
                var usr = dbBaglantisi.Users.Single(u => u.Username == user.Username && u.Password == user.Password);

                if (usr != null)
                {
                    Session["UserId"] = usr.Id;
                    Session["UserName"] = usr.Username;
                    Session["Email"] = usr.Email;
                    Session["Name"] = usr.Name;
                    Session["LastName"] = usr.Surname;

                    usr.LastLoginTime = DateTime.Now;
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

        public ActionResult Profile()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public ActionResult Profile(User user)
        {
            if (user.Name != null && user.Surname != null && user.Password != null)
            {
                User kullanici = dbBaglantisi.Users.Find(Session["UserId"]);
                kullanici.Name = user.Name;
                kullanici.Surname = user.Surname;

                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    kullanici.Password = hash;
                }

                dbBaglantisi.SaveChanges();

                var usr = dbBaglantisi.Users.Find(Session["UserId"]);

                if (usr != null)
                {
                    Session["Name"] = usr.Name;
                    Session["LastName"] = usr.Surname;

                    return RedirectToAction("Index");
                }
            }


            return View();
        }
    }
}