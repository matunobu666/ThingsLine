using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace thingslineWeb.Models
{


    public class SettingUserDataModel
    {
        [Display(Name = "ユーザーID")]
        public string UserID { get; set; }


        [Display(Name = "ユーザー")]
        public IEnumerable<SettingUserInfoModel> SettingUserInfoModel { get; set; }

        [Display(Name = "バイク")]
        public IEnumerable<SettingBikeInfoModel> SettingBiKeInfoModel { get; set; }

        [Display(Name = "デバイス")]
    public IEnumerable<SettingDeviceInfoModel> SettingDeviceInfoModel { get; set; }

    }




    //-----------------------------
    //ユーザー情報総合
    /*
                        + " userID"
                    + ",name1"
                    + ",name2"
                    + ",nickname"
                    + ",Email"
                    + ",role"
                    + ",stopFLG"
                     );
    */
    public class SettingUserInfoModel
    {
        [Display(Name = "ユーザーID")]
        public string userID { get; set; }

        [Display(Name = "苗字")]
        public string name1 { get; set; }

        [Display(Name = "名前")]
        public string name2 { get; set; }

        [Display(Name = "ニックネーム")]
        public string nickname { get; set; }

        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Display(Name = "権限")]
        public string role { get; set; }

        [Display(Name = "利用状況")]
        public string stopFLG { get; set; }

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