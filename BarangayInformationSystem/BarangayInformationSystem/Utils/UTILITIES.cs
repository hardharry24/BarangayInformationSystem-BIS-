using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarangayInformationSystem.Utils
{
    public class UTILITIES
    {
        public const string EMAIL_PASSWORD = "lvqjblrjjdupgtrm";
        public const string EMAIL_MAILADDRESS = "noreplybrgyopao@gmail.com";

        public const string KEY_ENCRYPTION = "b14ca5898a4e4133bbce2ea2315a1916";

    }

    public enum LOGIN_OPTION
    {
        fb = 0,
        google = 1,
        web = 2
    }
    public enum STATUS
    {
        inaactive = 0,
        active = 1,
        notverified = 2
    }
    public enum ERROR_CODE
    {
        ERROR = 0,
        SUCCESS = 1
    }

    public enum BOOL
    {
        FALSE = 0,
        TRUE = 1
    }
    public enum USER_TYPE
    {
        USER = 0,
        ADMIN = 1
    }
    public enum REQUEST_TYPE
    {
        BRGYCERTIFICATE = 0,
        CERTIFICATION = 1
    }
    public class REQUEST_TYPE_DETAIL
    {
        public const string BRGYCERTIFICATE = "BRGY CERTIFICATE";
        public const string CERTIFICATION = "CERTIFICATION";
    }
    public class REQUEST_STATUS
    {
        public const string REJECTED = "REJECTED";
        public const string PENDING = "PENDING";
        public const string APPROVED = "PENDING";
    }
}