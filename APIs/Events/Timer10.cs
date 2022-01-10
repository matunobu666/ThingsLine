using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ThingsLine.Event;
using ThingsLine.Models;
using ThingsLine.Modules;
using ThingsLineAPIs.Models;
using ThingsLineAPIs.Modules;
using static System.Console;
using static ThingsLine.Models.mdlDevice;

namespace ThingsLineAPIs.Events
{
    public static class Timer10
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            //-------------------------------------------------
            //ThingsLineAPIs.Modules
            //LINE
            mdlLineMsg LineAPIObj = new mdlLineMsg();
            modLINEMsg mLINE = new modLINEMsg();
            //SQLserver
            modSQLServer mSQLServer = new modSQLServer();
            StringBuilder sSQL = new StringBuilder();
            //Thingsline
            modThingsLine mThingsLine = new modThingsLine();
            modUserBaseData mUserBaseData = new modUserBaseData();
            modSSystemMessage mSSystemMessage = new modSSystemMessage();
            modDevice mDevice = new modDevice();
            modUMessageTask mUMessageTask = new modUMessageTask();
            modUSetting mUSetting = new modUSetting();
            modBatteryCheck mBatteryCheck = new modBatteryCheck();
            modBikeDeviceBaseData mBikeDeviceBaseData = new modBikeDeviceBaseData();
            modTheftMonitoring mTheftMonitoring = new modTheftMonitoring();
            //メッセージ処理用
            modSYS mSYS = new modSYS();
            var fullClassName = "ThingsLineAPIs.Events";            // 名前空間まで含めてクラス名を取得
            var methodName = MethodBase.GetCurrentMethod().Name;            // メソッド名を取得
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);


            try
           {
                //デバイスリスト取得（監視モードOFFのみ）
                //----------------------
                //デバイスリスト取得
                List<UserBaseData> retmUserBaseData = mUserBaseData.DeviceList();

                //----------------------------------------------
                //特定のimsiの10分カウントを取って特定の回数以下なら停車と判断
                foreach (UserBaseData d in retmUserBaseData)
                {
                    //最終通信データ取得
                    DeviceBaseData DBD = mDevice.LastData("data",d.imsi);

                    //特定のimsiの10分カウントを取って特定の回数以下なら停車と判断
                    if (DBD!=null)
                    {
                        //特定のimsiの10分カウントを取って特定の回数以下なら停車と判断
                        if (DBD.dt <= DateTime.Now.AddMinutes(-10))
                        {
                            //登録済みホームポイントリスト
                            List<U_Setting>  retUS = mUSetting.GetList(d.userID, 2, 1);
                            foreach (U_Setting US in retUS)
                            {
                                //位置計算
                                double hu = mThingsLine.hubeny_distance(DBD.d_lat, DBD.d_lon,   US.valueflt01, US.valueflt02);
                                if(hu < US.valueInt01)
                                {
                                    //監視モードON
                                    //監視開始処理
                                    Exception EX = mTheftMonitoring.TheftMonitoringStart(d.userID, d.imsi);

                                }
                            }
                        }
                    }
                }












                stepMSG = "END"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR("ThingsLineAPIs.Events", methodName, ex.ToString(), stepMSG);

            }
        }

    }
}
