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
    public class MainTimer
    {

        [FunctionName("MainTimer")]
        public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
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
            modUMessageTask mUMessageTask  = new modUMessageTask();
            modUSetting       mUSetting=new          modUSetting();
            modBatteryCheck mBatteryCheck = new  modBatteryCheck();
            modBikeDeviceBaseData mBikeDeviceBaseData =new modBikeDeviceBaseData();
            //メッセージ処理用
            modSYS mSYS = new modSYS();
            var fullClassName = "ThingsLineAPIs.Events";            // 名前空間まで含めてクラス名を取得
            var methodName = MethodBase.GetCurrentMethod().Name;            // メソッド名を取得
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);



            try
            {

                //----------------------
                //デバイスリスト取得
                List<UserBaseData> retmUserBaseData = mUserBaseData.DeviceList();

                //----------------------------------------------
                //バッテリーチェック
                stepMSG = "Bat LOW CHK"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

                foreach (UserBaseData d in retmUserBaseData)
                {

                    Boolean retb = mBatteryCheck.SetUMessageTask(d.imsi, d.userID);
                    stepMSG = "Bat CHK" + retb; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

                }

                //----------------------------------------------
                //メッセージ送信処理

                //メッセージリストを取得（LIST
                List<U_MessageTask> 　getU_MessageTask = mUMessageTask.GetUMessageTask();

                foreach (U_MessageTask d in getU_MessageTask)
                {
                    //送信OK判断（カウンタ
                    if (d.msgCount > 0)
                    {


                        List<U_Setting> getU_Setting = mUSetting.GetList(d.userID);
                        foreach (U_Setting US in getU_Setting)
                        {
                            //基準時間取得（NOW refVal_hh
                            int refVal_hh = DateTime.Now.AddHours(9).Hour;

                            //ユーザー許可時間かをチェック
                            if (US.valueInt01 == refVal_hh || US.valueInt01 == -1)
                            {
                                List<UserBaseData> retUBD = mUserBaseData.imsi2UserBaseData(d.imsi);

                                List<BikeDeviceBaseData> retDBD = mBikeDeviceBaseData.BikeDeviceChk(retUBD[0].LINEID, d.imsi);
                                //送信処理
                                //テキスト送信
                                if (d.msgType == 1 &&  d.msgCode == 1)
                                {
                                    Task<Exception> EX = mLINE.sendTextMSGAsyncTo(retUBD[0].LINEID
                                        , retDBD[0].bikeName + ""
                                        + "\n" + "バッテリー通知"
                                        + "\n" + "バッテリーが減っています"
                                        );
                                }else if (d.msgType == 1 && d.msgCode == 2)
                                {
                                    Task<Exception> EX = mLINE.sendTextMSGAsyncTo(retUBD[0].LINEID
                                        , retDBD[0].bikeName + ""
                                        + "\n" + "バッテリー通知"
                                        + "\n" + "通信が24時間以上ありません"
                                        + "\n" + "バッテリー切れの可能性があります"
                                        );
                                }
                                //カウンタダウン
                                Exception retex = mUMessageTask.UpdateCount(d);

                                if (US.valueInt01 == -1) { break; }
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
