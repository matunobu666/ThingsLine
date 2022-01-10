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
    public class modUProfile
    {
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();
        //Thingsline
        //メッセージ処理用
        modSYS mSYS = new modSYS();
        string classMSG = "modUProfile";
        string stepMSG = "";


        ///-------------------------------<summary>
        ///MNameの変換</summary> 
        /// <param name="codeGroup">codeGroup</param>
        /// <param name="codeID">codeID</param>
        /// <param name="Code">Code</param>
        /// <returns>string</returns>
        public U_Profile getUProfile (string userID)
        {
            stepMSG = "Start"; Console.WriteLine("[" + classMSG + "] " + stepMSG);
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT"
                                        + "  [userID]"
                                        + " ,[name1]"
                                        + " ,[name2]"
                                        + " ,[nickname]"
                                        + " ,[Email]"
                                        + " ,[role]"
                                        + " ,[stopFLG]"
                                    + " FROM"
                                        + " [dbo].[U_Profile]"
                                    + " where "
                                        + " [userID] = '" + userID + "'"
                                    );
                stepMSG = sSQL.ToString(); Console.WriteLine("[" + classMSG + "] " + stepMSG);
                List<U_Profile> rets = mSQLServer.GetSQL<U_Profile>(sSQL);

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


        ///-------------------------------<summary>
        ///MNameの変換</summary> 
        /// <param name="codeGroup">codeGroup</param>
        /// <param name="codeID">codeID</param>
        /// <param name="Code">Code</param>
        /// <returns>string</returns>
        public List<MNameList> getListUProfile(string codeGroup, string codeID)
        {
            stepMSG = "Start"; Console.WriteLine("[" + classMSG + "] " + stepMSG);
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT"
                                    + "  [Code]"
                                    + " , [Codetext]"
                                    + " FROM[dbo].[M_Name]"
                                    + " where "
                                    + " [codeGroup] = '" + codeGroup + "'"
                                    + " and [codeID] = '" + codeID + "'"
                                    + " and stopFLG = 0"
                                    );
                stepMSG = sSQL.ToString(); Console.WriteLine("[" + classMSG + "] " + stepMSG);
                List<MNameList> rets = mSQLServer.GetSQL<MNameList>(sSQL);

                stepMSG = "END "; Console.WriteLine("[" + classMSG + "] " + stepMSG);
                return rets;
            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR("ThingsLine.Modules", classMSG, ex.ToString(), stepMSG, "", "");
                stepMSG = "NG "; Console.Error.WriteLine("[" + classMSG + "] " + stepMSG);
                return null;
            }
        }

        public static implicit operator modUProfile(U_Profile v)
        {
            throw new NotImplementedException();
        }
    }
}
