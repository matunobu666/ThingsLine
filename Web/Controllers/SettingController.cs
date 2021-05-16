using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using thingslineWeb.Models;
using thingslineWeb.Modules;
using Microsoft.AspNet.Identity;

namespace thingslineWeb.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult Index()
        {

            mdlSetting mSetting = new mdlSetting();
            SettingUserDataModel getSettingUserDataModel = new SettingUserDataModel();

            //key情報をセット
            getSettingUserDataModel.UserID = User.Identity.GetUserId();

            //情報取得
            SettingUserDataModel retSettingUserDataModel = mSetting.getSettingUserInfo(getSettingUserDataModel);


            ViewBag.retSettingUserDataModel = retSettingUserDataModel;

            return View(retSettingUserDataModel);
        }

        // GET: Setting6
        [HttpPost]
        public ActionResult Index(SettingUserDataModel getSettingUserDataModel)
        {

            mdlSetting mSetting = new mdlSetting();

            //key情報をセット
            getSettingUserDataModel.UserID = User.Identity.GetUserId();

            //情報取得
            SettingUserDataModel retSettingUserDataModel = mSetting.getSettingUserInfo(getSettingUserDataModel);


            ViewBag.retSettingUserDataModel = retSettingUserDataModel;

            return View(retSettingUserDataModel);
        }




    }
}