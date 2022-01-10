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
using static ThingsLine.Models.mdlDevice;

namespace ThingsLine.Event
{
    public class modBatteryCheck
    {
        //-------------------------------------------------
        //ThingsLineAPIs.Modules
        //SQLserver
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();
        //Thingsline
        modThingsLine mThingsLine = new modThingsLine();
        modUserBaseData mUserBaseData = new modUserBaseData();
        modSSystemMessage mSSystemMessage = new modSSystemMessage();
        modDevice mDevice = new modDevice();
        modUMessageTask mUMessageTask = new modUMessageTask();
        modSYS mSYS = new modSYS();



        ///-------------------------------<summary>
        ///UMessageTaskの追加</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="eventType">eventType</param>
        /// <param name="eventCode">eventCode</param>
        /// <param name="fdata01">fdata01</param>
        /// <param name="fdata02">fdata02</param>
        /// <returns>Exception</returns>
        public Boolean SetUMessageTask(string imsi ,string userID )
        {
            var fullClassName = this.GetType().FullName;            // 名前空間まで含めてクラス名を取得
            var methodName = MethodBase.GetCurrentMethod().Name;            // メソッド名を取得
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
            try
            {
                //----------------------
                //デバイスごとの最新デバイスベースデータ取得
                DeviceBaseData retDeviceBaseData = mDevice.LastData("data",imsi);

                stepMSG = "imsi = " + retDeviceBaseData.imsi; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                stepMSG = "d_bat = " + retDeviceBaseData.d_bat; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                if (retDeviceBaseData != null)
                {

                    //----------------------------------------------
                    //バッテリー低
                    //----------------------------------------------
                    //ユーザー毎のメッセージタスク取得
                    List<U_MessageTask> retU_MessageTask = mUMessageTask.GetUMessageTask(imsi, userID, 1, 1);
                    //バッテリー低　MSG登録なしの場合
                    if (retDeviceBaseData.d_bat <= 10 && retU_MessageTask.Count == 0)
                    {
                        stepMSG = "Bat LOW!!"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                        //メッセージタスクに保存
                        U_MessageTask setU_MessageTask = new U_MessageTask
                        {
                            imsi = imsi,
                            userID = userID,
                            msgType = 1,
                            msgCode = 1,
                            msgCount = 2
                        };
                        Exception retEX = mUMessageTask.SetUMessageTask(setU_MessageTask);
                    }


                    //----------------------------------------------
                    //バッテリー無し&MSG登録無し
                    //ユーザー毎のメッセージタスク取得
                    retU_MessageTask = mUMessageTask.GetUMessageTask(imsi, userID, 1, 2);
                    stepMSG = retDeviceBaseData.dt.ToString() + "-" + DateTime.Now.AddHours(8).AddDays(-1); Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

                    if (retDeviceBaseData.dt <= DateTime.Now.AddHours(-30) && retU_MessageTask.Count == 0)
                    {
                        stepMSG = "NoData"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                        //メッセージタスクに保存
                        U_MessageTask setU_MessageTask = new U_MessageTask
                        {
                            imsi = imsi,
                            userID = userID,
                            msgType = 1,
                            msgCode = 2,
                            msgCount = 2
                        };
                        Exception retEX = mUMessageTask.SetUMessageTask(setU_MessageTask);
                    }


                    //----------------------------------------------
                    //バッテリーHi&MSG登録アリ
                    //MSGT削除処理
                    retU_MessageTask = mUMessageTask.GetUMessageTask(imsi, userID, 1);
                    stepMSG = "DellMsg:" + retDeviceBaseData.d_bat; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

                    if (retDeviceBaseData.d_bat == 100 || retDeviceBaseData.d_bat == -1)
                    {
                        stepMSG = "DelData"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                        //メッセージタスクを削除
                        Exception retEX = mUMessageTask.DelUMessageTask(imsi, userID, 1);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR(fullClassName, methodName, ex.ToString(), stepMSG, "", "");
                stepMSG = "NG"; Console.Error.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                return false;
            }
        }


    }
}
