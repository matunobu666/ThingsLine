using System;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using thingslineWeb.Modules;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Configuration;

using System.Globalization;
using System.Xml.Serialization;
using System.Linq;
using System.Net.Http.Json;
using thingslineWeb.Models;
using module;

namespace thingslineWeb.Modules
{
    public class mdlCommon
    {
        private readonly HttpClient _httpClient;

        public mdlCommon() { _httpClient = new HttpClient(); }

        mdlDatabase.DatabaseOpn db = new mdlDatabase.DatabaseOpn();
        mdlConst cnst = new mdlConst();


        public string GET_DDList()
        {
            var response = new HttpResponseMessage();
            var httpClient = new System.Net.Http.HttpClient();
            //返信JSON作成

            var sendJSONobj = new mdlDevicesData.DD_Soracom000()
            {
                d_type = 2,
            };

            try
            {
                string stdURL_LinMsgTextMSG000 = mdlConst.stURL_LinMsgTextMSG000;
                //MSG送信処理(LinMsgTextMSG000)
                var responses = httpClient.PostAsJsonAsync(stdURL_LinMsgTextMSG000, sendJSONobj);

                return responses.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }




        public List<DD> getDData(MapViweModel getMapViweModel)
        {
            module.mdlSQLServer mdlSQL = new module.mdlSQLServer();
            StringBuilder strSql = new StringBuilder();
            getMapViweModel.MapData = "";

            string schkDateST = DateTime.Now.ToString("yyyy-MM-dd");
            string schkDateED = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //string schkDate = "2021/03/07 00:00:00";           
            string schkDateIMSI = "440103227845569";


            if (getMapViweModel != null)
            {
                schkDateST = getMapViweModel.SearchCndDate.ToString("yyyy/MM/dd");
                schkDateED = getMapViweModel.SearchCndDate.AddDays(1).ToString("yyyy/MM/dd");
            }


            if (getMapViweModel != null)
            {
                schkDateIMSI = getMapViweModel.IMSI.ToString();
            }






            /*------------------------------*/
            strSql.Clear();
            strSql.Append("SELECT  dt,imsi,imei,operatorId,d_lat,d_lon,d_bat,d_rs,d_temp,d_humi,d_a_x,d_a_y,d_a_z,d_type "
                     );
            strSql.Append(" FROM [dbo].[DD_Soracom000]");
            strSql.Append(" Where d_lat IS NOT NULL ");
            strSql.Append(" and dt >= '" + schkDateST + "' and dt < '" + schkDateED + "' ");
            strSql.Append(" and imsi = '" + schkDateIMSI  + "' ");
            strSql.Append(" order by dt desc");

            List<DD> retDD = mdlSQL.GetSQL<DD>(strSql);


            return retDD;


        }





    }
}