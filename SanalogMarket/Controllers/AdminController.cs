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

        private static int AdminID;
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
                        AdminID = admn.Id;
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
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

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
        public ActionResult ProductCodeEdit(ProductCode editedProductCode, string IsValid,string RejectMessage)
        {
            try
            {
                ProductCode product = dbBaglantisi.Codes.Find(editedProductCode.ID);

                if (IsValid == "Accept")
                {
                    product.IsValid = 1;
                    if (Session["AdminId"]!=null)
                    {
                        Admin lpa = dbBaglantisi.Admins.Single(p => p.Id == AdminID);
                        product.LastProcessAdmin = lpa;
                    }
                   
                }
                else if (IsValid == "Approve")
                {
                    if (Session["AdminId"] != null)
                    {
                        Admin lpa = dbBaglantisi.Admins.Single(p => p.Id == AdminID);
                        product.LastProcessAdmin = lpa;
                    }
                    product.IsValid = 0;
                }
                else
                {   //Product Rejected!
                    product.IsValid = 2;
                    product.RejectMessage = RejectMessage;
                    if (Session["AdminId"] != null)
                    {
                        Admin lpa = dbBaglantisi.Admins.Single(p => p.Id == AdminID);
                        product.LastProcessAdmin = lpa;
                    }

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
        public ActionResult ProductThemeEdit(ProductTheme editedProductTheme, string IsValid, string RejectMessage)
        {
            try
            {
                ProductTheme product = dbBaglantisi.Themes.Find(editedProductTheme.ID);

                if (IsValid == "Accept")
                {
                    product.IsValid = 1;
                    if (Session["AdminId"] != null)
                    {
                        Admin lpa = dbBaglantisi.Admins.Single(p => p.Id == AdminID);
                        product.LastProcessAdmin = lpa;
                    }
                }
                else if (IsValid == "Approve")
                {
                    product.IsValid = 0;
                    if (Session["AdminId"] != null)
                    {
                        Admin lpa = dbBaglantisi.Admins.Single(p => p.Id == AdminID);
                        product.LastProcessAdmin = lpa;
                    }
                }
                else
                {   //Product Rejected!
                    product.IsValid = 2;
                    product.RejectMessage = RejectMessage;
                    if (Session["AdminId"] != null)
                    {
                        Admin lpa = dbBaglantisi.Admins.Single(p => p.Id == AdminID);
                        product.LastProcessAdmin = lpa;
                    }

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
            if (Session["AdminId"] == null)
                return RedirectToAction("Login");

            var productCode = dbBaglantisi.Codes.Where(p => p.IsValid == 2).ToList();
            ViewBag.productTheme = dbBaglantisi.Themes.Where(p => p.IsValid == 2).ToList();
            return View(productCode);
        }


        /*categorileri listeliyoruz*/
        public ActionResult Category()
        {
            return View(dbBaglantisi.Categories.ToList());
        }
        /*subcategorileri listeliyoruz.*/
        public ActionResult SubCategory()
        {
            return View(dbBaglantisi.SubCategories.ToList());
        }
        /*belirtilen subcategoriye ait uzantıları json formatında döndürür. Parametre almaz.*/
        public JsonResult secili_exten()
        {
            var id = Convert.ToInt32(Session["subID"]);

            var item = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == id).ToList();

            string yeni = "";
            if (item != null)
            {
                foreach (var gh in item)
                {
                    yeni = gh.uzanti;
                }

            }
            if (yeni != null)
            {
                var k = yeni.Split(',');

                return Json(k, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        /*subcategori edit*/
        public ActionResult SubEdit(int id)
        {
            Session["subID"] = id;
            var item = dbBaglantisi.SubCategories.Where(x => x.ID == id).FirstOrDefault();
            return View(item);
        }
        [HttpPost]
        public ActionResult SubEdit(SubCategory yeni)
        {
            int id = Convert.ToInt32(Session["subID"]);
            var item = dbBaglantisi.SubCategories.Where(x => x.ID == id).FirstOrDefault();
            item.Name = yeni.Name;
            item.Description = yeni.Description;
            dbBaglantisi.SaveChanges();
            return RedirectToAction("SubCategory", "Admin");
        }

        /*subedit için uzantı siler.*/
        public ActionResult silinecek_uzanti(string values)
        {

            int id = Convert.ToInt32(Session["subID"]);
            var exten = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == id).FirstOrDefault();
            var tumu = exten.uzanti.Split(',');
            var secilen = values.Split(',');
            var secilen_sayi = secilen.Length;
            var tumu_sayi = tumu.Length;


            List<string> list_tumu = new List<string>();
            List<string> list_secilen = new List<string>();
            /*subcategoriye ait uzantıları bir listeye atadık */
            foreach (var item in tumu)
            {
                list_tumu.Add(item);
            }
            /*Seçilen uzantıları bir listeye atadık.*/
            foreach (var item in secilen)
            {
                list_secilen.Add(item);
            }
            /*Subcategoriye ait uzantılardan seçilen uzantıları sildik*/
            int sayac = 0;
            for (int i = 0; i < secilen_sayi; i++)
            {
                for (int j = 0; j < tumu_sayi - sayac; j++)
                {
                    if (secilen[i].Equals(list_tumu[j]))
                    {
                        list_tumu.RemoveAt(j);
                        sayac++;
                        break;
                    }
                }
            }
            /*liste tipindeki uzantıları stringe atadık ve kaydettik.*/
            var yaz = "";
            var k = list_tumu.Count;
            if (k != 0)
            {
                yaz = list_tumu[0];
                for (int i = 1; i < k; i++)
                {
                    yaz = yaz + "," + list_tumu[i];
                }
            }
            exten.uzanti = yaz;
            dbBaglantisi.SaveChanges();

            return RedirectToAction("SubEdit", "Admin", new { id = id });
        }
        /*cat edite eklenecek uzanti*/
        public ActionResult catedit_uzantiekle(string values, string text_value, int subid, string catname, string subname)
        {
            int catid = Convert.ToInt32(Session["catID"]);
            var cat_item = dbBaglantisi.Categories.Where(x => x.ID == catid).FirstOrDefault();
            cat_item.Name = catname;
            cat_item.Description = catname;
            var sub_item = dbBaglantisi.SubCategories.Where(x => x.ID == subid).FirstOrDefault();
            sub_item.Name = subname;
            sub_item.Description = subname;

            /*eğer hiç kategoride uzantı yoksa onu baştan eklemeliyim*/
            var exten = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == subid).FirstOrDefault();
            string yaz = "";
            if (exten != null)
            {
                if (exten.uzanti != null)
                {
                    var uzanti = exten.uzanti.Split(',');
                    /*Birden çok eklenmiş uzantı varmı kontrolü yapıyoruz.*/
                    if (values != "")
                    {
                        var eklenecek = values.Split(',');
                        var sayi = eklenecek.Length;
                        yaz = eklenecek[0];

                        for (int i = 1; i < sayi - 1; i++)
                        {
                            yaz += "," + eklenecek[i];
                        }

                    }
                    List<string> liste = new List<string>();

                    /*tüm uzantıları bir listeye atıyoruz.*/
                    foreach (var item in dbBaglantisi.Lists.ToList())
                    {
                        liste.Add(item.uzanti);
                    }

                    int sayac = 0;
                    if (text_value.Equals("") == false)
                    {
                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in liste)
                        {
                            if (item.Equals(text_value))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value;
                            dbBaglantisi.Lists.Add(yeni);
                            exten.uzanti = exten.uzanti + yaz + "," + text_value;
                        }
                        else
                        {
                            exten.uzanti = exten.uzanti + yaz;
                        }
                    }
                    else
                    {
                        exten.uzanti = exten.uzanti +","+ yaz;
                    }
                }
                else
                {
                    if (values != "")
                    {
                        var eklenecek = values.Split(',');
                        var sayi = eklenecek.Length;
                        yaz = eklenecek[0];

                        for (int i = 1; i < sayi - 1; i++)
                        {
                            yaz += "," + eklenecek[i];
                        }

                    }
                    List<string> liste = new List<string>();

                    /*tüm uzantıları bir listeye atıyoruz.*/
                    foreach (var item in dbBaglantisi.Lists.ToList())
                    {
                        liste.Add(item.uzanti);
                    }

                    int sayac = 0;
                    if (text_value.Equals("") == false)
                    {
                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in liste)
                        {
                            if (item.Equals(text_value))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value;
                            dbBaglantisi.Lists.Add(yeni);
                            exten.uzanti = exten.uzanti + yaz + "," + text_value;
                        }
                        else
                        {
                            exten.uzanti = exten.uzanti + yaz;
                        }
                    }
                    else
                    {
                        exten.uzanti = exten.uzanti + yaz;
                    }

                }
            }
            else
            {/*categorinin hiç uzantısı yoksa biz baştan ekliyoruz.*/
                Extension yeni = new Extension();
                var k = dbBaglantisi.SubCategories.Where(x => x.ID == subid).FirstOrDefault();
                k.Name = subname;
                k.Description = subname;
                dbBaglantisi.SaveChanges();


                yeni.SubCategory = k;
                int catID = Convert.ToInt32(Session["catID"]);
                var category = dbBaglantisi.Categories.Where(x => x.ID == catID).FirstOrDefault();
                yeni.Category = category;

                /*liste kontrolünü burada yapıyoruz*/
                if (text_value != "")
                {
                    int sayac = 0;
                    /*tüm uzaantıları bir listeye atıyoruz*/
                    List<string> liste = new List<string>();
                    foreach (var za in dbBaglantisi.Lists.ToList())
                    {
                        liste.Add(za.uzanti);
                    }
                    /*textten girilen uzantı listede varmı kontrolü yapıyoruz yoksa listeye ekliyoruz*/
                    foreach (var lo in liste)
                    {
                        if (lo.Equals(text_value))
                        {
                            sayac++;
                        }
                    }
                    if (sayac == 0)
                    {

                        List listeye_ekle = new List();
                        listeye_ekle.uzanti = text_value;
                        dbBaglantisi.Lists.Add(listeye_ekle);

                    }
                }
                /*Biden çok seçilmiş uzantı varmı kontrolü yapıyoruz.*/
                if (values != "")
                {
                    var eklenecek = values.Split(',');
                    var sayi = eklenecek.Length;
                    yaz = eklenecek[0];
                    for (int i = 1; i < sayi - 1; i++)
                    {
                        yaz += "," + eklenecek[i];
                    }
                    yeni.uzanti = yaz + "," + text_value;


                    if (text_value == "")
                    {
                        yeni.uzanti = yaz;
                    }
                    else
                    {
                        yeni.uzanti = yaz + "," + text_value;
                    }
                }
                else
                {
                    if (text_value != "")
                    {
                        yeni.uzanti = text_value;
                    }
                }



                dbBaglantisi.Extensions.Add(yeni);

            }
            dbBaglantisi.SaveChanges();
            return RedirectToAction("Category", "Admin");
        }
        /*subcategoriyi editlerken uzantı ekleme işlemi için*/
        public ActionResult eklenecek_uzanti(string values, string text_value)
        {

            var id = Convert.ToInt32(Session["subID"]);
            /*subcategoriye ait uzantıları buluyoruz.*/
            var exten = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == id).FirstOrDefault();
            string yaz = "";
            List<string> liste = new List<string>();
            if (exten != null)
            {
                if (exten.uzanti != null)
                {
                    var uzanti = exten.uzanti.Split(',');
                    if (values != "")
                    {
                        var eklenecek = values.Split(',');
                        var sayi = eklenecek.Length;
                        yaz = eklenecek[0];
                        for (int i = 1; i < sayi - 1; i++)
                        {
                            yaz += "," + eklenecek[i];
                        }
                    }
                    if (text_value != "")
                    {
                        int sayac = 0;
                        foreach (var item in dbBaglantisi.Lists.ToList())
                        {
                            liste.Add(item.uzanti);
                        }
                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in liste)
                        {
                            if (item.Equals(text_value))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value;
                            dbBaglantisi.Lists.Add(yeni);

                        }
                        yaz = yaz + "," + text_value;
                    }

                    exten.uzanti = exten.uzanti + "," + yaz;
                    dbBaglantisi.SaveChanges();
                }
                else
                {
                    if (values != "")
                    {
                        var eklenecek = values.Split(',');
                        var sayi = eklenecek.Length;
                        yaz = eklenecek[0];
                        for (int i = 1; i < sayi - 1; i++)
                        {
                            yaz += "," + eklenecek[i];
                        }
                    }
                    if (text_value != "")
                    {
                        int sayac = 0;
                        foreach (var item in dbBaglantisi.Lists.ToList())
                        {
                            liste.Add(item.uzanti);
                        }
                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in liste)
                        {
                            if (item.Equals(text_value))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value;
                            dbBaglantisi.Lists.Add(yeni);

                        }
                        yaz =  yaz +"," + text_value;
                    }

                    if (yaz != "")
                    {
                        exten.uzanti = yaz;
                    }
                    else
                    {
                        exten.uzanti = null;
                    }

                    dbBaglantisi.SaveChanges();
                }
            }
            else
            {
                Extension yeni_uzantı = new Extension();
                if (values != "")
                {
                    var eklenecek = values.Split(',');
                    var sayi = eklenecek.Length;
                    yaz = eklenecek[0];
                    for (int i = 1; i < sayi - 1; i++)
                    {
                        yaz += "," + eklenecek[i];
                    }
                }
                if (text_value != "")
                {
                    int sayac = 0;
                    foreach (var item in dbBaglantisi.Lists.ToList())
                    {
                        liste.Add(item.uzanti);
                    }
                    /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                    foreach (var item in liste)
                    {
                        if (item.Equals(text_value))
                        {
                            sayac++;
                        }
                    }
                    if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                    {
                        List yeni = new List();
                        yeni.uzanti = text_value;
                        dbBaglantisi.Lists.Add(yeni);

                    }
                    yaz = yaz + "," + text_value;
                }
                if (yaz != null)
                {
                    yeni_uzantı.uzanti = yaz;
                }
                else
                {
                    yeni_uzantı.uzanti = null;
                }

                var sub = dbBaglantisi.SubCategories.Where(x => x.ID == id).FirstOrDefault();
                var cat = dbBaglantisi.Categories.Where(x => x.ID == sub.Category.ID).FirstOrDefault();
                yeni_uzantı.Category = cat;
                yeni_uzantı.SubCategory = sub;
                dbBaglantisi.Extensions.Add(yeni_uzantı);
                dbBaglantisi.SaveChanges();

            }
            return RedirectToAction("CreateSubCategory", "Admin");
        }
        /*View kısmına categori bilgilerini göderiyor.*/
        public ActionResult CatEdit(int id)
        {
            Session["catID"] = id;
            var item = dbBaglantisi.Categories.Where(x => x.ID == id).FirstOrDefault();
            return View(item);
        }
        /*categori edit yaparken catname kaydediyor.*/
        [HttpPost]
        public ActionResult CatEdit(string yeni)
        {
            int id = Convert.ToInt32(Session["catID"]);
            var item = dbBaglantisi.Categories.Where(x => x.ID == id).FirstOrDefault();
            item.Name = yeni;
            item.Description = yeni;
            dbBaglantisi.SaveChanges();
            return RedirectToAction("Category", "Admin");
        }
        /*subcategoriye ait uzantıları json şeklinde dödürür. */
        public JsonResult GetUzanti(int subid)
        {

            var uzantilar = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == subid).ToList();
            string yeni = "";

            if (uzantilar != null)
            {

                foreach (var dolas in uzantilar)
                {
                    yeni = dolas.uzanti;
                }
            }
            if (yeni != null)
            {
                var item = yeni.Split(',');
                return Json(item, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);


        }
        /*Categorideyken subcat ekleme işleminin veritabanı kısmı*/
        public ActionResult cat_sub_ekle(string values, string text_value, string subname)
        {
            var catid = Convert.ToInt32(Session["catID"]);
            var uzanti_ekle = "";
            /*values null mu kontrolü yapılıyor.*/
            if (values != null)
            {

                var uzantilar = values.Split(',');
                var sayi = uzantilar.Length;
                uzanti_ekle = uzantilar[0];
                for (int i = 1; i < sayi - 1; i++)
                {
                    uzanti_ekle = uzanti_ekle + "," + uzantilar[i];
                }

                /*text kısmında uzantı girmişmi kontrolü ve uzantılara ekleme işlemi*/
                if (text_value != null)
                {
                    uzanti_ekle = uzanti_ekle + "," + text_value;
                }

            }
            else/*eğer select ten uzantı seçmemiş ise*/
            {
                if (text_value != "")
                {
                    uzanti_ekle = text_value;
                }

            }

            /*categoriye ekleme işlemi yapıyor.*/
            SubCategory yeni_cat = new SubCategory();
            yeni_cat.Name = subname;
            yeni_cat.Description = subname;
            var cat = dbBaglantisi.Categories.Where(x => x.ID == catid).FirstOrDefault();
            yeni_cat.Category = cat;
            dbBaglantisi.SubCategories.Add(yeni_cat);
            dbBaglantisi.SaveChanges();
            var subid = yeni_cat.ID;
            /*uzantıyı ekleme işlemi yapıyor.*/
            Extension yeni_exten = new Extension();
            if (uzanti_ekle != "")
            {
                yeni_exten.uzanti = uzanti_ekle;
            }
            else
            {
                yeni_exten.uzanti = null;
            }
            yeni_exten.SubCategory = yeni_cat;
            yeni_exten.Category = cat;
            dbBaglantisi.Extensions.Add(yeni_exten);
            dbBaglantisi.SaveChanges();



            return View();
        }
        /*categoriye ait subcategorileri getirir*/
        public JsonResult GetSub()
        {
            var id = Convert.ToInt32(Session["catID"]);
            var item = dbBaglantisi.SubCategories.Where(x => x.Category.ID == id).ToList();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        /*categori silmek için gereklidir.*/
        public ActionResult CatDelete(int id)
        {
            /*categoriyi siliyoruz*/
            var catitem = dbBaglantisi.Categories.Where(x => x.ID == id).FirstOrDefault();
            dbBaglantisi.Categories.Remove(catitem);
            /*categoriye ait subcat siliyoruz.*/
            var subitem = dbBaglantisi.SubCategories.Where(x => x.Category.ID == id).ToList();
            foreach (var delete in subitem)
            {
                dbBaglantisi.SubCategories.Remove(delete);
            }
            /*categoriye ait uzantıları siliyoruz.*/
            var exten = dbBaglantisi.Extensions.Where(x => x.Category.ID == id).ToList();
            foreach (var item in exten)
            {
                dbBaglantisi.Extensions.Remove(item);
            }
            dbBaglantisi.SaveChanges();
            return RedirectToAction("Category", "Admin");
        }
        /*subcategori silmek için gerekli*/
        public ActionResult SubDelete(int id)
        {
            /*subcatı siliyoruz.*/
            var item = dbBaglantisi.SubCategories.Where(x => x.ID == id).FirstOrDefault();
            dbBaglantisi.SubCategories.Remove(item);
            /*subcata ait uzantılarıda siliyoruz.*/
            var exten = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == id).FirstOrDefault();
            dbBaglantisi.Extensions.Remove(exten);

            dbBaglantisi.SaveChanges();

            return RedirectToAction("SubCategory", "Admin");
        }
        /*categorileri listelemek için */
        public JsonResult GetCategory()
        {
            return Json(dbBaglantisi.Categories.ToList(), JsonRequestBehavior.AllowGet);
        }
        /*catedit değişikliklerini kaydetmek için*/
        public ActionResult CatEdit_End()
        {
            return View();
        }
        /*subcat da olmayan uzantiları listelemek için gereklidir parametre alır sub id*/
        public JsonResult GetExtension_disinda(int subid)
        {

            int id = 0;
            if (subid != 0)
            {
                id = subid;
            }
            else
            {
                id = Convert.ToInt32(Session["subID"]);
            }

            var exten = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == id).FirstOrDefault();
            List<string> at = new List<string>();
            /*subcategori uzantısı varmı kontrol ediyoruz.*/
            if (exten != null)
            {
                if (exten.uzanti != null)
                {
                    var exten_dizi = exten.uzanti.Split(',');
                    var exten_sayi = exten_dizi.Length;
                    var list = dbBaglantisi.Lists.ToList();
                    var liste_sayi = list.Count;
                    /*tüm uzantıları listeliyoruz.*/
                    foreach (var item in list)
                    {
                        at.Add(item.uzanti);
                    }

                    /*tüm uzantıdan subcat a ait uzantıları çıkartıyoruz.*/
                    int a = 0;
                    for (int i = 0; i < exten_sayi; i++)
                    {
                        for (int j = 0; j < liste_sayi - a; j++)
                        {
                            if (exten_dizi[i].Equals(at[j]))
                            {

                                at.RemoveAt(j);
                                a++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in dbBaglantisi.Lists.ToList())
                    {
                        at.Add(item.uzanti);
                    }
                }
            }
            else/*eğer subcat uzantısı yoksa uzantıların hepsini listeliyoruz.*/
            {
                var list = dbBaglantisi.Lists.ToList();
                foreach (var item in list)
                {
                    at.Add(item.uzanti);
                }
            }
            return Json(at, JsonRequestBehavior.AllowGet);
        }
        /*subcategori dışındaki uzantıları listelemek için gerekli get_extension_disinda jsonundan farkı parametre almaz aynı işlevi yapar*/
        public JsonResult GetExtension()
        {
            var id = Convert.ToInt32(Session["subID"]);
            var exten = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == id).FirstOrDefault();
            List<string> at = new List<string>();
            /*subcat ın uzantısı varmı kontrolü*/
            if (exten != null)
            {
                if (exten.uzanti != null)
                {
                    var exten_dizi = exten.uzanti.Split(',');
                    var exten_sayi = exten_dizi.Length;
                    var list = dbBaglantisi.Lists.ToList();
                    var liste_sayi = list.Count;
                    /*tüm uzantıları listeliyoruz.*/
                    foreach (var item in list)
                    {
                        at.Add(item.uzanti);
                    }

                    /*tüm uzantı listesinden subcat ye ait uzantıları çıkartıyoruz.*/
                    int a = 0;
                    for (int i = 0; i < exten_sayi; i++)
                    {
                        for (int j = 0; j < liste_sayi - a; j++)
                        {
                            if (exten_dizi[i].Equals(at[j]))
                            {

                                at.RemoveAt(j);
                                a++;
                                break;
                            }
                        }
                    }
                }
            }
            else/*subcat a ait uzantı yoksa tüm uzantıları listeleyip yolluyoruz.*/
            {
                foreach (var item in dbBaglantisi.Lists.ToList())
                {
                    at.Add(item.uzanti);
                }
            }
            return Json(at, JsonRequestBehavior.AllowGet);
        }
        /*tüm uzantıları listelemek için kukllanılır*/
        public JsonResult tum_uzanti_list()
        {
            List<string> at = new List<string>();
            foreach (var item in dbBaglantisi.Lists.ToList())
            {
                at.Add(item.uzanti);

            }
            return Json(at, JsonRequestBehavior.AllowGet);
        }
        /*category edit sayfasından uzantı silmek için*/
        public ActionResult Cat_Edit_extensil(string values, int subid)
        {
            /*subid den uzantıyı buluyoruz.*/
            var exten = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == subid).FirstOrDefault();
            var tumu = exten.uzanti.Split(',');
            var secilen = values.Split(',');
            var secilen_sayi = secilen.Length;
            var tumu_sayi = tumu.Length;

            List<string> list_tumu = new List<string>();
            List<string> list_secilen = new List<string>();
            /*subcat a ait uzantıları listeler*/
            foreach (var item in tumu)
            {
                list_tumu.Add(item);
            }
            /*secilen uzantıları listeliyoruz*/
            foreach (var item in secilen)
            {
                list_secilen.Add(item);
            }

            /*subcategoriye ait uzantılardan secilen uzantıları çıkarıyoruz.*/
            int sayac = 0;
            for (int i = 0; i < secilen_sayi; i++)
            {
                for (int j = 0; j < tumu_sayi - sayac; j++)
                {
                    if (secilen[i].Equals(list_tumu[j]))
                    {
                        list_tumu.RemoveAt(j);
                        sayac++;
                        break;
                    }
                }
            }
            /*listeyi stringe atıyoruz.*/
            var yaz = "";
            var k = list_tumu.Count;
            if (k != 0)
            {
                yaz = list_tumu[0];
                for (int i = 1; i < k; i++)
                {
                    yaz = yaz + "," + list_tumu[i];
                }
            }
            exten.uzanti = yaz;
            dbBaglantisi.SaveChanges();


            return RedirectToAction("Category", "Admin");
        }
        /*fonksiyon categori edit sayfasından subcategory silme işlemini yapıyor.*/
        public ActionResult catedit_subsil(int subid)
        {

            var subcat = dbBaglantisi.SubCategories.Where(x => x.ID == subid).FirstOrDefault();
            var exten = dbBaglantisi.Extensions.Where(x => x.SubCategory.ID == subid).FirstOrDefault();
            /*subcategoriye ait uzantı varmı kontrol ediyor.*/
            if (exten != null)
            {
                dbBaglantisi.Extensions.Remove(exten);
            }
            dbBaglantisi.SubCategories.Remove(subcat);

            dbBaglantisi.SaveChanges();
            return RedirectToAction("Category", "Admin");
        }
        public ActionResult CreateSubCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateSubCategory(string SubName, string category, string uzanti, string text_uzanti)
        {
            /*categori isminden veri tabanındaki bilgilere ulaşıldı.*/
            var cat_id = dbBaglantisi.Categories.Where(x => x.Name.Equals(category)).FirstOrDefault();
            /*subcategori kaydediliyor.*/
            SubCategory sub_item = new SubCategory();
            sub_item.Description = SubName;
            sub_item.Category = cat_id;
            sub_item.Name = SubName;
            dbBaglantisi.SubCategories.Add(sub_item);
            dbBaglantisi.SaveChanges();

            /*şimdi sabcategoriye ait uzantılar ekleniyor*/
            var uzanti_ekle = "";
            /*Birden fazla uzantı seçilmiş mi onun kontrolü*/
            if (uzanti != "")
            {

                var uzantilar = uzanti.Split(',');
                var sayi = uzantilar.Length;
                uzanti_ekle = uzantilar[0];
                for (int i = 1; i < sayi - 1; i++)
                {
                    uzanti_ekle = uzanti_ekle + "," + uzantilar[i];
                }

                /*text kısmında uzantı girmişmi kontrolü ve uzantılara ekleme işlemi*/
                if (text_uzanti != "")
                {
                    uzanti_ekle = uzanti_ekle + "," + text_uzanti;

                    int sayac = 0;

                    /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                    foreach (var item in dbBaglantisi.Lists.ToList())
                    {
                        if (item.Equals(text_uzanti))
                        {
                            sayac++;
                        }
                    }
                    if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                    {
                        List yeni = new List();
                        yeni.uzanti = text_uzanti;
                        dbBaglantisi.Lists.Add(yeni);

                    }
                }

            }
            else/*eğer select ten uzantı seçmemiş ise*/
            {
                if (text_uzanti != "")
                {
                    uzanti_ekle = text_uzanti;
                    int sayac = 0;

                    /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                    foreach (var item in dbBaglantisi.Lists.ToList())
                    {
                        if (item.Equals(text_uzanti))
                        {
                            sayac++;
                        }
                    }
                    if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                    {
                        List yeni = new List();
                        yeni.uzanti = text_uzanti;
                        dbBaglantisi.Lists.Add(yeni);

                    }
                }
            }


            /*uzantılar veri tabanına kaydediliyor.*/
            Extension yeni_exten = new Extension();
            if (uzanti_ekle != "") {
                yeni_exten.uzanti = uzanti_ekle;
            }
            else
            {
                yeni_exten.uzanti = null;
            }
            
            yeni_exten.SubCategory = sub_item;
            yeni_exten.Category = cat_id;
            dbBaglantisi.Extensions.Add(yeni_exten);
            dbBaglantisi.SaveChanges();

            /*etkisiz eleman*/
            return RedirectToAction("SubCategory");
        }
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(string catname, string subname1, string subname2, string subname3, string uzantilar1, string uzantilar2, string uzantilar3, string text_value1, string text_value2, string text_value3)
        {
            /*ilk önce kategori kaydediyoruz.*/
            Category cat = new Category();
            cat.Name = catname;
            cat.Description = catname;
            dbBaglantisi.Categories.Add(cat);
            dbBaglantisi.SaveChanges();

            /*daha sonra subcategori kaydediyoruz*/
            /*subname null mu kontrol et değilse uzantıyla birlikte kaydetme yaptık*/
            if (subname1 != "")
            {
                /*subcategori kaydettik*/
                SubCategory sub1 = new SubCategory();
                sub1.Name = subname1;
                sub1.Description = subname1;
                sub1.Category = cat;
                dbBaglantisi.SubCategories.Add(sub1);
                dbBaglantisi.SaveChanges();
                /*şimdi uzantıları kaydettik*/
                var uzanti_ekle = "";
                /*silinebilir*/
                if (uzantilar1 != "")
                {

                    var uzantilar = uzantilar1.Split(',');
                    var sayi = uzantilar.Length;
                    uzanti_ekle = uzantilar[0];
                    for (int i = 1; i < sayi - 1; i++)
                    {
                        uzanti_ekle = uzanti_ekle + "," + uzantilar[i];
                    }

                    /*text kısmında uzantı girmişmi kontrolü ve uzantılara ekleme işlemi*/
                    if (text_value1 != "")
                    {
                        uzanti_ekle = uzanti_ekle + "," + text_value1;
                        /*textten girilen uzantı listede yoksa listeye ekleme*/
                        int sayac = 0;

                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in dbBaglantisi.Lists.ToList())
                        {
                            if (item.Equals(text_value1))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value1;
                            dbBaglantisi.Lists.Add(yeni);

                        }
                    }


                }
                else/*eğer select ten uzantı seçmemiş ise*/
                {
                    if (text_value1 != "")
                    {
                        uzanti_ekle = text_value1;
                        int sayac = 0;

                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in dbBaglantisi.Lists.ToList())
                        {
                            if (item.Equals(text_value1))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value1;
                            dbBaglantisi.Lists.Add(yeni);

                        }

                    }
                }
                /*uzantıları kaydediyoruz*/
                Extension exten = new Extension();
                exten.Category = cat;
                exten.SubCategory = sub1;
                if (uzanti_ekle == "")
                {
                    exten.uzanti = null;
                }
                else
                {
                    exten.uzanti = uzanti_ekle;
                }

                dbBaglantisi.Extensions.Add(exten);
                dbBaglantisi.SaveChanges();

            }
            /*subname2 kontrolü*/
            if (subname2 != "")
            {
                /*subategori kaydettik*/
                SubCategory sub2 = new SubCategory();
                sub2.Name = subname2;
                sub2.Description = subname2;
                sub2.Category = cat;
                dbBaglantisi.SubCategories.Add(sub2);
                dbBaglantisi.SaveChanges();
                /*şimdi uzantıları kaydettik*/
                var uzanti_ekle = "";
                /*silinebilir*/
                if (uzantilar2 != "")
                {

                    var uzantilar = uzantilar2.Split(',');
                    var sayi = uzantilar.Length;
                    uzanti_ekle = uzantilar[0];
                    for (int i = 1; i < sayi - 1; i++)
                    {
                        uzanti_ekle = uzanti_ekle + "," + uzantilar[i];
                    }

                    /*text kısmında uzantı girmişmi kontrolü ve uzantılara ekleme işlemi*/
                    if (text_value2 != "")
                    {
                        uzanti_ekle = uzanti_ekle + "," + text_value2;
                        int sayac = 0;

                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in dbBaglantisi.Lists.ToList())
                        {
                            if (item.Equals(text_value2))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value2;
                            dbBaglantisi.Lists.Add(yeni);

                        }
                    }

                }
                else/*eğer select ten uzantı seçmemiş ise*/
                {
                    if (text_value2 != "")
                    {
                        uzanti_ekle = text_value2;
                        int sayac = 0;

                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in dbBaglantisi.Lists.ToList())
                        {
                            if (item.Equals(text_value2))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value2;
                            dbBaglantisi.Lists.Add(yeni);

                        }
                    }
                }
                /*uzantıları kaydediyoruz*/
                Extension exten = new Extension();
                exten.Category = cat;
                exten.SubCategory = sub2;
                exten.uzanti = uzanti_ekle;
                dbBaglantisi.Extensions.Add(exten);
                dbBaglantisi.SaveChanges();

            }
            /*subname3 kontrolü*/
            if (subname3 != "")
            {
                /*subategori kaydettik*/
                SubCategory sub3 = new SubCategory();
                sub3.Name = subname3;
                sub3.Description = subname3;
                sub3.Category = cat;
                dbBaglantisi.SubCategories.Add(sub3);
                dbBaglantisi.SaveChanges();
                /*şimdi uzantıları kaydettik*/
                var uzanti_ekle = "";

                if (uzantilar3 != "")
                {

                    var uzantilar = uzantilar3.Split(',');
                    var sayi = uzantilar.Length;
                    uzanti_ekle = uzantilar[0];
                    for (int i = 1; i < sayi - 1; i++)
                    {
                        uzanti_ekle = uzanti_ekle + "," + uzantilar[i];
                    }

                    /*text kısmında uzantı girmişmi kontrolü ve uzantılara ekleme işlemi*/
                    if (text_value3 != "")
                    {
                        uzanti_ekle = uzanti_ekle + "," + text_value3;
                        int sayac = 0;

                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in dbBaglantisi.Lists.ToList())
                        {
                            if (item.Equals(text_value3))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value3;
                            dbBaglantisi.Lists.Add(yeni);

                        }
                    }

                }
                else/*eğer select ten uzantı seçmemiş ise*/
                {
                    if (text_value3 != "")
                    {
                        uzanti_ekle = text_value3;
                        /*text değeri listede varmı kontrolü*/
                        int sayac = 0;

                        /*Girilen text değeri listede varmı onun kontrolünü yapıyor.*/
                        foreach (var item in dbBaglantisi.Lists.ToList())
                        {
                            if (item.Equals(text_value3))
                            {
                                sayac++;
                            }
                        }
                        if (sayac == 0)/*girilen text value değeri listede var mı kontrol ediliyor.*/
                        {
                            List yeni = new List();
                            yeni.uzanti = text_value3;
                            dbBaglantisi.Lists.Add(yeni);

                        }

                    }
                }
                /*uzantıları kaydediyoruz*/
                Extension exten = new Extension();
                exten.Category = cat;
                exten.SubCategory = sub3;
                exten.uzanti = uzanti_ekle;
                dbBaglantisi.Extensions.Add(exten);
                dbBaglantisi.SaveChanges();

            }


            /*etkisiz eleman*/
            return RedirectToAction("Category");
        }


    }
}