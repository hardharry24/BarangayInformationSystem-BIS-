//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BarangayInformationSystem.App_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        public int user_auto_id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
        public Nullable<int> user_login_type { get; set; }
        public string user_access_code { get; set; }
        public Nullable<int> user_is_verified { get; set; }
        public Nullable<int> user_status { get; set; }
    }
}