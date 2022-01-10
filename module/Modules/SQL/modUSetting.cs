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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThingsLine.Modules
{
    public class modUSetting
    {
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();
        //Thingsline
        modSYS mSYS = new modSYS();



        ///-------------------------------<summary>
        ///UMessageTaskの追加</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="eventType">eventType</param>
        /// <param name="eventCode">eventCode</param>
        /// <param name="fdata01">fdata01</param>
        /// <param name="fdata02">fdata02</param>
        /// <returns>Exception</returns>
        public List<U_Setting> GetList(string userID = null, int type01=0, int type02=0)
        {
            var fullClassName = this.GetType().FullName;            // 名前空間まで含めてクラス名を取得
            var methodName = MethodBase.GetCurrentMethod().Name;            // メソッド名を取得
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
            try
            {
                //---------------------
                //監視フラグセット
                //DB登録
                sSQL.Clear();
                sSQL.Append(""
                    + "Select "
                        + " *"
                    + "From "
                        + "[dbo].[U_Setting] "
                   + " where "
                       + "  [stopFLG] = 0"
                );
                if (userID != null)
                {
                    sSQL.Append(" and [userID] = '" + userID + "'");
                }
                if (type01 != 0)
                {
                    sSQL.Append(" and [type01] = " + type01);
                }
                if (type02 != 0)
                {
                    sSQL.Append(" and [type02] = " + type02);
                }
                sSQL.Append(" order by  [userID] ");

                stepMSG = sSQL.ToString(); Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                
                List<U_Setting> rets = mSQLServer.GetSQL<U_Setting>(sSQL);

                stepMSG = "END : RetCount=" + rets.Count.ToString(); Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                return rets;
            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR(fullClassName, methodName, ex.ToString(), stepMSG, "", "");
                stepMSG = "NG"; Console.Error.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                return null;
            }
        }
        ///-------------------------------<summary>
        ///UMessageTaskの追加</summary> 
        /// <param name="U_MessageTask">U_MessageTask</param>
        /// <returns>Exception</returns>
        public Exception SetData(U_Setting getU_Setting)
        {
            var fullClassName = this.GetType().FullName;            // 名前空間まで含めてクラス名を取得
            var methodName = MethodBase.GetCurrentMethod().Name;            // メソッド名を取得
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
            try
            {
                //---------------------
                //監視フラグセット
                //DB登録
                sSQL.Clear();
                sSQL.Append(""
                    + "insert into  [dbo].[U_Setting]("
                        + "  [userID]"
                        + ", [type01]"
                        + ", [type02]"
                        + ", [valueInt01]"
                        + ", [valueInt02]"
                        + ", [valueflt01]"
                        + ", [valueflt02]"
                        + ", [stopFLG]"
                    + " ) VALUES ( "
                        + " '" + getU_Setting.userID + "'"
                        + ", " + getU_Setting.type01 
                        + ", " + getU_Setting.type02
                        + ", " + getU_Setting.valueInt01
                        + ", " + getU_Setting.valueInt02
                        + ", " + getU_Setting.valueflt01
                        + ", " + getU_Setting.valueflt02
                        + ", " + getU_Setting.stopFLG
                    + ")"
                    );
                stepMSG = sSQL.ToString(); Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                Exception retE = mSQLServer.setSQL(sSQL.ToString());

                stepMSG = "END"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                return null;
            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR(fullClassName, methodName, ex.ToString(), stepMSG, "", "");
                stepMSG = "NG"; Console.Error.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                return ex;
            }
        }

        ///-------------------------------<summary>
        ///UMessageTaskの削除</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="eventType">eventType</param>
        /// <param name="eventCode">eventCode</param>
        /// <param name="fdata01">fdata01</param>
        /// <param name="fdata02">fdata02</param>
        /// <returns>Exception</returns>
        public Exception DelData(string userID)
        {
            var fullClassName = this.GetType().FullName;            // 名前空間まで含めてクラス名を取得
            var methodName = MethodBase.GetCurrentMethod().Name;            // メソッド名を取得
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
            try
            {
                //---------------------
                //監視フラグセット
                //DB登録
                sSQL.Clear();
                sSQL.Append(""
                   + "delete from "
                        + " [dbo].[U_Setting]"
                   + " where "
                       + "  [userID] = " + userID + "'"
                );

                stepMSG = sSQL.ToString(); Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                Exception retE = mSQLServer.setSQL(sSQL.ToString());

                stepMSG = "END"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                return null;
            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR(fullClassName, methodName, ex.ToString(), stepMSG, "", "");
                stepMSG = "NG"; Console.Error.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                return ex;
            }
        }


    }
}
