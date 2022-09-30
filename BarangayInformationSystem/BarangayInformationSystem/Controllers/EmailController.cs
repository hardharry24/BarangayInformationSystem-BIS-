using BarangayInformationSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BarangayInformationSystem.Controllers
{
    public class EmailController : Controller
    {
        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(String msgTo, String msgBody, String msgSubject)
        {
            String res = "";
            //Session[""]
            string to = msgTo; //To address    
            string from = UTILITIES.EMAIL_MAILADDRESS; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = msgBody;//"In this article you will learn how to send a email using Asp.Net & C#";
            message.Subject = msgSubject;//"Sending Email Using Asp.Net & C#";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 25); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(UTILITIES.EMAIL_MAILADDRESS, UTILITIES.EMAIL_PASSWORD);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
                res = "Succes";
            }

            catch (Exception ex)
            {
                res = ex.Data.ToString();
            }
            return Content(res);
        }
    }
}