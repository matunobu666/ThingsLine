using System;
using System.Collections.Generic;
using System.Text;
using static ThingsLine.Models.mdlDevice;

namespace ThingsLine.Modules
{
    public class modDD_Soracom000
    {
        //SQLServer関連
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        ///-------------------------------<summary>
        ///端末最終位置取得(DD_Soracom000)</summary> 
        /// <param name="imsi">imsi</param>
        /// <returns>端末MSG(DD_Soracom000)</returns>
        public DD_Soracom000 LastPoint(string imsi)
        {

            Console.WriteLine("[" + this.GetType().FullName + "][Start]データ取得 ");
            try
            {
                //---------------------
                //  端末最終位置チェック
                sSQL.Clear();
                sSQL.Append("SELECT"
                        + " TOP (1) "
                        + " dt,imsi,imei,operatorId,d_lat,d_lon,d_bat,d_rs,d_temp,d_humi,d_a_x,d_a_y,d_a_z,d_type "
                        + " FROM[dbo].[DD_Soracom000]"
                        + "  where imsi = '" + imsi + "'"
                        + "     and d_lat IS NOT NULL"
                        + "     and d_lon IS NOT NULL"
                        + " order by dt desc");
                Console.WriteLine("[" + this.GetType().FullName + "] " + sSQL.ToString());
                List<DD_Soracom000> rets = mSQLServer.GetSQL<DD_Soracom000>(sSQL);


                Console.WriteLine("[" + this.GetType().FullName + "][END] データ取得");
                return rets[0];
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[" + this.GetType().FullName + "][NG]データ取得" + ex.ToString());
                return null;
            }
        }

        ///-------------------------------<summary>
        ///端末最終データ取得(DD_Soracom000)</summary> 
        /// <param name="imsi">imsi</param>
        /// <returns>端末MSG(DD_Soracom000)</returns>
        public DD_Soracom000 LastData(string mode ,string imsi)
        {

            Console.WriteLine("[" + this.GetType().FullName + "][Start]データ取得 ");
            try
            {
                //---------------------
                //  端末最終位置チェック
                sSQL.Clear();
                sSQL.Append(""
                        + "SELECT"
                            + " TOP (1) "
                            + " dt"
                            + " ,imsi"
                            + " ,imei"
                            + " ,operatorId"
                            + " ,d_lat"
                            + " ,d_lon"
                            + " ,d_bat"
                            + " ,d_rs"
                            + " ,d_temp"
                            + " ,d_humi"
                            + " ,d_a_x"
                            + " ,d_a_y"
                            + " ,d_a_z"
                            + " ,d_type "
                        + " FROM"
                            + " [dbo].[DD_Soracom000]"
                        + "  where "
                            + " imsi = '" + imsi + "'"
                );
                if (mode == "point")
                {
                    sSQL.Append(""
                        + "     and d_lat IS NOT NULL"
                        + "     and d_lon IS NOT NULL"
                    );
                }

                sSQL.Append(""
                        + " order by dt desc"
                );
                Console.WriteLine("[" + this.GetType().FullName + "] " + sSQL.ToString());
                List<DD_Soracom000> rets = mSQLServer.GetSQL<DD_Soracom000>(sSQL);


                Console.WriteLine("[" + this.GetType().FullName + "][END] データ取得");
                return rets[0];
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[" + this.GetType().FullName + "][NG]データ取得" + ex.ToString());
                return null;
            }
        }


        ///-------------------------------<summary>
        ///端末最終位置取得(DD_Soracom000)</summary> 
        /// <param name="imsi">imsi</param>
        /// <returns>端末MSG(DD_Soracom000)</returns>
        public List<DD_Soracom000> PeriodTimeData(string imsi, DateTime STDT, DateTime EDDT)
        {
            Console.WriteLine("[" + this.GetType().FullName + "][Start]データ取得 ");
            try
            {
                //---------------------
                //  端末最終位置チェック
                sSQL.Clear();
                sSQL.Append("SELECT"
                        + " dt,imsi,imei,operatorId,d_lat,d_lon,d_bat,d_rs,d_temp,d_humi,d_a_x,d_a_y,d_a_z,d_type "
                        + " FROM[dbo].[DD_Soracom000]"
                        + "  where imsi = '" + imsi + "'"
                        + "  and dt <= '" + STDT + "'"
                        + "  and dt >= '" + EDDT + "'"
                        + " order by dt desc");
                Console.WriteLine("[" + this.GetType().FullName + "] " + sSQL.ToString());
                List<DD_Soracom000> rets = mSQLServer.GetSQL<DD_Soracom000>(sSQL);

                Console.WriteLine("[" + this.GetType().FullName + "][END] データ取得");
                return rets;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[" + this.GetType().FullName + "][NG]データ取得" + ex.ToString());
                return null;
            }
        }



    }
}
