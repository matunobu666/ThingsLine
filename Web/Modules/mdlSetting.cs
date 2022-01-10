using System;
using System.Collections.Generic;
using System.Text;
using ThingsLine.Models;
using ThingsLine.Modules;

namespace thingslineWeb.Modules
{
    public class mdlSetting
    {
        public U_Profile GetUserInfo(String getUserID)
        {
            /*------------------------------*/
            //SQL関連初期処理
            modSQLServer mCommon = new modSQLServer();
            StringBuilder strSql = new StringBuilder();

            U_Profile retUserInfoModel = new U_Profile();

            /*------------------------------*/
            /*　SettingUserDataModel*/
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
            strSql.Append(" userID = '" + getUserID + "' ");

            List<U_Profile> retSUDM = mCommon.GetSQL<U_Profile>(strSql);

            if (retSUDM.Count == 1)
            {
                retUserInfoModel = retSUDM[0];
            }
            else
            {
                //ほんとはエラー？
                retUserInfoModel = null;
            }

            //RET
            return retUserInfoModel;
        }




    }
}