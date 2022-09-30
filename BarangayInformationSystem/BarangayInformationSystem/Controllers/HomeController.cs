using BarangayInformationSystem.App_Data;
using BarangayInformationSystem.Models;
using BarangayInformationSystem.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BarangayInformationSystem.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Login", new {@ReturnUrl = "Index" });

            ViewModel model = new ViewModel();
            model.user_detail = (user_detail)Session["user_detail"];

            return View(model);
        }

        public ActionResult Login(String ReturnUrl = "")
        {
            if (!String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Index");
            ViewModel model = new ViewModel();
            model.user = new user();
            model.response = new Response();
            Session["ReturnUrl"] = ReturnUrl;

            return View(model);
        }
        [HttpPost]
        public ActionResult Login(String isRemember, user user)
        {
            ViewModel model = new ViewModel();
            model.user = new user();
            model.response = new Response();
            var isRememberMe = String.IsNullOrEmpty(isRemember) ? false : true;

            var userExist = db.user.Where(m => m.user_email.Equals(user.user_email)).FirstOrDefault();
            if (userExist != null)
            {
                if (userExist.user_is_verified == (int)BOOL.TRUE)
                {
                    Session["user_detail"] = db.user_detail.Where(m => m.user_detail_id == userExist.user_id).FirstOrDefault();
                    Session["user_id"] = userExist.user_id;

                    if (isRememberMe)
                        FormsAuthentication.SetAuthCookie(user.user_email, isRememberMe);

                    var password = DecryptString(userExist.user_password);

                    if (password.Equals(user.user_password))
                    {
                        if (!String.IsNullOrEmpty(Session["ReturnUrl"] as string))
                        {
                            var url = (Session["ReturnUrl"] as string);

                            return RedirectToAction(url);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        model.response.code = ERROR_CODE.ERROR;
                        model.response.message = Literals.PASSWORD_INCORRECT;
                    }
                }
                else
                {
                    model.response.code = ERROR_CODE.ERROR;
                    model.response.message = Literals.EMAIL_NOT_VERIFIED;
                }
            }
            else
            {
                model.response.code = ERROR_CODE.ERROR;
                model.response.message = Literals.EMAIL_NOT_REGISTERED;
            }

            return View(model);
        }

        public ActionResult Register()
        {
            if (!String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Index");
            ViewModel model = new ViewModel();
            model.user = new user();
            model.response = new Response();

            return View(model);
        }
        [HttpPost]
        public ActionResult Register(user user, String password2)
        {
            if (!String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Index");
            ViewModel model = new ViewModel();
            model.user = new user();
            model.response = new Response();

            if (password2.Equals(user.user_password))
            {
                var isEmailExist = db.user.Where(m => m.user_email == user.user_email).FirstOrDefault();
                if (isEmailExist == null)
                {
                    user.user_id = getUserId();
                    user.user_password = EncryptString(password2);
                    user.user_access_code = getUserAccessCode();
                    user.user_is_verified = (int)BOOL.FALSE;
                    user.user_status = (int)STATUS.notverified;
                    user.user_login_type = (int)LOGIN_OPTION.web;
                    db.user.Add(user);
                    db.SaveChanges();

                    var userDetail = new user_detail();
                    userDetail.user_detail_id = user.user_id;
                    db.user_detail.Add(userDetail);
                    db.SaveChanges();

                    model.response.code = ERROR_CODE.SUCCESS;
                    model.response.message = Literals.ACCOUNT_CREATED;
                    //Session["user_id"] = user.user_id;
                    Session["user_id_verification"] = user.user_id;
                    Session["user_detail"] = db.user_detail.Where(m => m.user_detail_id == user.user_id).FirstOrDefault();

                    //Send Email Confirmation
                    String subject = "OPAO(BIS) - Account Validation";
                    String body = GenerateVerificationMessage(user.user_access_code);
                    String recipient = user.user_email;

                    Response res = SendEmail(recipient, body, subject);

                    if (res.code == ERROR_CODE.ERROR)
                        return Content(res.message);

                    return RedirectToAction("Verification");

                }
                else
                {
                    model.response.code = ERROR_CODE.ERROR;
                    model.response.message = Literals.EMAIL_ALREADY_EXIST;
                }

               
            }
            else
            {
                model.response.code = ERROR_CODE.ERROR;
                model.response.message = Literals.PASSWORD_NOT_MATCH;
            }

            model.user = new user();
            model.user_detail = new user_detail();

            return View(model);
        }

        public ActionResult Verification()
        {
            ViewModel model = new ViewModel();
            model.user = new user();
            model.response = new Response();
            // Check User
            if (String.IsNullOrEmpty(Session["user_id_verification"] as String)) return RedirectToAction("Register");

            var userId = (String)Session["user_id_verification"];
            var userAccount = db.user.Where(m => m.user_id.Equals(userId)).FirstOrDefault();
            if (userAccount.user_is_verified == (int)BOOL.TRUE) return RedirectToAction("Index");

            //
            Session["userAcc"] = userAccount;



            return View(model);
        }
        [HttpPost]
        public ActionResult Verification(String code)
        {
            ViewModel model = new ViewModel();
            model.user = new user();
            model.response = new Response();

            var user =  (user)Session["userAcc"];
            if (user.user_access_code.Equals(code))
            {
                var userVerified = db.user.Find(user.user_auto_id);
                userVerified.user_is_verified = (int)BOOL.TRUE;
                userVerified.user_status = (int)STATUS.active;
                db.SaveChanges();

                //Success
                return RedirectToAction("Index");
            }
            else
            {
                //Failed
                model.response.code = ERROR_CODE.ERROR;
                model.response.message = Literals.CODE_INVALID;
            }
            
            return View(model);
        }

        public ActionResult MyProfile()
        {
            if (String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Login", new { @ReturnUrl = "MyProfile" });

            ViewModel model = new ViewModel();
            var userDetailSession = (user_detail)Session["user_detail"];

            model.user_detail = db.user_detail.Find(userDetailSession.user_detail_auto_id);
            model.civil_status_option = db.civil_status.ToList();
            model.gender_option = db.gender.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult MyProfile(user_detail user_detail, String action)
        {
            if (String.IsNullOrEmpty(Session["user_id"] as String)) return RedirectToAction("Login", new { @ReturnUrl = "MyProfile" });

            switch(action)
            {
                case "profile":

                    break;
                case "account":

                    break;
            }

            ViewModel model = new ViewModel();
            var userDetailSession = (user_detail)Session["user_detail"];

            model.user_detail = db.user_detail.Find(userDetailSession.user_detail_auto_id);
            model.civil_status_option = db.civil_status.ToList();
            model.gender_option = db.gender.ToList();
            return View(model);
        }
        public ActionResult Logout()
        {
            //Reset
            Session.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }

        public ActionResult GetBarangay()
        {
            //var web = new WebClient();
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //var url = "https://psgc.gitlab.io/api/cities/072230000/barangays/";
            //var responseString = web.DownloadString(url);
            ////var client = new RestClient("https://psgc.gitlab.io/api/cities/072230000/barangays/");
            ////client.Timeout = -1;
            ////var request = new RestRequest(Method.GET);
            ////IRestResponse response = client.Execute(request);
            ////Console.WriteLine(response.Content);

            var strResult = ApiUrl.getPSGCaPI(ApiUrl.Url_CityBarangay);
            var result = JsonConvert.DeserializeObject<List<ResponseBody>>(strResult);

            return Content("SUcces");
        }

        public ActionResult Survey()
        {
            return View();
        }

        public JsonResult AddUser(List<user_detail> memberList)
        {
            List<String> listOfIds = new List<string>();

            try
            {
                foreach (var userDetail in memberList)
                {
                    var code = getUserAccessCode();
                    var user = new user();
                    user.user_id = getUserId();
                    user.user_email = userDetail.user_email;
                    user.user_name = userDetail.user_username;
                    user.user_password = EncryptString(code);
                    user.user_access_code = code;
                    user.user_is_verified = (int)BOOL.TRUE;
                    user.user_status = (int)STATUS.active;
                    user.user_login_type = (int)LOGIN_OPTION.web;
                    db.user.Add(user);
                    db.SaveChanges();
                    //
                    listOfIds.Add(user.user_id);

                    userDetail.user_detail_id = user.user_id;
                    db.user_detail.Add(userDetail);
                    db.SaveChanges();

                    //Send Email Confirmation
                    String subject = "OPAO(BIS) - Account Registration";
                    String body = GenerateWelcomeMessage(user, userDetail);
                    String recipient = user.user_email;

                    Response res = SendEmail(recipient, body, subject);
                }
            }
            catch (Exception ex)
            {

            }
            
            return Json(listOfIds.Count); 
        }
    }
}