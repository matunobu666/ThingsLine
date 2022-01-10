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
    public class modDDDataCNV
    {
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();
        //Thingsline
        modSYS mSYS = new modSYS();
        //メッセージ処理用
        string classMSG = "modDDDataCNV";
        string stepMSG = "";

        //-------------------------------
        //ba
        //-------------------------------        
        public int DDataCNV(string deviceType, string fieldName, string data)
        {
            stepMSG = "Start"; Console.WriteLine("[" + classMSG + "] " + stepMSG);
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT"
                                        + "  [TLDataNVIN] as data"
                                    + " FROM[dbo].[DD_DataCNV]"
                                    + " where "
                                        + " [deviceType] = '" + deviceType + "'"
                                        + " and [fieldName] = '" + fieldName + "'"
                                        + " and [data] = '" + data + "'"
                                        + " and stopFLG = 0"
                                    );
                stepMSG = sSQL.ToString(); Console.WriteLine("[" + classMSG + "] " + stepMSG);
                List <retdataInt> rets = mSQLServer.GetSQL<retdataInt>(sSQL);

                stepMSG = "END "; Console.WriteLine("[" + classMSG + "] " + stepMSG);
                return rets[0].data;
            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR("ThingsLine.Modules", classMSG, ex.ToString(), stepMSG, "", "");
                stepMSG = "NG "; Console.Error.WriteLine("[" + classMSG + "] " + stepMSG);
                return -9999;
            }
        }


    }
}
