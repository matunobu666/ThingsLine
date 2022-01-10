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
    public class modSSystemMessage
    {
        String CN = "[modSSystemMessage]";
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        ///-------------------------------<summary>
        //S_ErrorMessageの追加</summary> 
        /// <param name="S_ErrorMessage">lGroupInfo</param>
        /// <returns>Exception</returns>
        public void SetSErrorMessage(S_SystemMessage SErrorMessage)
        {
            string mdl = "[SetSSystemMessage]";

            Console.WriteLine(CN + mdl + "の追加[Start] ");

            try
            {
                //---------------------
                //監視フラグセット
                //DB登録
                sSQL.Clear();
                sSQL.Append(""
                    + "insert into  [dbo].[S_SystemMessage]("
                    + " [dt]"
                    + ",[type]"
                    + ",[namespace]"
                    + ",[member]"
                    + ",[Exc]"
                    + ",[comment01]"
                    + ",[comment02]"
                    + ",[comment03]"
                    + " ) VALUES ( "
                        + " dateadd(hour,9,'" + DateTime.Now + "')"
                        + " , '" + SErrorMessage.type + "'"
                        + " , '" + SErrorMessage.s_namespace + "'"
                        + " , '" + SErrorMessage.s_member + "'"
                        + " , '" + SErrorMessage.Exc + "'"
                        + " , N'" + SErrorMessage.comment01 + "'"
                        + " , N'" + SErrorMessage.comment02 + "'"
                        + " , N'" + SErrorMessage.comment03 + "'"
                    + ")"
                    );
                Console.WriteLine(CN + mdl + " sql : " + sSQL.ToString());

                Exception retE = mSQLServer.setSQL(sSQL.ToString());

                Console.WriteLine(CN + mdl + "の追加[END] ");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(CN + mdl +"の追加[NG]" + ex.ToString());
            }
        }

    }
}
