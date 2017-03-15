using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SanalogMarket.Models;

namespace SanalogMarket.Controllers
{
    public class UserController : Controller
    {
        DbBaglantisi dbBaglantisi = new DbBaglantisi();

        public bool SendConfirmEmail(User user)
        {
            string Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            string TokenWithUserId = Token.Replace("+", "r").Replace("/", "d") +user.Id;


            //Eğer Mail gönderme başarılı ise true dön!
            try
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("ramazan@sanalog.org", "Sanalog Market"),
                    new System.Net.Mail.MailAddress(user.Email));


                m.Subject = "E-mail Confirmation";

                m.Body =
                    string.Format(
                        "<!DOCTYPE html>\r\n<html>\r\n  <head>\r\n   </head>\r\n  <body>\r\n<div class=\"main\">\r\n<div id=\"m_-7833288497107706094editable\">\r\n<table class=\"m_-7833288497107706094container\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#eceff1\">\r\n<tbody>\r\n<tr>\r\n<td align=\"center\">\r\n<table class=\"m_-7833288497107706094container\" style=\"border-top-left-radius: 3px; border-top-right-radius: 3px;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" bgcolor=\"#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td align=\"center\">\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td height=\"40\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td align=\"center\"><img style=\"display: block;\" src=\"https://ci3.googleusercontent.com/proxy/cbgadI1ObZGxgBdLTnXUS7P4czRmnQK4ELLOTVfcz-xFcBVFyMz5bGl23MlFmo9xIVGWAl5-t1qMFqajRW5tG69nZs3-iSU-3OWYUqEjO0tYr905nsGTjKLpBLRxXKW0TmbSjsZfSUlcFS47Olh_ZEBfLDNTztkqpvAsm3c=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/9b80e05e-396d-4a9e-abd1-0cf4a6da578c.png\" border=\"0\" alt=\"Sanalog Market\" width=\"248\" height=\"40\" class=\"CToWUd\" /></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td height=\"40\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td class=\"m_-7833288497107706094head1\" style=\"font-family: \'PT Sans\',Arial,Helvetica,sans-serif; font-size: 40px; line-height: 52px; color: #243238; font-weight: bold;\" align=\"center\"><span>Registration at Sanalog Market</span></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td height=\"40\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family: \'PT Sans\',Arial,Helvetica,sans-serif; font-size: 14px; line-height: 21px; color: #243238;\" align=\"center\"><span>Hello <span style=\"font-weight: bold; text-decoration: none;\" >{0}</span>,<br /> Looks like you want to register at <a href=\"www.SanalogMarket.com\" style=\"color: #2196f3; font-weight: bold; text-decoration: none;\" target=\"_blank\">Sanalog Market</a>,<br />so we ask you to confirm your email.</span></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td height=\"40\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td align=\"center\"><a href=\"{1}\"><img style=\"display: block;\" src=\"https://ci6.googleusercontent.com/proxy/P5u4l_jW89BD2atoD8Nevpea8uSVmG4pmZh6d2_Bsbo8a9_GhmOHXE2kYrSvDQyivyU7SPZEV0VFznKuqan5jeYYa-icpHBA-UeqOnUs9mA1duMsBU9VvDjyJrJXCyLOFPtGYLjmhYSJ774TIBgxsx0Qcbl22EoZEJ0x8Oc=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/49b251f0-745c-41d9-ac39-a1f2d1425bc5.png\" border=\"0\" alt=\"Confirm\" width=\"163\" height=\"60\" class=\"CToWUd\" /></a></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td height=\"40\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family: \'PT Sans\',Arial,Helvetica,sans-serif; font-size: 14px; line-height: 21px; color: #243238;\" align=\"center\"><span><span style=\"font-weight: bold;\">Stay strong in the pursuit of your goals!</span><br /> Best wishes, Sanalog Market Team</span></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094content\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td height=\"40\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094container\" style=\"border-bottom-left-radius: 3px; border-bottom-right-radius: 3px; border-top: 1px solid #dde3e6;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" bgcolor=\"#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td class=\"m_-7833288497107706094container\" align=\"center\">\r\n<table class=\"m_-7833288497107706094container\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td height=\"68\" align=\"center\" valign=\"bottom\"><img class=\"m_-7833288497107706094align CToWUd\" style=\"display: block; margin-left: 63px;\" src=\"https://ci3.googleusercontent.com/proxy/Ebfu7XlraCFGFrgJI-zFBHxVvXl0NO_XQYvRVCyPZ2UNJwwxH9Tkyo4kqQN5IgAfgQhvqtnDxODLCbfBWxPCaHuj8z22f1IWRfYltnceb5Vt1uHLxL5L6OXredRedzwigYo6s8J8J1y3Gs3HrBX5b2y30D17oQ_UiZiqzLY=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/14f09f85-852c-4582-885b-84043fe2e167.png\" border=\"0\" alt=\"Please, follow us on:\" width=\"182\" height=\"28\" /></td>\r\n</tr>\r\n<tr>\r\n<td height=\"20\" align=\"center\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094follow\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td>\r\n<table class=\"m_-7833288497107706094flleft\" style=\"float: left;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"340\" align=\"left\">\r\n<tbody>\r\n<tr>\r\n<td>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\">\r\n<tbody>\r\n<tr>\r\n<td align=\"left\"><img class=\"m_-7833288497107706094hide CToWUd\" style=\"display: block;\" src=\"https://ci3.googleusercontent.com/proxy/h1dSm39xPsfEydvFNC67NjpfbmItlFgMxeG2jNG_D98pq8RJ4WaSraQrnLRlTSUJDS7H92_HOMD3geDUyE7oEVFTitC9eQ8DolymJFeloJFfgxj9umOjtmltx-5fhX8zg4AN9HlaAQuzFwGNPoEbva-L5U3xPRm-zyKEeHI=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/3c5dbaaf-7602-49fd-8996-9c365c58769d.png\" border=\"0\" alt=\"Facebook\" width=\"160\" height=\"40\" /></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<div class=\"m_-7833288497107706094reset\" style=\"float: left; max-height: 0px; font-size: 0; display: none; overflow: hidden;\"><img class=\"m_-7833288497107706094followmin CToWUd\" style=\"display: block;\" src=\"https://ci5.googleusercontent.com/proxy/kGZix-himd69eX4s7n42G2V_hAkPT4BNX40hWZqbrax-oLAPhW7e4IkYTxixtaEPoDK98wspDFQjg0x060QOA5q846WGeJVw-pEPyFs3UH-PO7xBehPf1-kV-0-mygD0RrOTyf0GuUWPYjuUs2TrysMoZufwI26AcrRFwRA=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/d7c03270-ac2c-4324-9bf7-1965182c7fda.png\" border=\"0\" alt=\"\" /></div>\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"right\">\r\n<tbody>\r\n<tr>\r\n<td align=\"right\"><img class=\"m_-7833288497107706094hide CToWUd\" style=\"display: block;\" src=\"https://ci5.googleusercontent.com/proxy/fdXpFbYIUgpXBfZT9zDFDGwVeW_Ma1SzBGOgccrDAqX7GDJsJTVTf1-sxqjMUkdDB7pjihbg_90VuohMlMddSoSX_tKAB3CFmoeKSW3fq-nmVpliMGmF8S42b_t8PY1HllOFhHPY1WqpRCVHRVf_ZAZfMdqdY6IRXWxFObI=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/dac958c4-ef79-4255-ba7e-eb03e7fb7a7e.png\" border=\"0\" alt=\"Twitter\" width=\"160\" height=\"40\" /></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<div class=\"m_-7833288497107706094reset\" style=\"float: right; max-height: 0px; font-size: 0; display: none; overflow: hidden;\"><img class=\"m_-7833288497107706094followmin CToWUd\" style=\"display: block;\" src=\"https://ci3.googleusercontent.com/proxy/4wjrfVbX3VcSUttNqjdz69NjBzzyngXYzHTDblP-CCt7lWd-kT6Q46_GJACKP8k1DB_puRXFyYlKhxaCp2i0EQYeYPTW2CP44CIbHcaesnE62XpU-msxjFcDQTEv7WC-gczaRNAK7IRylnSufDNJBetWDYp-NGeX1nKZUFg=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/a1d1f667-aa4b-4911-b90c-df9919542130.png\" border=\"0\" alt=\"\" /></div>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094flleft\" style=\"float: right;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"160\" align=\"right\">\r\n<tbody>\r\n<tr>\r\n<td><img class=\"m_-7833288497107706094hide CToWUd\" style=\"display: block;\" src=\"https://ci6.googleusercontent.com/proxy/NAcNXcxqN865eC2It5_Y6Dx_u0T3VEoguXBB4TpoRLz0iNFLuAjZ3Ljzn41jxDEZfPuthN23OhKU789aOEuaRDg4qMFhvNvykPIgrty9LTI6aS8Tqed41aABKuY0LDM0yHZ4mKzV_kL8URA84V0GejX_wDuBzGUYJUj-25Y=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/689a3405-d875-48f7-81c3-92d702940dfd.png\" border=\"0\" alt=\"Our Blog\" width=\"160\" height=\"40\" />\r\n<div class=\"m_-7833288497107706094reset\" style=\"float: right; max-height: 0px; font-size: 0; display: none; overflow: hidden;\"><img class=\"m_-7833288497107706094followmax CToWUd\" style=\"display: block;\" src=\"https://ci5.googleusercontent.com/proxy/KTCe5tzLrIXCL76uOuTwnbnjSvMXKP3DFPcy0OVY3Cxsl7hA4HJyS93EH_PN_4YqmM29BmqVIq_kFXcqvGNBMRRYKX8hfT4ETTBWE9QgDLydLSafKi9P0IpRDDPiwT7LkK9XhjTxNipuPXkXKub4YfMmULMmoiy3-zqgErA=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/d1afa924-16ee-4e52-b5dc-8c165ff7a4c5.png\" border=\"0\" alt=\"\" /></div>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094follow\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"220\">\r\n<tbody>\r\n<tr>\r\n<td height=\"60\" valign=\"bottom\"><img style=\"display: block;\" src=\"https://ci5.googleusercontent.com/proxy/_KlKocd7eejuNFTLXkhV1rzzj-y4ZOpDPa29rD7K0bd6GtU5Eo0pc0UULEHuzQPVykqI06DMm_K3ZYFnyPCXVyMpdVKIJ1zIIQ-ltS5EuVx-XmLHKlfu5x6vOjwl3LYCLytpATbR5M2EfWwP7watKhTKaqCGJWnN4WgB-fE=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/b175d993-b0c6-4eb2-9b56-9421a50ccb87.png\" border=\"0\" alt=\"G+\" width=\"40\" height=\"40\" class=\"CToWUd\" /></td>\r\n<td width=\"20\"> </td>\r\n<td valign=\"bottom\"><<img style=\"display: block;\" src=\"https://ci4.googleusercontent.com/proxy/n6uKO6bM9dn7Xu6J31nkWAyf4qKhF1A9YyIZrJGufvy6bY4JWMSY6GVZEflPjPEBfrzKiIr-JGwd3F2yCr9dDBg5CeKdVb9hMEIwrOeVmNIhlAb0piN9kbfQLMSg72CD3g5VVco-wuojBFw7kR4rNTtJkC7NpvvgniKG7cY=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/370be562-5f8f-4c02-bb2e-69488934c7f9.png\" border=\"0\" alt=\"Inst\" width=\"40\" height=\"40\" class=\"CToWUd\" /></td>\r\n<td width=\"20\"> </td>\r\n<td valign=\"bottom\"><img style=\"display: block;\" src=\"https://ci5.googleusercontent.com/proxy/MXkAseUdMzPAp-1Q5dn1wX5Kz0F-xDRcIfOyzPQlX2YN6WYVwVX34-34-GsC7KFXMBCuLJZyT3wZQ-9HfA37BubiGKC_RByhlBILY2hN9GV8OlR5kTWD8Rwg6lSrgy7CaIVO0iNF0SZHQPDTr0E7obyowBtHgAmyrQ3QnQ8=s0-d-e1-ft#https://gallery.mailchimp.com/d78239e73023f6e63abce9fad/images/525f1d5f-f053-4bd2-bdf2-a3ddafedcbec.png\" border=\"0\" alt=\"Ytb\" width=\"40\" height=\"40\" class=\"CToWUd\" /></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094container\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family: \'PT Sans\',Arial,Helvetica,sans-serif; font-size: 12px; line-height: 18px; color: #243238;\" height=\"19\" align=\"center\" valign=\"middle\">\r\n<p style=\"margin-top: 40px; margin-bottom: 5px; font-weight: 400;\">© 2017 ISTANBUL,LAKS Inc. All Rights Reserved</p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094container\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"520\">\r\n<tbody>\r\n<tr>\r\n<td height=\"40\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094container\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family: \'PT Sans\',Arial,Helvetica,sans-serif; font-size: 12px; line-height: 18px; color: #90a4ae;\" align=\"center\">\r\n<p style=\"margin-bottom: 40px;\"> </p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table class=\"m_-7833288497107706094fix\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n<tbody>\r\n<tr>\r\n<td class=\"m_-7833288497107706094fix\" style=\"min-width: 600px; font-size: 0px; line-height: 0px;\" height=\"1\"> </td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</div>\r\n</div>\r\n<p></p>\r\n<p></p>\r\n</body>\r\n</html>",
                        user.Name + " " + user.Surname, "http://localhost:1388/User/ConfirmEmail/" + TokenWithUserId);

                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.yandex.com.tr");
                smtp.Credentials = new System.Net.NetworkCredential("ramazan@sanalog.org", "124500");
                //            smtp.ServerCertificateValidationCallback = () => true; //Solution for client certificate error
                smtp.EnableSsl = true;
                smtp.Send(m);
                //            return RedirectToAction("Index", "Home");

                UserConfirm userConfirm = new UserConfirm
                {
                    User = user,
                    Token = Token
                };

                dbBaglantisi.UserConfirms.Add(userConfirm);
                dbBaglantisi.SaveChanges();

                return true;
            }
            catch(Exception e )
            {
                //Eğer mail gönderirken bir hata vs. oluşursa false dön ve kaydedilen kullanıcıyı database'den kaldır!
                dbBaglantisi.Users.Remove(user);
                dbBaglantisi.SaveChanges();

                return false;
            }
           
        }

        public ActionResult ConfirmEmail(string Id)
        {
            string urlParametre = Id;

            string[] lines = Regex.Split(urlParametre, "==");

            //Bölünen ilk string tokendir.
            string token = lines[0] + "==";

            //ikincisi ise userId'dir.
            int userId = Convert.ToInt16(lines[1]);

            try
            {
                var userConfirm = dbBaglantisi.UserConfirms.Single(p => p.Token == token && p.User.Id == userId);

                if (userConfirm != null)
                {
                    ViewBag.mesaj = "Sayın " + userConfirm.User.Name + " " + userConfirm.User.Surname +
                                    " Kaydınız başarıyla tamamlandı!";
                }
            }
            catch (Exception e)
            {
                ViewBag.mesaj = "Beklenmedik bir hata oluştu,Lütfen mailinize gelen linke tıkladığınızdan emin olun!";
            }


            return View();
        }

        [AllowAnonymous]
        public JsonResult IsUserNameExist(string Username)
        {
            return Json(!dbBaglantisi.Users.Any(lo => lo.Username == Username), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult IsEmailExist(string Email)
        {
            return Json(!dbBaglantisi.Users.Any(lo => lo.Email == Email), JsonRequestBehavior.AllowGet);
        }


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
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

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
                createUser.LastLoginTime = new DateTime(1953, 1, 1);


                dbBaglantisi.Users.Add(createUser);


                dbBaglantisi.SaveChanges();

                var user = dbBaglantisi.Users.Single(p => p.Email == createUser.Email);

                if (SendConfirmEmail(user))
                {
                    ViewBag.Mesaj = "You're succesfully registered! Please check your E-mail for confirm your account!";
                    return View();

                }
               
                //Eğer üstteki if ifadesi true ise ordan dön , doğru değilse hata mesajı verip dön! 
                ViewBag.HataMesajı = "Kayıt işlemi gerçekleştirilerken hata meydana geldi!";
                return View();

            }

            ViewBag.HataMesajı = "Kayıt başarısız.";
            return View();
        }

        public ActionResult Login()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public ActionResult Login(User user)
        {
            if (!(user.Username == null || user.Password == null))
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
                    ViewBag.HataMesjı = "Kullanıcı Adı veya Şifre Yanlış";
                }
            }
            return View();
        }

        public ActionResult Public_Profile()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            var profile = dbBaglantisi.Users.Find(ProductCodeController.product_user_id);
            return View(profile);
        }



        public ActionResult Profile()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            var profile = dbBaglantisi.Users.Find(Session["UserId"]);
            return View(profile);
        }

        [HttpPost]
        public ActionResult Profile(User user, string country_code, string description, HttpPostedFileBase profile_image, HttpPostedFileBase background_image)
        {
            if (user.Name != null && user.Surname != null )
            {
                User kullanici = dbBaglantisi.Users.Find(Session["UserId"]);
                
                kullanici.Surname = user.Surname;
                kullanici.Company = user.Company;
                kullanici.Address = user.Address;
                kullanici.City = user.City;
                kullanici.Country = country_code;
                kullanici.Email = user.Email;
                kullanici.PhoneNumber = user.PhoneNumber;
                kullanici.CompanyNo = user.CompanyNo;
                kullanici.ProfileDescription = description;



//                using (var sha256 = SHA256.Create())
//                {
//                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
//                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
//
//                    kullanici.Password = hash;
//                }

               
                dbBaglantisi.SaveChanges();
                var usr = dbBaglantisi.Users.Find(Session["UserId"]);

                if (profile_image != null)
                {
                    string filePath = Path.Combine(Server.MapPath("~/Project_Icon"), Guid.NewGuid().ToString() + "_" + Path.GetFileName(profile_image.FileName));
                    profile_image.SaveAs(filePath);
                    kullanici.Avatar = "/Project_Icon/" + profile_image.FileName;
                }

                if (background_image != null)
                {
                    string filePath = Path.Combine(Server.MapPath("~/Project_Icon"), Guid.NewGuid().ToString() + "_" + Path.GetFileName(background_image.FileName));
                    background_image.SaveAs(filePath);
                    kullanici.BackgroundImage = "/Project_Icon/" + background_image.FileName;
                }
                dbBaglantisi.SaveChanges();

                if (usr != null)
                {
                    Session["Name"] = usr.Name;
                    Session["LastName"] = usr.Surname;

                    return RedirectToAction("Profile","User");
                }
            }

           


            return View();
        }
    }
}