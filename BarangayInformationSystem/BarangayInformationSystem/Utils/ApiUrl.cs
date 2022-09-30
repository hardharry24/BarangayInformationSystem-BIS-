using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BarangayInformationSystem.Utils
{
    public class ApiUrl
    {
        public const string CityCode = "072230000";//Mandaue Code
        public const string Url_CityBarangay = "https://psgc.gitlab.io/api/cities/" + CityCode + "/barangays.json";
        public const string Url_Barangay = "https://psgc.gitlab.io/api/barangays/072230019.json";


        public static string getPSGCaPI(String url)
        {
            string jsonResult = String.Empty;

            var web = new WebClient();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            jsonResult = web.DownloadString(url);

            return jsonResult;
        }
    }

    //Cless for the response
    public class ResponseBody
    {
        public String code { get; set; }
        public String name { get; set; }
        public String oldName { get; set; }
        public bool subMunicipalityCode { get; set; }
        public bool districtCode { get; set; }
        public String provinceCode { get; set; }
        public String regionCode { get; set; }
        public String islandGroupCode { get; set; }
        public String psgc10DigitCode { get; set; }
    }
}