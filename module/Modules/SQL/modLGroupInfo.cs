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
using static ThingsLine.Models.mdlLINE;

namespace ThingsLine.Modules
{
    public class modLGroupInfo
    {
        String CN = "[modLGroupInfo]";
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        //-------------------------------
        //ba
        //-------------------------------        
        public List<l_GroupInfo> getLGroupInfo(string groupID)
        {
            Console.WriteLine(CN + "データ取得[Start] ");
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT " 
                                        + " groupId"
                                        + ", groupName"
                                        + ", type"
                                        + ", timestamp"
                                        + ", Status"
                                    + " FROM"
                                        + " [dbo].[l_GroupInfo]"
                                    + " where" 
                                        + " groupId = '" + groupID + "'"
                                    );

                Console.WriteLine(CN +  sSQL.ToString());
                List<l_GroupInfo> rets = mSQLServer.GetSQL<l_GroupInfo>(sSQL);

                Console.WriteLine(CN + "データ取得[END] ");
                return rets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(CN + "データ取得[NG]" + ex.ToString());
                return null;
            }
        }

        ///-------------------------------<summary>
        ///l_GroupInfoの追加</summary> 
        /// <param name="lGroupInfo">lGroupInfo</param>
        /// <returns>Exception</returns>
        public Exception SetlGroupInfo(l_GroupInfo lGroupInfo)
        {
            Console.WriteLine(CN + " l_GroupInfoの追加[Start] ");
            try
            {
                //---------------------
                //監視フラグセット
                //DB登録
                sSQL.Clear();
                sSQL.Append(""
                    + "insert into  [dbo].[l_GroupInfo]("
                                        + " groupId"
                                        + ", groupName"
                                        + ", type"
                                        + ", timestamp"
                                        + ", Status"
                    + " ) VALUES ( "
                        + "  '" + lGroupInfo.groupId + "'"
                        + ", '" + lGroupInfo.groupName + "'"
                        + ", '" + lGroupInfo.type + "'"
                        + ", dateadd(hour,9,'" + DateTime.Now + "')"
                        + ", '" + lGroupInfo.Status + "'"
                    + ")"
                    );
                Console.WriteLine(CN + "sql : " + sSQL.ToString());

                Exception retE = mSQLServer.setSQL(sSQL.ToString());

                Console.WriteLine(CN + "l_GroupInfoの追加[END] ");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(CN + "l_GroupInfoの追加[NG]" + ex.ToString());
                return ex;
            }
        }


    }
}
