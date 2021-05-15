using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace thingslineWeb.Modules
{
    public class mdlDatabase
    {
        public class DatabaseOpn
        {
            // SQL Server用コネクション
            public SqlConnection DB_Connection;

            // DB接続
            public bool DB_Connect()
            {
                DB_Connection = new SqlConnection();

                try
                {
                    // 接続文字列を作成して接続を開始する
                    DB_Connection.ConnectionString = "data source = " + mdlConst.cstrDB_Sorce + ";" +
                                              "initial catalog = " + mdlConst.cstrDB_Name + ";" +
                                              "user id         = " + mdlConst.cstrDB_User + ";" +
                                              "password        = " + mdlConst.cstrDB_Pass + ";" +
                                              "persist security info=True;" +
                                              "Pooling=True;" +
                                              "Min Pool Size=20;" +
                                              "Max Pool Size=200;" +
                                              "Connection Lifetime=2;";

                    if (DB_Connection.State == System.Data.ConnectionState.Closed)
                    {
                        DB_Connection.Open();
                    }

                    return true;
                }
                catch 
                {
                    if (DB_Connection != null)
                    {
                        DB_Connection.Close();
                    }

                    return false;
                }
            }

            // DB切断
            public bool DB_Close()
            {
                try
                {
                    if (DB_Connection.State == System.Data.ConnectionState.Open)
                    {
                        DB_Connection.Close();
                    }

                    DB_Connection.Dispose();
                    DB_Connection = null;

                    return true;

                }
                catch 
                {
                    return false;
                }
            }

            // SQL実行
            public bool DB_SqlReader(string strSql, ref SqlDataReader sqlRdr)
            {
                SqlCommand sqlCmn = new SqlCommand();

                try
                {
                    sqlCmn = new SqlCommand(strSql, DB_Connection);
                    sqlCmn.Connection = DB_Connection;
                    sqlRdr = sqlCmn.ExecuteReader();
                    sqlCmn.Dispose();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
