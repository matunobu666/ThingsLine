using System;
using System.Collections.Generic;
using System.Text;
using ThingsLine.Models;



namespace ThingsLine.Modules
{
    public class modDeviceMode
    {
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        ///-------------------------------<summary>
        ///デバイス設定取得</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="sWhere">追加Where文</param>
        /// <returns>デバイスモードのリスト(DeviceMode)</returns>
        public List<DeviceMode> GetDeviceMode(string imsi)
        {
            Console.WriteLine("[MName] MNameデータ取得[Start] ");
            try
            {
                //---------------------
                //  デバイス設定取得
                sSQL.Clear();
                sSQL.Append("SELECT"
                                        + "  [imsi]"
                                        + " ,[eventType]"
                                        + " ,[eventCode]"
                                        + " ,[fdata01]"
                                        + " ,[fdata02]"
                                        + " ,[fdata03]"
                                        + " ,[fdata04]"
                                    + " FROM [dbo].[d_DeviceMode]"
                                    + " where "
                                        + " [imsi] = '" + imsi + "'"
                                    );
                Console.WriteLine("[EventCHK] " + sSQL.ToString());
                List<DeviceMode> rets = mSQLServer.GetSQL<DeviceMode>(sSQL);

                Console.WriteLine("[MName] MNameデータ取得[END] ");
                return rets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[MName] MNameデータ取得[NG]" + ex.ToString());
                return null;
            }
        }
        ///-------------------------------<summary>
        ///デバイス設定取得(特定コード)</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="sWhere">追加Where文</param>
        /// <returns>デバイスモードのリスト(DeviceMode)</returns>
        public List<DeviceMode> GetDeviceMode(string imsi, int eventType , int eventCode )
        {
            Console.WriteLine("[GetDeviceMode] MNameデータ取得[Start] ");
            try
            {
                //---------------------
                //  デバイス設定取得
                sSQL.Clear();
                sSQL.Append("SELECT"
                                        + "  [imsi]"
                                        + " ,[eventType]"
                                        + " ,[eventCode]"
                                        + " ,[fdata01]"
                                        + " ,[fdata02]"
                                        + " ,[fdata03]"
                                        + " ,[fdata04]"
                                    + " FROM [dbo].[d_DeviceMode]"
                                    + " where "
                                        + " [imsi] = '" + imsi + "'"
                                        + " and [eventType] = " + eventType
                                        + " and [eventCode] = " + eventCode
                                    );
                Console.WriteLine("[GetDeviceMode] " + sSQL.ToString());
                List<DeviceMode> rets = mSQLServer.GetSQL<DeviceMode>(sSQL);

                Console.WriteLine("[GetDeviceMode] MNameデータ取得[END] ");
                return rets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[GetDeviceMode] MNameデータ取得[NG]" + ex.ToString());
                return null;
            }
        }




        ///-------------------------------<summary>
        ///デバイス設定の追加</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="eventType">eventType</param>
        /// <param name="eventCode">eventCode</param>
        /// <param name="fdata01">fdata01</param>
        /// <param name="fdata02">fdata02</param>
        /// <returns>Exception</returns>
        public Exception SetDeviceMode(string imsi, string eventType, string eventCode, string fdata01 = null, string fdata02 = null, string fdata03 = null, string fdata04 = null)
        {
            Console.WriteLine("[SetDeviceMode] デバイス設定の追加[Start] ");
            try
            {
                //---------------------
                //監視フラグセット
                //DB登録
                sSQL.Clear();
                sSQL.Append("" 
                    + "insert into  [dbo].[d_DeviceMode]("
                        + "  [dt]"
                        + ", [imsi]"
                        + ", [eventType]"
                        + ", [eventCode]"
                        + ", [fdata01]" 
                        + ", [fdata02]"
                        + ", [fdata03]"
                        + ", [fdata04]"
                    + " ) VALUES ( "
                        + " dateadd(hour,9,'" + DateTime.Now + "')"
                        + ", '" + imsi + "'"
                        + ", " + eventType
                        + ", " + eventCode
                        + ", " + fdata01
                        + ", " + fdata02
                        + ", " + fdata03
                        + ", " + fdata04
                    + ")"
                    );
                Console.WriteLine("[SetDeviceMode] sql : " + sSQL.ToString());

                Exception retE = mSQLServer.setSQL(sSQL.ToString());


                Console.WriteLine("[SetDeviceMode] デバイス設定の追加[END] ");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[SetDeviceMode] デバイス設定の追加[NG]" + ex.ToString());
                return ex;
            }
        }
        ///-------------------------------<summary>
        ///デバイス設定の削除</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="eventType">eventType</param>
        /// <param name="eventCode">eventCode</param>
        /// <returns>Exception</returns>
        public Exception DelDeviceMode(string imsi, int eventType, int eventCode, string fdata01 = null, string fdata02 = null, string fdata03 = null, string fdata04 = null)
        {
            Console.WriteLine("[SetDeviceMode] デバイス設定の追加[Start] ");
            try
            {
                //---------------------
                //監視フラグセット
                //DB登録
                sSQL.Clear();
                sSQL.Append(""
                   + "delete from [dbo].[d_DeviceMode]"
                   + " where "
                        + " [imsi] = '" + imsi + "'"
                        + " and [eventType] = " + eventType
                        + " and [eventCode] = " + eventCode
                    );
                Console.WriteLine("[SetDeviceMode] sql : " + sSQL.ToString());

                Exception retE = mSQLServer.setSQL(sSQL.ToString());


                Console.WriteLine("[SetDeviceMode] デバイス設定の追加[END] ");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[SetDeviceMode] デバイス設定の追加[NG]" + ex.ToString());
                return ex;
            }
        }
    }
}
