using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

using ThingsLine.Modules;
using ThingsLine.Models;
using ThingsLineAPIs.Modules;
using ThingsLineAPIs.Models;
using static ThingsLine.Models.mdlDevice;

namespace ThingsLineAPIs.Modules
{
    class modBikeLocation
    {
        //        private readonly HttpClient _httpClient;
        //        public modBikeLocation() { _httpClient = new HttpClient(); }

        //-------------------------------
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();
        //LINE関連
        modLINEMsg mLINE = new modLINEMsg();
        //Thingsline
        modThingsLine mThingsLine = new modThingsLine();
        modBikeDeviceBaseData mBikeDeviceBaseData = new modBikeDeviceBaseData();
        modMName mMName = new modMName();
        modDD_Soracom000 mDD_Soracom000 = new modDD_Soracom000();
        modDeviceMode mDeviceMode  = new modDeviceMode();

        ///-------------------------------<summary>
        ///LINE MENU 位置確認　コマンド用</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <returns>Exception</returns>
        public Exception BikeLocation(string userLineId)
        {
            Console.WriteLine("[BikeLocation] テキスト＞状態確認 ");
            try
            {
                //---------------------
                //  ユーザー保有バイクチェック
                List<BikeDeviceBaseData> rets = mBikeDeviceBaseData.BikeDeviceChk(userLineId);

                if (rets.Count == 0)
                {
                    Console.WriteLine("[BikeLocation] 登録バイクなし処理");
                    //---------------------
                    //LINETXT送信
                    Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, "バイク、端末が登録されていません");
                }
                else
                {
                    Console.WriteLine("[BikeLocation] 登録バイクあり処理 ");

                    foreach (BikeDeviceBaseData d in rets)
                    {
                        Console.WriteLine("[BikeLocation] : " + d.userID + ":" + d.bikeName + ":" + d.imsi + ":" + d.deviceType);
                        //---------------------
                        //  端末最終位置取得
                        DD_Soracom000 retList = mDD_Soracom000.LastData("point", d.imsi);

                        if (retList == null)
                        {
                            //----------------------------
                            //処理対象が無い場合
                            Console.WriteLine("[BikeLocation] 対象なしMSG");
                            //MSG送信
                            Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, d.bikeName + "\n" + "位置データは有りません");
                        }
                        else
                        {
                            Console.WriteLine("[BikeLocation] ロケーションMSG");
                            Console.WriteLine("[BikeLocation] " + d.imsi + "  " + retList.d_lat + "  " + retList.d_lon);

                            //-------------------------------
                            //ジオフェンスチェック
                            int bRet = mThingsLine.Geofence_imsi( retList.imsi, retList.d_lat, retList.d_lon);

                            string sEvent = "";
                            if (bRet == 1 || bRet == 2)
                            {
                                sEvent = "移動検知";
                            }
                            //---------------------
                            //  デバイス設定取得
                            List<DeviceMode> retsDeviceMode = mDeviceMode.GetDeviceMode(d.imsi);

                            string sDeviceMode = "";
                            foreach (DeviceMode dDeviceMode in retsDeviceMode)
                            {
                                //監視中
                                if (dDeviceMode.eventCode == 1 && dDeviceMode.eventType == 1)
                                {
                                    //デバイスモード
                                    sDeviceMode = sDeviceMode + "監視中";
                                }
                            }
                            //---------------------
                            //返信JSON作成
                            string stitle = d.bikeName + " " + sDeviceMode;
                            string saddress = ""
                                            + retList.dt.ToString("yyyy/MM/dd HH:mm:ss") + "\n"
                                            + " イベント:" + sEvent + "\n"
                                            + " BAT:" + mMName.MName(d.deviceType, "bat", retList.d_bat.ToString()) + "\n"
                                            + " アンテナ:" + mMName.MName(d.deviceType, "rs", retList.d_rs.ToString()) + "\n"
                                            + " イベント:" + mMName.MName(d.deviceType, "type", retList.d_type.ToString()) + "\n"
                                            + " 温度:" + retList.d_temp.ToString() + "℃\n"
                                            + " 湿度:" + retList.d_humi.ToString() + "%\n"
                                            ;
                            Double dlatitude = retList.d_lat;
                            Double dlongitude = retList.d_lon;
                            //MSG送信処理(LinMsgLocMSG000)
                            Task<Exception> EX = mLINE.sendLocationMSGAsyncTo(userLineId, stitle, saddress, dlatitude, dlongitude);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
    }
}

