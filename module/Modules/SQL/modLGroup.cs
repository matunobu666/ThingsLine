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
    public class modLGroup
    {
        String CN = "[modLGroup]";
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        //-------------------------------
        //ba
        //-------------------------------        
        public List<l_Group> getLGroup(string groupID = "",string userId = "")
        {
            Console.WriteLine(CN + "データ取得[Start] ");
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT " 
                                        + "  groupId"
                                        + ", userId"
                                        + ", type"
                                        + ", timestamp"
                                        + ", Status"
                                    + " FROM [dbo].[l_Group]"
                                    + "  where "
                                        + "  Status <> '9'"
                                    );
                if (groupID != "")
                {
                    sSQL.Append(" and groupId = '" + groupID + "'");
                }
                if (userId != "")
                {
                    sSQL.Append(" and userId = '" + userId + "'");
                }

                Console.WriteLine(CN +  sSQL.ToString());
                List<l_Group> rets = mSQLServer.GetSQL<l_Group>(sSQL);

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
        ///l_Groupの追加</summary> 
        /// <param name="lGroup">lGroup</param>
        /// <returns>Exception</returns>
        public Exception SetlGroup(l_Group lGroup)
        {
            Console.WriteLine(CN + " メッセージタスクの追加[Start] ");
            try
            {
                //---------------------
                //監視フラグセット
                //DB登録
                sSQL.Clear();
                sSQL.Append(""
                    + "insert into  [dbo].[l_Group]("
                                        + "  groupId"
                                        + ", userId"
                                        + ", type"
                                        + ", timestamp"
                                        + ", Status"
                    + " ) VALUES ( "
                        + "  '" + lGroup.groupId + "'"
                        + ", '" + lGroup.userId + "'"
                        + ", '" + lGroup.type + "'"
                        + ", dateadd(hour,9,'" + DateTime.Now + "')"
                        + ", '" + lGroup.Status + "'"
                    + ")"
                    );
                Console.WriteLine(CN + "sql : " + sSQL.ToString());

                Exception retE = mSQLServer.setSQL(sSQL.ToString());

                Console.WriteLine(CN + "メッセージタスクの追加[END] ");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(CN + "メッセージタスクの追加[NG]" + ex.ToString());
                return ex;
            }
        }

    }
}
