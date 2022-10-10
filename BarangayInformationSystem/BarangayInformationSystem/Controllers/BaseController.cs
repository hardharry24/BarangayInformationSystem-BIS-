using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BarangayInformationSystem.Utils;
using BarangayInformationSystem.App_Data;
using BarangayInformationSystem.Models;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BarangayInformationSystem.Controllers
{
    public class BaseController : Controller
    {
        public db_bisEntities db;

        public BaseController()
        {
            // Get instance of entity
            if (db == null)
                db = new db_bisEntities();
        }
    
        // GET: Base
        [HttpGet]

        public ActionResult LoginOption(LOGIN_OPTION type, String name, String userId)
        {
            var response = new Response();
            var user = new user();
            switch (type)
            {
                case LOGIN_OPTION.fb:
                    user.user_id = userId;
                    user.user_status = (Int32)STATUS.active;
                    user.user_login_type = (Int32)type;

                    var IsExist = db.user.Where(m => m.user_id == userId).FirstOrDefault();
                    if (IsExist == null)
                    {
                        db.user.Add(user);
                        db.SaveChanges();

                        var userdetail = new user_detail();
                        userdetail.user_detail_id = userId;

                        db.user_detail.Add(userdetail);
                        db.SaveChanges();
                    }

                    var currUser = db.user_detail.Where(m => m.user_detail_id == userId).FirstOrDefault();

                    Session["user_detail"] = currUser;
                    Session["user_id"] = userId;
                    response.code = ERROR_CODE.SUCCESS;
                    response.message = "/Home/Index";

                    break;
                case LOGIN_OPTION.google:

                    break;
                default:
                    break;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public String getUserId()
        {
            int initial = 0;
            String result = String.Empty;
            var dbUserCount = db.user.Where(m => m.user_login_type == (Int32)LOGIN_OPTION.web).ToList().LastOrDefault();
            if (dbUserCount == null)
            {
                initial = 1;
                result = String.Format("WebApp-{0}", initial);
            }
            else
            {
                var splitValue = dbUserCount.user_id.Split('-');
                initial = Convert.ToInt32( splitValue[1] ) + 1;
                result = String.Format("WebApp-{0}", initial);
            }

            return result;
        }

        public String getUserAccessCode()
        {
            var rnd = new Random(DateTime.Now.Millisecond);

            return rnd.Next(1000, 9999).ToString(); ;
        }

        public String GenerateVerificationMessage(String code)
        {
            String resStr = String.Empty;

            resStr = "<h2>Dear Opaoanon, </h2> </br>" +
                "<p>In order to continue your registrations with Barangay Opao Information System (BIS) </p>" +
                "</br>"+
                "<h4>Please use the following validation code: </h4>" +
                "</br>" +
                "<h1>" + code + "</h1> \n" +
                "</br>" +
                "<h5>Regards,</h5>" +
                "</br>"+
                "BIS-OPAO Management </br>";

            return resStr;
        }
        public String GenerateWelcomeMessage(user user, user_detail userdetail)
        {
            String resStr = String.Empty;

            resStr = "<h2>Dear Opaoanon, </h2> </br>" +
                "<p>Welcome to Barangay Opao Information System (BIS) </p>" +
                "</br>" +
                "<h4>Please use the following information : </h4>" +
                "</br>" +
                "Username : <h1>" + userdetail.user_username + "</h1> \n" +
                "Email : <h1>" + userdetail.user_email + "</h1> \n" +
                "Password : <h1>" + user.user_access_code + "</h1> \n" +
                "</br>" +
                "<h5>Regards,</h5>" +
                "</br>" +
                "BIS-OPAO Management </br>";

            return resStr;
        }

        public Response SendEmail(String TO, String BODY, String SUBJECT)
        {
            Response resp = new Response();
            try
            {
                string from = UTILITIES.EMAIL_MAILADDRESS; //From address    
                MailMessage message = new MailMessage(from, TO);

                string mailbody = BODY;//"In this article you will learn how to send a email using Asp.Net & C#";
                message.Subject = SUBJECT;//"Sending Email Using Asp.Net & C#";
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 25); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(UTILITIES.EMAIL_MAILADDRESS, UTILITIES.EMAIL_PASSWORD);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;

                client.Send(message);
                resp.code = ERROR_CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                resp.code = ERROR_CODE.ERROR;
                resp.message = ex.ToString();
            }
            return resp;
        }

        public string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;
            string key = UTILITIES.KEY_ENCRYPTION;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            string key = UTILITIES.KEY_ENCRYPTION;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public JsonResult ValidateDataSurvey(SurveyValidation model)
        {
            var response = new Response();


            var isExist = db.user_detail.Where(m => m.user_fname == model.firstname && m.user_mname == model.mi && m.user_lname == model.lastname).FirstOrDefault();
            if (isExist == null)
                response.code = ERROR_CODE.SUCCESS;
            else
            {
                response.code = ERROR_CODE.ERROR;
                response.message = "Name Already Exist!";

                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //isExist = db.user_detail.Where(m => m.user_email == model.email).FirstOrDefault();
            //if (isExist == null)
            //    response.code = ERROR_CODE.SUCCESS;
            //else
            //{
            //    response.code = ERROR_CODE.ERROR;
            //    response.message = "Email Already Exist!";

            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            isExist = db.user_detail.Where(m => m.user_username == model.username).FirstOrDefault();
            if (isExist == null)
            {
                response.code = ERROR_CODE.SUCCESS;
            }
            else
            {
                response.code = ERROR_CODE.ERROR;
                response.message = "Username Already Exist!";

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getListPurok()
        {
            return Json(db.purok.Select(x => new
            {
                id = x.purok_id,
                purok_name = x.purok_name,
                purok_leader = x.purok_leader
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getListSitio()
        {
            return Json(db.sitio.Select(x => new
            {
                id = x.sitio_id,
                sitio_name = x.sitio_name
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public String GetRequestType(REQUEST_TYPE type)
        {
            if (type == REQUEST_TYPE.BRGYCERTIFICATE)
                return REQUEST_TYPE_DETAIL.BRGYCERTIFICATE;
            else if (type == REQUEST_TYPE.CERTIFICATION)
                return REQUEST_TYPE_DETAIL.CERTIFICATION;
            return String.Empty;
        }
    }

}