using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace module
{
    public class mdlSQLServer
    {

//        private static readonly string connectionString = ConfigurationManager.AppSettings.Get("DefaultSQLConnection");
//        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
          private static readonly string connectionString = "Server=tcp:tldb001.database.windows.net,1433;Initial Catalog=thingsline;Persist Security Info=False;User ID=matu;Password=masa2203!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        /*------------------------------------*/
        /* SELECT（string）
        /*------------------------------------*/
        public string GetSQL(StringBuilder getSQL)
        {
            Console.WriteLine("[mdlSQLServer] start ");
            try
            {
                string sSQL = getSQL.ToString();
                Console.WriteLine("[mdlSQLServer] sSQL: " + sSQL);
                Console.WriteLine("[mdlSQLServer] connectionString: " + connectionString);
                string retString = "";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sSQL, connection))
                    {

                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine("[mdlSQLServer] reader.HasRows: " + reader.HasRows);
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("[mdlSQLServer] retString: test");
                                retString = reader.GetString(0);
                                Console.WriteLine("[mdlSQLServer] retString: " + retString);

                            }
                        }
                        else
                        {
                            retString="";
                        }
                        reader.Close();
                    }
                    connection.Close();

                    Console.WriteLine("[mdlSQLServer] retString: " + retString);
                    Console.WriteLine("[mdlSQLServer] END(OK) ");

                    return retString;

                }
            }
            catch (Exception ex) {

                Console.Error.WriteLine("[mdlSQLServer] ERR: " + ex.Message.ToString());
                Console.Error.WriteLine("[mdlSQLServer] END(Err) ");

                return "";

            }
        }



        /*------------------------------------*/
        /* SELECT（LIST）
        /*------------------------------------*/
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

                    dtoList = ConvertDto<T>(command);
                    connection.Close();
                    return dtoList;


                }
            }

        }


        /*------------------------------------*/
        /* コンバート
        /*------------------------------------*/
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

        /*------------------------------------*/
        /* 投げっぱなし用
        /*------------------------------------*/
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
            catch(Exception ex) {
                return ex;
            
            }

        }




    }
}
