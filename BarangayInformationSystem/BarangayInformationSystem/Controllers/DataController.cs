using BarangayInformationSystem.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarangayInformationSystem.Controllers
{
    public class DataController : BaseController
    {
        // GET: Data
        public JsonResult ListUserDatails()
        {
            var userList = db.user_detail.ToList();
            var obj = new
            {
                total = userList.Count,
                totalNotFiltered = userList.Count,
                rows = userList
            };
     
            
            return Json(obj,JsonRequestBehavior.AllowGet);
        }
    }
}