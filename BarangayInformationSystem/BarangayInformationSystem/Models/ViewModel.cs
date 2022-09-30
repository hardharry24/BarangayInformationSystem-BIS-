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
        public List<gender> gender_option { get; set; }
        public List<civil_status> civil_status_option { get; set; }
        
    }

    
}