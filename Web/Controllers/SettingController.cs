using System;
using System.Web.Mvc;
using thingslineWeb.Models;
using thingslineWeb.Modules;
using Microsoft.AspNet.Identity;
using ThingsLine.Models;
using ThingsLine.Modules;
using System.Collections.Generic;

namespace thingslineWeb.Controllers
{
    public class SettingController : Controller
    {
        //===================================
        //初期表示
        //----------------------------------------------------------------
        public ActionResult Index()
        {
            mdlSetting mSetting = new mdlSetting();
            modMName mMName = new modMName();
            modUProfile mUProfile = new modUProfile();
            SettingUserDataModel retSettingUserDataModel = null;

            //ログインユーザー情報を取得＆セット

            //----------------------------------------------------------------
            //情報取得
            //ログインユーザー情報
            retSettingUserDataModel.LoginUserInfo = mUProfile.getUProfile(User.Identity.GetUserId());

            //retSettingUserDataModel = mSetting.GetSettingUserData(User.Identity.GetUserId());

            //バイク情報


            //デバイス情報


            List<MNameList> retMName = mMName.getListMName("User", "UserRole");
            //retSettingUserDataModel.MNameList_UserRole = retMName;

            //----------------------------------------------------------------
            //MSG
            //retSettingUserDataModel.infoMSG = "";

            //----------------------------------------------------------------
            //データ引き渡し
            ViewBag.retSettingUserDataModel = retSettingUserDataModel;
            return View();
        }

        //===================================
        // GET: Setting6
        //----------------------------------------------------------------
        [HttpPost]
        public ActionResult Index(SettingUserDataModel getSettingUserDataModel)
        {

            mdlSetting mSetting = new mdlSetting();
            modMName mMName = new modMName();
            modUProfile mUProfile = new modUProfile();
            SettingUserDataModel retSettingUserDataModel = null;

            //ログインユーザー情報を取得＆セット

            //情報取得
            U_Profile retU_Profile = mUProfile.getUProfile(User.Identity.GetUserId());
            retSettingUserDataModel.LoginUserInfo = retU_Profile;





            List<MNameList> retMName = mMName.getListMName("User", "UserRole");
            retSettingUserDataModel.MNameList_UserRole = retMName;

            //MSG
            retSettingUserDataModel.infoMSG = "";

            //データ引き渡し
            ViewBag.retSettingUserDataModel = retSettingUserDataModel;
            return View();
        }
        //===================================
        //初期表示
        // GET: Setting
        //----------------------------------------------------------------
        public ActionResult Menu()
        {

            mdlSetting mSetting = new mdlSetting();
            modMName mMName = new modMName();
            modUProfile mUProfile = new modUProfile();
            SettingUserDataModel retSettingUserDataModel = null;
            //ログインユーザー情報を取得＆セット

            //情報取得
            U_Profile retU_Profile = mUProfile.getUProfile(User.Identity.GetUserId());
            retSettingUserDataModel.LoginUserInfo = retU_Profile;

            List<MNameList> retMName = mMName.getListMName("User", "UserRole");
            retSettingUserDataModel.MNameList_UserRole = retMName;

            //MSG

            retSettingUserDataModel.infoMSG = "";

            //データ引き渡し
            ViewBag.retSettingUserDataModel = retSettingUserDataModel;
            return View();
        }
        //===================================
        //初期表示
        // GET: Setting
        //----------------------------------------------------------------
        public ActionResult LoginUserInfo()
        {
            mdlSetting mSetting = new mdlSetting();
            modMName mMName = new modMName();
            modUProfile mUProfile = new modUProfile();
            SettingUserDataModel retSettingUserDataModel = null;

            //ログインユーザー情報を取得＆セット

            //情報取得
            U_Profile retU_Profile = mUProfile.getUProfile(User.Identity.GetUserId());
            retSettingUserDataModel.LoginUserInfo = retU_Profile;

            List<MNameList> retMName = mMName.getListMName("User", "UserRole");
            retSettingUserDataModel.MNameList_UserRole = retMName;

            //MSG

            retSettingUserDataModel.infoMSG = "";

            //データ引き渡し
            ViewBag.retSettingUserDataModel = retSettingUserDataModel;
            return View();
        }
        //===================================
        // GET: Setting6
        //----------------------------------------------------------------
        [HttpPost]
        public ActionResult LoginUserInfo(SettingUserDataModel getSettingUserDataModel)
        {

            mdlSetting mSetting = new mdlSetting();
            modMName mMName = new modMName();
            modUProfile mUProfile = new modUProfile();
            SettingUserDataModel retSettingUserDataModel = null;

            //ログインユーザー情報を取得＆セット

            //情報取得
            U_Profile retU_Profile = mUProfile.getUProfile(User.Identity.GetUserId());
            retSettingUserDataModel.LoginUserInfo = retU_Profile;

            List<MNameList> retMName = mMName.getListMName("User", "UserRole");
            retSettingUserDataModel.MNameList_UserRole = retMName;

            //MSG
            retSettingUserDataModel.infoMSG = "";

            //データ引き渡し
            ViewBag.retSettingUserDataModel = retSettingUserDataModel;
            return View();
        }
        //===================================
        //初期表示
        // GET: Setting
        //----------------------------------------------------------------
        public ActionResult UserInfo()
        {
            mdlSetting mSetting = new mdlSetting();
            modMName mMName = new modMName();
            modUProfile mUProfile = new modUProfile();
            SettingUserDataModel retSettingUserDataModel = null;

            //ログインユーザー情報を取得＆セット

            //情報取得
            U_Profile retU_Profile = mUProfile.getUProfile(User.Identity.GetUserId());
            retSettingUserDataModel.LoginUserInfo = retU_Profile;

            List<MNameList> retMName = mMName.getListMName("User", "UserRole");
            retSettingUserDataModel.MNameList_UserRole = retMName;

            //MSG

            retSettingUserDataModel.infoMSG = "";

            //データ引き渡し
            ViewBag.retSettingUserDataModel = retSettingUserDataModel;
            return View();
        }
        //===================================
        // GET: Setting6
        //----------------------------------------------------------------
        [HttpPost]
        public ActionResult UserInfo(SettingUserDataModel getSettingUserDataModel)
        {
            mdlSetting mSetting = new mdlSetting();
            modMName mMName = new modMName();
            modUProfile mUProfile = new modUProfile();
            SettingUserDataModel retSettingUserDataModel = null;

            //ログインユーザー情報を取得＆セット

            //情報取得
            U_Profile retU_Profile = mUProfile.getUProfile(User.Identity.GetUserId());
            retSettingUserDataModel.LoginUserInfo = retU_Profile;

            List<MNameList> retMName = mMName.getListMName("User", "UserRole");
            retSettingUserDataModel.MNameList_UserRole = retMName;

            //MSG
            retSettingUserDataModel.infoMSG = "";

            //データ引き渡し
            ViewBag.retSettingUserDataModel = retSettingUserDataModel;
            return View();
        }


    }
}