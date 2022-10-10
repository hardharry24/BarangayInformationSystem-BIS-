using BarangayInformationSystem.App_Data;
using BarangayInformationSystem.Models;
using BarangayInformationSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarangayInformationSystem.Controllers
{
    public class AdminController : BaseController   
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            if (String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Login","Home");

            return View();
        }

        public ActionResult Users()
        {
            if (String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Login", "Home");

            var model = new ViewModel();

            //Populate data
            var userInfos = new List<UserInfo>();
            foreach (var user in db.user)
            {
                var userInfo = new UserInfo();
                userInfo.user = user;
                userInfo.user_detail = db.user_detail.Where(m => m.user_detail_id == user.user_id).FirstOrDefault();
                userInfos.Add(userInfo);
                
            }
            model.UserInfos = userInfos;
            return View(model);
        }
        public PartialViewResult UserInfo(int id)
        {
            var model = new ViewModel();
            var userInfo = db.user.Find(id);
            var user_detail = db.user_detail.Where(m => m.user_detail_id == userInfo.user_id).FirstOrDefault();
            model.UserInfo = new UserInfo() { user = userInfo, user_detail = user_detail };

            return PartialView(model);
        }

        public ActionResult Purok()
        {
            if (String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Login", "Home");

            var model = new ViewModel();
            model.puroks = db.purok.ToList();

            return View(model);
        }
        public JsonResult AddPurok(purok purok)
        {
            var response = new Response();

            var ifExistNo = db.purok.Where(m => m.purok_no == purok.purok_no).FirstOrDefault();
            if (ifExistNo == null)
            {
                db.purok.Add(purok);
                db.SaveChanges();
                response.code = ERROR_CODE.SUCCESS;
                response.message = "Purok Detail Added!";
            }
            else
            {
                response.code = ERROR_CODE.ERROR;
                response.message = "Purok # Already Exist!"; 
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemovePurok(int id)
        {
            var purok = db.purok.Where(m => m.purok_id == id).FirstOrDefault();
            db.purok.Remove(purok);
            db.SaveChanges();

            return RedirectToAction("Purok");
        }
        public ActionResult RemoveSitio(int id)
        {
            var sitio = db.sitio.Where(m => m.sitio_id == id).FirstOrDefault();
            db.sitio.Remove(sitio);
            db.SaveChanges();

            return RedirectToAction("Sitio");
        }

        public JsonResult AddSitio(sitio sitio)
        {
            var response = new Response();

            var ifExistNo = db.sitio.Where(m => m.sitio_name == sitio.sitio_name).FirstOrDefault();
            if (ifExistNo == null)
            {
                db.sitio.Add(sitio);
                db.SaveChanges();
                response.code = ERROR_CODE.SUCCESS;
                response.message = "Sitio Added!";
            }
            else
            {
                response.code = ERROR_CODE.ERROR;
                response.message = "Sitio Name Already Exist!";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Sitio()
        {
            if (String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Login", "Home");

            var model = new ViewModel();
            model.sitios = db.sitio.ToList();

            return View(model);
        }

        public ActionResult Password(int id)
        {
            if (String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Login", "Home");

            var user = db.user.Where(m => m.user_auto_id == id).FirstOrDefault();
            var password = DecryptString(user.user_password);

            return Content(password);
        }

        public ActionResult _Request()
        {
            var model = new ViewModel();
            model.requests = db.Request.ToList();


            return View(model);
        }
    }
}