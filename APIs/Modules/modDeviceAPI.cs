using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Configuration;
using ThingsLineAPIs.Models;
using ThingsLine.Modules;
using ThingsLine.Models;
using static ThingsLine.Models.mdlDevice;

namespace ThingsLineAPIs.Modules
{
    public class modDeviceAPI
    { 
        //Thingsline
        modTheftMonitoring mTheftMonitoring = new modTheftMonitoring();
        modDeviceMode mDeviceMode = new modDeviceMode();
        modUserBaseData mUserBaseData = new modUserBaseData();
        modDD_Soracom000 mDD_Soracom000 = new modDD_Soracom000();
        modUMessageTask mUMessageTask = new modUMessageTask();


        ///-------------------------------<summary>
        ///イベントチェックメイン処理</summary> 
        /// <param name="imsi">imsi</param>
        /// <returns>true  false</returns>
        public Boolean EventCHK( string imsi, double lat, double lon)
        {
            Console.WriteLine("[EventCHK] EVENTCHK処理(共通) START");
            //---------------------
            //  デバイス状態取得
            List<DeviceMode> rets = mDeviceMode.GetDeviceMode(imsi);
         
            foreach (DeviceMode d in rets)
            {
                //---------------------
                //監視処理（ジオフェンス）
                if (d.eventCode == 1 && d.eventType == 1)
                {
                    Console.WriteLine("[EventCHK] [Start] TheftMonitoringCHK");
                    //移動チェック
                    Boolean bRet = mTheftMonitoring.TheftMonitoringCHK(imsi, lat, lon);
                    if (bRet == false)
                    {
                        Console.Error.WriteLine("[EventCHK] [ERR] TheftMonitoringCHK");
                    }
                    Console.WriteLine("[EventCHK] [END] TheftMonitoringCHK");
                }

                //---------------------
                //  バイク状態取得
                //移動中判断平均移動距離を取って・・・渋滞どうするよ

           //     List<DeviceMode> rets = mDeviceMode.GetDeviceMode(imsi);


                //---------------------
                //***処理
                //ココに追加

            }
            Console.WriteLine("[EventCHK] EVENTCHK処理(共通) END");
            return true;
        }



    }
}
