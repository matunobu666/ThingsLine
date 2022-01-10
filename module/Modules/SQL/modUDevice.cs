using System;
using System.Collections.Generic;
using System.Text;
using ThingsLine.Models;
using static ThingsLine.Models.mdlDevice;

namespace ThingsLine.Modules
{
    public class modUDevice
    {
        //-------------------------------------------------
        //ThingsLineAPIs.Modules
        //SQLserver
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();
        //Thingsline
        modSYS mSYS = new modSYS();
        modThingsLine mThingsLine = new modThingsLine();
        modUserBaseData mUserBaseData = new modUserBaseData();
        modSSystemMessage mSSystemMessage = new modSSystemMessage();

        //メッセージ処理用
        string classMSG = "modUDevice";
        string stepMSG = "";

        ///-------------------------------<summary>
        ///デバイスリスト取得</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="sWhere">追加Where文</param>
        /// <returns>デバイスモードのリスト(DeviceMode)</returns>
        public U_Device GetUDevice(string imsi)
        {
            stepMSG = "Start"; Console.WriteLine("[" + classMSG + "] " + stepMSG);
            try
            {
                //---------------------
                //  デバイス設定取得
                sSQL.Clear();
                sSQL.Append("SELECT"
                                        + "  UD.[userID]"
                                        + ", UD.[imsi]"
                                        + ", UD.[deviceType]"
                                        + ", UD.[deviceName]"
                                        + ", UD.[errCNT]"
                                        + ", UD.[stopFLG]"
                                        + ", UD.[bikeID]"
                                    + " FROM [dbo].[U_Device] as UD"
                                    + " where "
                                        + " UD.[imsi] = '" + imsi + "'"
                                        + " and UD.stopFLG = 0"
                                    );
                stepMSG = sSQL.ToString(); Console.WriteLine("[" + classMSG + "] " + stepMSG);
                List<U_Device> rets = mSQLServer.GetSQL<U_Device>(sSQL);

                stepMSG = "END"; Console.WriteLine("[" + classMSG + "] " + stepMSG);
                return rets[0];
            }
            catch (Exception ex)
            {
                stepMSG = "NG"; Console.WriteLine("[" + classMSG + "] " + stepMSG);
                mSYS.Log2DBERR("ThingsLineAPIs.Events", "MainTimer", ex.ToString(), stepMSG, "", "");
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

    }
}
