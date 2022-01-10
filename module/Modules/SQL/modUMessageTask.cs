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
    public class modUMessageTask
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
        public List<U_MessageTask> GetUMessageTask(string imsi = null, string userID = null, int msgType = 0, int msgCode = 0)
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
                        + "[dbo].[U_MessageTask] "
                   + " where "
                       + "  [stopFLG] = 0"
                );
                if (imsi != null)
                {
                    sSQL.Append(" and [imsi] = '" + imsi + "'");
                }
                if (userID != null)
                {
                    sSQL.Append(" and [userID] = '" + userID + "'");
                }
                if (msgType != 0)
                {
                    sSQL.Append(" and [msgType] = " + msgType);
                }
                if (msgCode != 0)
                {
                    sSQL.Append(" and [msgCode] = " + msgCode);
                }
                sSQL.Append(" order by  [userID] ");


                stepMSG = sSQL.ToString(); Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);
                
                List<U_MessageTask> rets = mSQLServer.GetSQL<U_MessageTask>(sSQL);

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
        public Exception SetUMessageTask(U_MessageTask getU_MessageTask)
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
                    + "insert into  [dbo].[U_MessageTask]("
                        + "  [dt]"
                        + ", [imsi]"
                        + ", [userID]"
                        + ", [msgType]"
                        + ", [msgCode]"
                        + ", [msgCount]"
                        + ", [fdata01]"
                        + ", [fdata02]"
                        + ", [fdata03]"
                        + ", [fdata04]"
                    + " ) VALUES ( "
                        + " dateadd(hour,9,'" + DateTime.Now + "')"
                        + ", '" + getU_MessageTask.imsi + "'"
                        + ", '" + getU_MessageTask.userID + "'"
                        + ", " + getU_MessageTask.msgType
                        + ", " + getU_MessageTask.msgCode
                        + ", " + getU_MessageTask.msgCount
                        + ", " + getU_MessageTask.fdata01
                        + ", " + getU_MessageTask.fdata02
                        + ", " + getU_MessageTask.fdata03
                        + ", " + getU_MessageTask.fdata04
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
        public Exception DelUMessageTask(string imsi = null,string userID = null, int msgType = 0, int msgCode = 0)
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
                        +" [dbo].[U_MessageTask]"
                   + " where "
                       + "  [stopFLG] = 0"
                );
                if (imsi != null)
                {
                    sSQL.Append(" and [imsi] = '" + imsi + "'");
                }
                if (userID != null)
                {
                    sSQL.Append(" and [userID] = '" + userID + "'");
                }
                if (msgType != 0)
                {
                    sSQL.Append(" and [msgType] = " + msgType);
                }
                if (msgCode != 0)
                {
                    sSQL.Append(" and [msgCode] = " + msgCode);
                }

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
        public Exception UpdateCount(U_MessageTask getUMessageTask)
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
                   + "UPDATE "
                        + " [dbo].[U_MessageTask]"
                   + " SET  "
                       + "  [msgCount] = " + (getUMessageTask.msgCount - 1)
                   + " WHERE  "
                       + "  [imsi] = '" + getUMessageTask.imsi + "'"
                       + "  and [userID] = '" + getUMessageTask.userID + "'"
                       + "  and [msgType] = " + getUMessageTask.msgType
                       + "  and [msgCode] = " + getUMessageTask.msgCode
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
