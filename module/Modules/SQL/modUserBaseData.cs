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

namespace ThingsLine.Modules
{
    public class modUserBaseData
    {
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        ///-------------------------------<summary>
        ///デバイスのリスト</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <returns>デバイスリスト</returns>
        public List<UserBaseData> DeviceList()
        {
            Console.WriteLine("[DeviceList] デバイスのリスト取得[Start]");
            try
            {
                //---------------------
                //  デバイスリスト取得
                sSQL.Clear();
                sSQL.Append(""
                    + " SELECT "
                        + "   UD.[imsi] "
                        + " , NUL.ProviderKey as LINEID"
                        + " , UD.userID"
                    + " FROM"
                        + "[dbo].[U_Device] as UD"
                        + " join[dbo].AspNetUserLogins as NUL "
                            + " on(UD.userID = NUL.UserId)"
                    + " where "
                    + " UD.stopFLG = 0"
                                    );
                Console.WriteLine("[DeviceList] " + sSQL.ToString());
                List<UserBaseData> rets = mSQLServer.GetSQL<UserBaseData>(sSQL);

                Console.WriteLine("[DeviceList] ベースデータ取得[END] ");
                return rets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[DeviceList] ベースデータ取得[NG]" + ex.ToString());
                return null;
            }
        }

        //-------------------------------
        //ba
        //-------------------------------        
        public List<UserBaseData> imsi2UserBaseData(string imsi)
        {
            Console.WriteLine("[imsi2UserBaseData] ベースデータ取得[Start] imsi:" + imsi);
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();



                sSQL.Append("SELECT ANUL.[UserId],ANUL.[ProviderKey] as LINEID, '" + imsi + "'  as imsi  FROM [dbo].[AspNetUserLogins] as ANUL"
                                    + "  where ANUL.LoginProvider = 'Line' and ANUL.[UserId] = "
                                                                        + " (SELECT UDevice.userID"
                                                                        + " FROM[dbo].[U_Device] as UDevice"
                                                                        + " where  UDevice.imsi = '" + imsi + "'"
                                                                        + " and UDevice.stopFLG = 0"
                                    + ")"
                                    );
                Console.WriteLine("[imsi2UserBaseData] " + sSQL.ToString());
                List<UserBaseData> rets = mSQLServer.GetSQL<UserBaseData>(sSQL);

                Console.WriteLine("[imsi2UserBaseData] ベースデータ取得[END] ");
                return rets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[imsi2UserBaseData] ベースデータ取得[NG]" + ex.ToString());
                return null;
            }
        }

    }
}
