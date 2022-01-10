using System;
using System.ComponentModel.DataAnnotations;

namespace ThingsLine.Models
{
    public class U_Profile
    {
        [Display(Name = "ユーザーID")]
        public string userID { get; set; } = null;
        [Display(Name = "苗字")]
        public string name1 { get; set; } = null;
        [Display(Name = "名前")]
        public string name2 { get; set; } = null;
        [Display(Name = "ニックネーム")]
        public string nickname { get; set; } = null;
        [Display(Name = "E-Mail")]
        public string Email { get; set; } = null;
        [Display(Name = "権限")]
        public int role { get; set; } = 0;
        [Display(Name = "利用状況")]
        public int stopFLG { get; set; } = 0;
    }


    public class U_MessageTask
    {
        public DateTime dt { get; set; }
        public string imsi { get; set; } = null;
        public string userID { get; set; } = null;
        public int msgType { get; set; } = 0;
        public int msgCode { get; set; } = 0;
        public int msgCount { get; set; } = 0;
        public Double fdata01 { get; set; } = 0;
        public Double fdata02 { get; set; } = 0;
        public Double fdata03 { get; set; } = 0;
        public Double fdata04 { get; set; } = 0;
        public int stopFLG { get; set; } = 0;
    }

    public class U_Setting
    {
        public string userID { get; set; } = null;
        public int type01 { get; set; } = 0;
        public int type02 { get; set; } = 0;
        public int valueInt01 { get; set; } = 0;
        public int valueInt02 { get; set; } = 0;
        public Double valueflt01 { get; set; } = 0;
        public Double valueflt02 { get; set; } = 0;
        public int stopFLG { get; set; } = 0;
    }


    public class S_SystemMessage
    {
        public DateTime dt { get; set; }
        public string type { get; set; }
        public string s_namespace { get; set; }
        public string s_member { get; set; }
        public string Exc { get; set; }
        public string comment01 { get; set; }
        public string comment02 { get; set; }
        public string comment03 { get; set; }
    }
    public class MdlThingsLine
    {

        public class AspNetUsers
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
        }

    }
    public class BikeDeviceBaseData
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public string userID { get; set; }
        public string imsi { get; set; }
        public string bikeName { get; set; }
        public string deviceType { get; set; }
        public BikeDeviceBaseData()
        {
        }
    }

    public class UserBaseData
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public string LINEID { get; set; }
        public string imsi { get; set; }
        public string userID { get; set; }
    }
    public class retdataString
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public string data { get; set; }
    }
    public class retdataInt
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public int data { get; set; }
    }
    public class AspNetUserLogins
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }
    }
    public class MNameList
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public string Code { get; set; }
        public string Codetext { get; set; }
    }
    public class MName
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public string Codetext { get; set; }
    }
    public class DeviceMode
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public string imsi { get; set; }
        public int eventType { get; set; }
        public int eventCode { get; set; }
        public double fdata01 { get; set; }
        public double fdata02 { get; set; }
        public double fdata03 { get; set; }
        public double fdata04 { get; set; }
        public DeviceMode()
        {
        }
    }
    public class DeviceTheftMonitoringMode
    {
        //   UDevice.userID, UDevice.imsi,UDevice.imei"
        public double fdata01 { get; set; }
        public double fdata02 { get; set; }
        public double fdata03 { get; set; }
        public double fdata04 { get; set; }
        public DeviceTheftMonitoringMode()
        {
        }
    }
}
