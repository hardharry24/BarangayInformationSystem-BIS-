using BarangayInformationSystem.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarangayInformationSystem.Models
{
    public class SurveyValidation
    {
        public String firstname { get; set; }
        public String mi { get; set; }
        public String lastname { get; set; }

        public String email { get; set; }
        public String username { get; set; }
    }
    public class UserInfo
    {
        public user user { get; set; }
        public user_detail user_detail { get; set; }
    }
}