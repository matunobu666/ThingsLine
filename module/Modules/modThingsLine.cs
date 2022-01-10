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


namespace ThingsLine.Modules
{
    public class modThingsLine
    {
        //SQLserver
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();
        //Thingsline
        modDeviceMode mDeviceMode = new modDeviceMode();




        ///-------------------------------<summary>
        ///ジオフェンスチェック</summary> 
        /// <param name="imsi">imsi</param>
        /// <param name="lat">現在のLat</param>
        /// <param name="lon">現在のLon</param>
        /// <returns>0：移動無し　１：移動検知 2:移動検知最終処理　９：エラー</returns>
        public int Geofence_imsi(string imsi, double lat, double lon)
        {

            try
            {
                //---------------------
                //  デバイス設定取得
                List<DeviceMode> rets = mDeviceMode.GetDeviceMode(imsi, 1, 1);

                if (rets.Count == 1)
                {

                    Console.WriteLine("[geofence] imsi : " + imsi + " lat : " + lat + " lon : " + lon);

                    double distance = hubeny_distance(lat, lon, rets[0].fdata01, rets[0].fdata02);

                    Console.WriteLine("[geofence] distance : " + distance.ToString());

                    if (distance > rets[0].fdata03 && rets[0].fdata04 > 1)
                    {
                        Console.WriteLine("[geofence] 移動検知");
                        //---------------------
                        //カウンターダウン処理
                        //DB登録
                        sSQL.Clear();
                        sSQL.Append("update [dbo].[d_DeviceMode] SET "
                                        + " [fdata04] = " + (rets[0].fdata04 - 1)
                                    + " where "
                                        + " [imsi] = '" + imsi + "'"
                                        + " and [eventType] = 1"
                                        + " and [eventCode] = 1"
                            );
                        Console.WriteLine("[modTheftMonitoring] sql : " + sSQL.ToString());

                        Exception retE = mSQLServer.setSQL(sSQL.ToString());

                        Console.WriteLine("[geofence] ジオフェンス[END] ");
                        return 1;
                    }
                    if (distance > rets[0].fdata03 && rets[0].fdata04 <= 1)
                    {
                        Console.WriteLine("[geofence] 移動検知");
                        //---------------------
                        //監視モード削除
                        //DB登録
                        Exception EX = mDeviceMode.DelDeviceMode(imsi, 1, 1);
                        return 2;
                    }
                }
                Console.WriteLine("[geofence] 移動なし");
                Console.WriteLine("[geofence] ジオフェンス[END] ");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[geofence] ジオフェンス[NG]" + ex.ToString());
                return 9;
            }
        }

        /// <summary>
        /// Hubenyの公式による２点間の距離計測
        /// </summary>
        /// <param name="d_lat0"></param>
        /// <param name="d_long0"></param>
        /// <param name="d_lat1"></param>
        /// <param name="d_long1"></param>
        /// <returns></returns>
        public double hubeny_distance(double d_lat0, double d_long0, double d_lat1, double d_long1)
        {
            //d_lat0：出発点緯度（度）
            //d_long0：出発点経度（度）
            //d_lat1：到達点緯度（度）
            //d_long1：到達点経度（度）
            //d_distance：２点の距離（ｍ）

            double Rx = 6378137.0; //赤道半径(m)
            double E2 = 0.00669437999019758; //第2離心率(e^2)


            double mnum = 6335439.32729246;
            var my = Math.PI / 180.0 * ((d_lat0 + d_lat1) / 2.0);
            var dy = Math.PI / 180.0 * (d_lat0 - d_lat1);
            var dx = Math.PI / 180.0 * (d_long0 - d_long1);

            var sin = Math.Sin(my);
            var w = Math.Sqrt(1.0 - E2 * sin * sin);
            var m = mnum / Math.Pow(w, 3);
            var n = Rx / w;

            var dym = dy * m;
            var dxncos = dx * n * Math.Cos(my);

            double d_distance = Math.Sqrt(dym * dym + dxncos * dxncos);
            return d_distance/* = d_distance * 1.00045*/;    //日本周辺だけで使う場合。ヒュベニの誤差をごまかす
        }


        /*------------------------------------*/
        /* SELECT（LIST）
        /*------------------------------------*/
        public List<MdlThingsLine.AspNetUsers> GetAspNetUsers(MdlThingsLine.AspNetUsers getSQL)
        {
            try
            {

                //  基本情報
                sSQL.Clear();
                sSQL.Append("SELECT"
                                + " [Id] ,[Email],[UserName] "
                        + " FROM"
                                + " [dbo].[AspNetUsers] "
                        + " WHERE"
                                + " Email = '" + getSQL.Email + "'");

                Console.WriteLine("[GetAspNetUsers] sSQL: " + sSQL.ToString());
                List<MdlThingsLine.AspNetUsers> retData = mSQLServer.GetSQL<MdlThingsLine.AspNetUsers>(sSQL);
                return retData;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[GetAspNetUsers] sSQL: " + sSQL.ToString());
                Console.Error.WriteLine("[GetAspNetUsers] ERR: " + ex.Message.ToString());
                Console.Error.WriteLine("[GetAspNetUsers] END(Err) ");
                return null;
            }

        }

    }
}
