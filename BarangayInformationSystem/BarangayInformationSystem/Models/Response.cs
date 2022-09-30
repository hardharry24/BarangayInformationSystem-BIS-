using BarangayInformationSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarangayInformationSystem.Models
{
    public class Response
    {
        public ERROR_CODE code { get; set; }
        public String message { get; set; }
    }
}