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
    
    public partial class Request
    {
        public int requestId { get; set; }
        public string requestType { get; set; }
        public string requestedBy { get; set; }
        public string requestStatus { get; set; }
        public string requestApprovedby { get; set; }
        public string requestFile { get; set; }
        public Nullable<System.DateTime> requestDate { get; set; }
    }
}