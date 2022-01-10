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
    public class modBikeDeviceBaseData
    {
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        ///-------------------------------<summary>
        ///ユーザー保有バイクチェック</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <returns>バイクのリスト(userLineId)</returns>
        public List<BikeDeviceBaseData> BikeDeviceChk(string userLineId)
        {
            Console.WriteLine("[BikeDeviceChk] ベースデータ取得[Start] userLineId:"+ userLineId);
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT UDevice.userID, UDevice.imsi,UBIKE.bikeName,UDevice.deviceType"
                                    + " FROM[dbo].[U_Device] as UDevice join [dbo].[U_BIKE] as UBIKE on (UDevice.bikeID = UBIKE.bikeID)"
                                    + " where UDevice.userID = "
                                        + "(SELECT [UserId]  FROM [dbo].[AspNetUserLogins] as ANUL"
                                        + "  where ANUL.LoginProvider = 'Line' and ANUL.ProviderKey = '" + userLineId + "')"
                                    + " and UDevice.stopFLG = 0"
                                    + " order by UBIKE.sortNO"
                                    );
                Console.WriteLine("[BikeDeviceChk] " + sSQL.ToString());
                List<BikeDeviceBaseData> rets = mSQLServer.GetSQL<BikeDeviceBaseData>(sSQL);

                Console.WriteLine("[BikeDeviceChk] ベースデータ取得[END] ");
                return rets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[BikeDeviceChk] ベースデータ取得[NG]" + ex.ToString());
                return null;
            }
        }

        ///-------------------------------<summary>
        ///ユーザー保有バイクチェック</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <param name="imsi">imsi</param>
        /// <returns>バイクのリスト(userLineId,imsi)</returns>
        public List<BikeDeviceBaseData> BikeDeviceChk(string userLineId,string imsi)
        {
            Console.WriteLine("[BikeDeviceChk] ベースデータ取得[Start] userLineId:" + userLineId);
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT UDevice.userID, UDevice.imsi,UBIKE.bikeName,UDevice.deviceType"
                                    + " FROM[dbo].[U_Device] as UDevice join [dbo].[U_BIKE] as UBIKE on (UDevice.bikeID = UBIKE.bikeID)"
                                    + " where UDevice.userID = "
                                        + "(SELECT [UserId]  FROM [dbo].[AspNetUserLogins] as ANUL"
                                        + "  where ANUL.LoginProvider = 'Line' and ANUL.ProviderKey = '" + userLineId + "')"
                                        + " and  UDevice.imsi = '" + imsi + "'"
                                    + " and UDevice.stopFLG = 0"
                                    + " order by UBIKE.sortNO"
                                    );
                Console.WriteLine("[BikeLocation] " + sSQL.ToString());
                List<BikeDeviceBaseData> rets = mSQLServer.GetSQL<BikeDeviceBaseData>(sSQL);

                Console.WriteLine("[BikeDeviceChk] ベースデータ取得[END] ");
                return rets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[BikeDeviceChk] ベースデータ取得[NG]" + ex.ToString());
                return null;
            }
        }
        ///-------------------------------<summary>
        ///ユーザー保有バイクチェック</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <param name="imsi">imsi</param>
        /// <returns>バイクのリスト(userLineId,imsi)</returns>
        public BikeDeviceBaseData BikeDeviceChk2userID(string userID, string imsi)
        {
            Console.WriteLine("[BikeDeviceChk] ベースデータ取得[Start] userLineID:" + userID);
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                sSQL.Clear();
                sSQL.Append("SELECT UDevice.userID, UDevice.imsi,UBIKE.bikeName,UDevice.deviceType"
                                    + " FROM[dbo].[U_Device] as UDevice join [dbo].[U_BIKE] as UBIKE on (UDevice.bikeID = UBIKE.bikeID)"
                                    + " where UDevice.userID = '" + userID + "'"
                                        + " and  UDevice.imsi = '" + imsi + "'"
                                    + " and UDevice.stopFLG = 0"
                                    + " order by UBIKE.sortNO"
                                    );
                Console.WriteLine("[BikeLocation] " + sSQL.ToString());
                List<BikeDeviceBaseData> rets = mSQLServer.GetSQL<BikeDeviceBaseData>(sSQL);

                Console.WriteLine("[BikeDeviceChk] ベースデータ取得[END] ");
                return rets[0];
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[BikeDeviceChk] ベースデータ取得[NG]" + ex.ToString());
                return null;
            }
        }

    }
}
