using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ThingsLine.Models;
using ThingsLine.Modules;

namespace thingslineWeb.Models
{


    public class SettingUserDataModel
    {

        public U_Profile LoginUserInfo { get; set; }

        public IList<SettingBikeInfoModel> BiKeInfo { get; set; }

        public IList<SettingDeviceInfoModel> DeviceInfo { get; set; }

        public IList<MNameList> MNameList_UserRole { get; set; }

        public string infoMSG { get; set; }
    }


    //-----------------------------
    //バイク情報(LIST
    public class SettingBikeInfoModel
    {
        [Display(Name = "車両ニックネーム")]
        public string nickname { get; set; }

        [Display(Name = "車両名")]
        public string name { get; set; }

        [Display(Name = "紐付け端末")]
        public string LINEname { get; set; }

    }


    //-----------------------------
    //端末情報(LIST
    public class SettingDeviceInfoModel
    {
        [Display(Name = "デバイス名")]
        public string Devicename { get; set; }

        [Display(Name = "デバイス種別")]
        public string DeviceType { get; set; }

        [Display(Name = "IMSI")]
        public string DeviceIMSI { get; set; }

        [Display(Name = "IMEI")]
        public string DeviceIMEI { get; set; }

    }


}