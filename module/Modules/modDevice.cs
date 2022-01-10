using System;
using System.Text;
using System.Collections.Generic;
using ThingsLine;
using ThingsLine.Modules;
using ThingsLine.Models;
using static ThingsLine.Models.mdlDevice;
using System.Reflection;

namespace ThingsLine.Modules
{

    ///-------------------------------<summary>
    ///端末情報の取得
    ///ココで端末の違いを吸収
    ///メインキー　：　imsi
    ///(DD_Soracom000)
    ///</summary> 
    public class modDevice
    {

        //-------------------------------------------------
        //ThingsLine.Modules
        modUDevice mUDevice = new modUDevice();
        modDDDataCNV mDDDataCNV = new modDDDataCNV();
        modDD_Soracom000 mDD_Soracom000  = new modDD_Soracom000();
        //SQLserver
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        modSYS mSYS = new modSYS();

        ///-------------------------------<summary>
        ///端末最終位置取得()</summary> 
        /// <param name="imsi">imsi</param>
        /// <returns>端末MSG(DD_Soracom000)</returns>
        public DeviceBaseData LastData(string mode ,string imsi)
        {
            //メッセージ処理用
            var fullClassName = "ThingsLineAPIs.Events";            // 名前空間まで含めてクラス名を取得
            var methodName = MethodBase.GetCurrentMethod().Name;            // メソッド名を取得
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

            try
            {
                //デバイスタイプ取得
                DeviceBaseData retDeviceBaseData = new DeviceBaseData();
                U_Device UDevice =  mUDevice.GetUDevice(imsi);

                //デバイスの種類毎の処理
                if (UDevice.deviceType == "SKG001")
                {
                    stepMSG = "SKG001"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                    DD_Soracom000 retData = null;
                    retData = mDD_Soracom000.LastData(mode,imsi);
                    
                    if (retData == null)
                    {
                        return null;
                    }


                    retDeviceBaseData.dt = retData.dt;
                    retDeviceBaseData.imsi = retData.imsi;
                    retDeviceBaseData.d_lat = retData.d_lat;
                    retDeviceBaseData.d_lon = retData.d_lon;
                    retDeviceBaseData.d_bat = mDDDataCNV.DDataCNV(UDevice.deviceType, "d_bat", retData.d_bat.ToString());
                    retDeviceBaseData.d_type = retData.d_type;

                    stepMSG = "-------------------------retDeviceBaseData"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                    stepMSG = "dt : " + retDeviceBaseData.dt; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                    stepMSG = "imsi : " + retDeviceBaseData.imsi; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                    stepMSG = "d_lat : " + retDeviceBaseData.d_lat; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                    stepMSG = "d_lon : " + retDeviceBaseData.d_lon; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                    stepMSG = "d_bat : " + retDeviceBaseData.d_bat; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                    stepMSG = "d_type : " + retDeviceBaseData.d_type; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                    stepMSG = "-------------------------retDeviceBaseData"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

                }

                return retDeviceBaseData;
            }
            catch(Exception ex) {
                stepMSG = "NG : " + ex.ToString(); Console.Error.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                mSYS.Log2DBERR("ThingsLineAPIs.Events", "MainTimer", ex.ToString(), stepMSG, "", "");
                return null;
            }
        }

    }
}
