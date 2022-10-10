using BarangayInformationSystem.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarangayInformationSystem.Models
{
    public class ViewModel
    {
        public Response response { get; set; }
        public user user { get; set; }
        public user_detail user_detail { get; set; }
        public UserInfo UserInfo { get; set; }
        public purok purok { get; set; }
        public sitio sitio { get; set; }
        public Request request { get; set; }
        public List<user> users { get; set; }
        public List<user_detail> user_details { get; set; }
        public List<gender> gender_option { get; set; }
        public List<civil_status> civil_status_option { get; set; }
        public List<UserInfo> UserInfos { get; set; }
        public List<purok> puroks { get; set; }
        public List<sitio> sitios { get; set; }
        public List<Request> requests { get; set; }
    }

    
}