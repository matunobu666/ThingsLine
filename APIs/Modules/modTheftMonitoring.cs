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
using ThingsLineAPIs.Modules;
using ThingsLine.Modules;
using ThingsLine.Models;
using static ThingsLine.Models.mdlDevice;

namespace ThingsLineAPIs.Modules
{

    class modTheftMonitoring
    {
        //LINE関連
        modLINEMsg mLINE = new modLINEMsg();
        //Thingsline
        modThingsLine mThingsLine = new modThingsLine();
        modBikeDeviceBaseData mBikeDeviceBaseData = new modBikeDeviceBaseData();
        modUserBaseData mUserBaseData = new modUserBaseData();
        modDD_Soracom000 mDD_Soracom000 = new modDD_Soracom000();
        modDeviceMode mDeviceMode = new modDeviceMode();
        modAspNetUserLogins mAspNetUserLogins = new modAspNetUserLogins();

        ///-------------------------------<summary>
        ///移動検知処理</summary> 
        /// <param name="imsi">imsi</param>
        /// <returns>バイクのリスト(userLineId)</returns>
        public Boolean TheftMonitoringCHK(string imsi, double lat, double lon)
        {
            string stClass = "[" + this.GetType().Name + "]";
            Console.WriteLine(stClass + "[Start] 移動検知処理");

            //ジオフェンスチェック
            //0：移動無し　１：移動検知　９：エラー
            int iRet = mThingsLine.Geofence_imsi(imsi, lat, lon);

            if (iRet == 1)
            {
                List<UserBaseData> UBD = mUserBaseData.imsi2UserBaseData(imsi);
                //  ユーザー保有バイクチェック
                List<BikeDeviceBaseData> BDBD = mBikeDeviceBaseData.BikeDeviceChk(UBD[0].LINEID, imsi);

                //テキスト送信
                Task<Exception> EX = mLINE.sendTextMSGAsyncTo(UBD[0].LINEID, BDBD[0].bikeName + "の移動を検知しました");
            }
            if (iRet == 2)
            {
                List<UserBaseData> UBD = mUserBaseData.imsi2UserBaseData(imsi);
                //  ユーザー保有バイクチェック
                List<BikeDeviceBaseData> BDBD = mBikeDeviceBaseData.BikeDeviceChk(UBD[0].LINEID, imsi);

                //テキスト送信
                Task<Exception> EX = mLINE.sendTextMSGAsyncTo(UBD[0].LINEID, BDBD[0].bikeName + "の移動を検知しました"
                    + "\n" + "通知制限を超えたため、監視モードは解除されます"
                    + "\n" + "バイクの位置追跡は継続します"
                    );
            }

            Console.WriteLine(stClass + "[END] 移動検知処理");
            return true;
        }

        ///-------------------------------<summary>
        ///監視開始処理</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <param name="imsi">imsi</param>
        /// <returns>Exception</returns>   
        public Exception TheftMonitoringStart(string userLineId)
        {
            string stClass = "[" + this.GetType().Name + "]";
            Console.WriteLine(stClass + "[Start] 監視開始処理");

            //---------------------
            //バイクリスト＆監視状態取得
            //---------------------
            //  ユーザー保有バイクチェック
            List<BikeDeviceBaseData> rets = mBikeDeviceBaseData.BikeDeviceChk(userLineId);

            if (rets.Count == 0)
            {
                //---------------------
                //＝０対象なしMSG(LINETXT送信
                Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, "バイク、端末が登録されていません");
            }
            else
            {
                //保有端末＆バイクループ
                foreach (BikeDeviceBaseData d in rets)
                {
                    //---------------------
                    //監視フラグ存在チェック
                    List<DeviceMode> retDeviceMode = mDeviceMode.GetDeviceMode(d.imsi, 1,1);

                    if (retDeviceMode.Count == 0)
                    {
                        //---------------------
                        //  DeviceMode未設定
                        
                        //  端末最終位置チェック
                        DD_Soracom000 retDD_Soracom000 = mDD_Soracom000.LastData("point",d.imsi);



                        if (retDD_Soracom000 == null)
                        {
                            //---------------------
                            //監視済みメッセージ送信(LINETXT送信
                            Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, d.bikeName + ":最終位置情報が不明です");
                        }
                        else {

                            //デバイス設定の追加
                            Exception retEX = mDeviceMode.SetDeviceMode(d.imsi , "1" , "1"
                                , retDD_Soracom000.d_lat.ToString()
                                , retDD_Soracom000.d_lon.ToString()
                                , "50"
                                , "5"
                                );

                            //---------------------
                            //監視開始メッセージ送信(LINETXT送信
                            Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, d.bikeName + ":監視を開始しました");
                        }
                    }
                    else if (retDeviceMode.Count == 1)
                    {
                        //---------------------
                        //監視済みメッセージ作成(LINETXT送信
                        Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, d.bikeName + ":監視を開始済みです");
                    }
                    else
                    {
                        //---------------------
                        //エラー(LINETXT送信
                        Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, d.bikeName + ":監視開始エラー、一旦解除してください");
                    }
                }
            }
            Console.WriteLine(stClass + "[END] 監視開始処理");
            return null;
        }

        ///-------------------------------<summary>
        ///監視設定処理</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <param name="imsi">imsi</param>
        /// <returns>Exception</returns>   
        public Exception TheftMonitoringStart(string userId, string imsi)
        {
            string stClass = "[" + this.GetType().Name + "]";
            Console.WriteLine(stClass + "[Start] 監視開始処理");
            //  ユーザー保有バイクチェック
            BikeDeviceBaseData rets = mBikeDeviceBaseData.BikeDeviceChk2userID(userId,imsi);


            //---------------------
            //監視フラグ存在チェック
            List<DeviceMode> retDeviceMode = mDeviceMode.GetDeviceMode(imsi, 1, 1);
            AspNetUserLogins retANUL = mAspNetUserLogins.getID(userId);
                    if (retDeviceMode.Count == 0)
                    {
                        //---------------------
                        //  DeviceMode未設定

                        //  端末最終位置チェック
                        DD_Soracom000 retDD_Soracom000 = mDD_Soracom000.LastData("point", imsi);

                        if (retDD_Soracom000 == null)
                        {
                            //---------------------
                            //監視済みメッセージ送信(LINETXT送信
                            Task<Exception> EX = mLINE.sendTextMSGAsyncTo("U25758193796510fa7103410e0e9558d9", rets.bikeName + ":最終位置情報が不明です");
                        }
                        else
                        {

                            //デバイス設定の追加
                            Exception retEX = mDeviceMode.SetDeviceMode(imsi, "1", "1"
                                , retDD_Soracom000.d_lat.ToString()
                                , retDD_Soracom000.d_lon.ToString()
                                , "50"
                                , "5"
                                );

                            //---------------------
                            //監視開始メッセージ送信(LINETXT送信
                            Task<Exception> EX = mLINE.sendTextMSGAsyncTo("U25758193796510fa7103410e0e9558d9", rets.bikeName + ":自動監視を開始しました");
                        }
                    }

            Console.WriteLine(stClass + "[END] 監視開始処理");
            return null;
        }



        ///-------------------------------<summary>
        ///監視終了処理</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <param name="imsi">imsi</param>
        /// <returns>Exception</returns>   
        public Exception TheftMonitoringEnd(string userLineId, string imsi)
        {
            string stClass = "[" + this.GetType().Name + "]";
            Console.WriteLine(stClass + "[Start] 監視終了処理");
            //---------------------
            //バイクリスト＆監視状態取得
            //---------------------
            //  ユーザー保有バイクチェック
            List<BikeDeviceBaseData> rets = mBikeDeviceBaseData.BikeDeviceChk(userLineId);

            if (rets.Count == 0)
            {
                //---------------------
                //対象なしMSG
                //テキスト送信(LINETXT送信
                Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, "バイク、端末が登録されていません");
            }
            else
            {
                foreach (BikeDeviceBaseData d in rets)
                {
                    //デバイス設定の削除
                    Exception retEX = mDeviceMode.DelDeviceMode(d.imsi, 1, 1);
                    //MSG送信(LINETXT送信
                    Task<Exception> EX = mLINE.sendTextMSGAsyncTo(userLineId, d.bikeName + "の監視を解除しました");
                }
            }
            Console.WriteLine(stClass + "[END] 監視終了処理");
            return null;
        }

    }
}
