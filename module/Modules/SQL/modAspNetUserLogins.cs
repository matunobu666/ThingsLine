using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ThingsLine.Models;
using ThingsLine.Modules;
using System.Device.Location;


namespace ThingsLine.Modules
{
    public class modAspNetUserLogins
    {
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();
        //Thingsline
        //メッセージ処理用
        modSYS mSYS = new modSYS();
        string classMSG = "modAspNetUserLogins";
        string stepMSG = "";


        ///-------------------------------<summary>
        ///UserIdの変換</summary> 
        /// <param name="UserId">UserId</param>
        /// <returns>string</returns>
        public AspNetUserLogins getID(string UserId)
        {
            stepMSG = "Start"; Console.WriteLine("[" + classMSG + "] " + stepMSG);
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT"
                                        + "[LoginProvider]"
                                        + ",[ProviderKey]"
                                        + ",[UserId]"
                                    + " FROM[dbo].[AspNetUserLogins]"
                                    + " where "
                                    + " [UserId] = '" + UserId + "'"
                                    );
                stepMSG = sSQL.ToString(); Console.WriteLine("[" + classMSG + "] " + stepMSG);
                List<AspNetUserLogins> rets = mSQLServer.GetSQL<AspNetUserLogins>(sSQL);

                stepMSG = "END "; Console.WriteLine("[" + classMSG + "] " + stepMSG);
                return rets[0];
            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR("ThingsLine.Modules", classMSG, ex.ToString(), stepMSG, "", "");
                stepMSG = "NG "; Console.Error.WriteLine("[" + classMSG + "] " + stepMSG);
                return null;
            }
        }


    }
}
