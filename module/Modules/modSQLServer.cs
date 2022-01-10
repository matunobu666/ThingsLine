using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace ThingsLine.Modules
{
    /// <summary>
    /// SQLサーバークラス
    /// </summary>
    public class modSQLServer
    {

        //        private static readonly string connectionString = ConfigurationManager.AppSettings.Get("DefaultSQLConnection");
        //        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                private static readonly string connectionString = "Server=tcp:tldb001.database.windows.net,1433;Initial Catalog=thingsline;Persist Security Info=False;User ID=matu;Password=masa2203!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //private static readonly string connectionString = "Data Source=(local);Initial Catalog=thingsline;Integrated Security=True;";

        // private static readonly string connectionString =

        //------------------------------------
        // SELECT（string）
        //------------------------------------
        /// <summary>
        /// SELECT（文字戻し）
        /// </summary>
        public string GetSQL(StringBuilder getSQL)
        {
            Console.WriteLine("[modSQLServerSQLSERVER] start : " + connectionString);
            try
            {
                string sSQL = getSQL.ToString();
                Console.WriteLine("[modSQLServer] sSQL: " + sSQL);
                Console.WriteLine("[modSQLServer] connectionString: " + connectionString);
                string retString = "";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sSQL, connection))
                    {

                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine("[modSQLServer] reader.HasRows: " + reader.HasRows);
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("[modSQLServer] retString: test");
                                retString = reader.GetString(0);
                                Console.WriteLine("[modSQLServer] retString: " + retString);

                            }
                        }
                        else
                        {
                            retString="";
                        }
                        reader.Close();
                    }
                    connection.Close();

                    Console.WriteLine("[modSQLServer] retString: " + retString);
                    Console.WriteLine("[modSQLServer] END(OK) ");

                    return retString;

                }
            }
            catch (Exception ex) {

                Console.Error.WriteLine("[modSQLServer] ERR: " + ex.Message.ToString());
                Console.Error.WriteLine("[modSQLServer] END(Err) ");

                return "";

            }
        }
        /*------------------------------------*/
        /* SELECT（カウンター用）
        /*------------------------------------*/
        /// <summary>
        /// SELECT（数字戻し）
        /// </summary>
        public int GetCNTSQL(StringBuilder getSQL)
        {
            Console.WriteLine("[modSQLServer] start ");
            try
            {
                string sSQL = getSQL.ToString();
                Console.WriteLine("[modSQLServer] sSQL: " + sSQL);
                Console.WriteLine("[modSQLServer] connectionString: " + connectionString);
                int retInt = -1 ;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sSQL, connection))
                    {

                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine("[modSQLServer] reader.HasRows: " + reader.HasRows);
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                retInt = reader.GetInt32(0);
                                Console.WriteLine("[modSQLServer] retInt: " + retInt);
                            }
                        }
                        reader.Close();
                    }
                    connection.Close();
                    Console.WriteLine("[modSQLServer] END(OK) ");

                    return retInt;
                }
            }
            catch (Exception ex)
            {

                Console.Error.WriteLine("[modSQLServer] ERR: " + ex.Message.ToString());
                Console.Error.WriteLine("[modSQLServer] END(Err) ");
                throw ex;

            }
        }


        /*------------------------------------*/
        /* SELECT（LIST）
        /*------------------------------------*/
        /// <summary>
        /// SELECT（リスト戻し）
        /// </summary>
        public List<T> GetSQL<T>(StringBuilder getSQL)
        {
            List<T> dtoList = new List<T>();
            // LINEユーザーIDチェック＆取得
            //DB格納
            string sSQL = getSQL.ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                using (SqlCommand command = new SqlCommand(sSQL, connection))
                {
                    // コンバート
                    dtoList = ConvertDto<T>(command);

                    connection.Close();
                    return dtoList;
                }
            }
        }

        /*------------------------------------*/
        /* 投げっぱなし用
        /*------------------------------------*/
        /// <summary>
        /// 投げっぱなし
        /// </summary>
        public Exception setSQL(string sSQL)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sSQL, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /*------------------------------------*/
        /* コンバート
        /*------------------------------------*/
        /// <summary>
        /// SQL戻りデータをLISTに変換
        /// </summary>
        /// <param name="command">SQLリターン</param>
        /// <returns>指定LIST形式のSQLリターン</returns>
        public List<T> ConvertDto<T>(SqlCommand command)
        {
            List<T> dtoList = new List<T>();

            // usingブロックが終わり次第、コネクションを閉じるようにCommandBehavior.CloseConnectionを指定
            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                // 行データ分のループ
                while (reader.Read())
                {
                    // Tで指定された総称型のインスタンスを生成（DTOの作成）
                    T dtoObject = (T)Activator.CreateInstance(typeof(T));

                    foreach (PropertyInfo property in dtoObject.GetType().GetProperties())
                    {
                        if (!DBNull.Value.Equals(reader[property.Name]))
                        {
                            // プロパティ（フィールド）名を元に、SqlDataReaderから値を取得し、DTOにセット
                            property.SetValue(dtoObject, reader[property.Name], null);
                        }
                    }

                    dtoList.Add(dtoObject);
                }
            }

            return dtoList;
        }






    }
}
