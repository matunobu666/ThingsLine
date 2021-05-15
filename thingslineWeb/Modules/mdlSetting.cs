using System;
using System.IO;
using System.Text;
using System.Data.SqlClient;


using thingslineWeb.Modules;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Configuration;

using System.Globalization;
using System.Xml.Serialization;
using System.Linq;
using System.Net.Http.Json;
using thingslineWeb.Models;
using module;

namespace thingslineWeb.Modules
{
    public class mdlSetting
    {


        public SettingUserDataModel getSettingUserInfo(SettingUserDataModel getSettingUserDataModel)
        {
            module.mdlSQLServer mCommon = new module.mdlSQLServer();

            StringBuilder strSql = new StringBuilder();
            SettingUserDataModel retSettingUserDataModel = new SettingUserDataModel();



            /*------------------------------*/
            /*　Keyデータチェック*/
            //retMapViweModel.IMSI
            retSettingUserDataModel.UserID = getSettingUserDataModel.UserID;

            /*------------------------------*/
            /*　MAP用データリスト*/
            /*------------------------------*/
            /*------------------------------*/
            //retMapViweModel
            strSql.Clear();
            strSql.Append("SELECT  "
                    + " userID"
                    + ",name1"
                    + ",name2"
                    + ",nickname"
                    + ",Email"
                    + ",str(role) as role"
                    + ",str(stopFLG) as stopFLG"
                     );
            strSql.Append(" FROM [dbo].[U_Profile]");
            strSql.Append(" Where ");
            strSql.Append(" userID = '" + getSettingUserDataModel.UserID + "' ");

            List<SettingUserInfoModel> retSUIModel = mCommon.GetSQL<SettingUserInfoModel>(strSql);
            if (retSUIModel.Count > 0)
            {
                retSettingUserDataModel.SettingUserInfoModel = retSUIModel;
            }
            else
            {
                retSettingUserDataModel.SettingUserInfoModel = new List<SettingUserInfoModel>();
            }

            //RET
            return retSettingUserDataModel;
        }
    }
}