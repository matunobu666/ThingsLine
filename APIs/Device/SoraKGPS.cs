using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Data.SqlClient;
using ThingsLineAPIs.Modules;
using ThingsLine.Modules;
using System.Collections.Generic;
using ThingsLine.Models;

namespace ThingsLineAPIs.Device
{
    public static class SoraKGPS
    {
        public class rcvJWTdataH
        {
                public string alg { get; set; }
                public string typ { get; set; }
                public string kid { get; set; }
        }

        public class Ctx
        {
            public string operatorId { get; set; }
            public string coverage { get; set; }
            public string resourceType { get; set; }
            public string resourceId { get; set; }
            public string sourceProtocol { get; set; }
            public string srn { get; set; }
            public string imsi { get; set; }
            public string imei { get; set; }
        }

        public class rcvJWTdata
        {
            public string iss { get; set; }
            public string aud { get; set; }
            public string jti { get; set; }
            public int iat { get; set; }
            public string typ { get; set; }
            public string sub { get; set; }
            public Ctx ctx { get; set; }
        }

        public class rcvBodyData
        {
            public string lat { get; set; }
            public string lon { get; set; }
            public int bat { get; set; }
            public int rs { get; set; }
            public string temp { get; set; }
            public string humi { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
            public int type { get; set; }
        }

        /*
                 [FunctionName("SoraKBeacon")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
         */


        [FunctionName("SoraKGPS")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //Thingsline
            modThingsLine mThingsLine = new modThingsLine();
            modSYS mSYS = new modSYS();
            //LINEä÷òA
            modLINEMsg mLINE = new modLINEMsg();
            modDeviceAPI mDeviceAPI = new modDeviceAPI();

            Console.WriteLine("[SoraKGPS] Start 2 ");
            try
            {
                //------------------------------------
                //Handlerèàóù
                string coll = req.Method;
                coll = req.Headers["x-soracom-token"];
                var stream = coll;
                var handler = new JwtSecurityTokenHandler();
                Console.WriteLine("[SoraKGPS] handler:" + handler.ToString());

                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                Console.WriteLine("[SoraKGPS] tokenS:" + tokenS.ToString());

                //àÍíUSTRÇ≈èàóùÅiÇ©Ç¡Ç±ÇÌÇÈÇ¢
                string str = @jsonToken.ToString();
                string[] sp = str.Split("}.{");
                string postJWTdata = "{" + sp[1];
                var JWTdata = JsonConvert.DeserializeObject<rcvJWTdata>(postJWTdata.ToString());

                Console.WriteLine("[SoraKGPS] JWT END");

                //------------------------------------
                //POSTBODYèàóù
                //{"lat":35.766131,"lon":139.794101,"bat":3,"rs":4,"temp":30.7,"humi":53.3,"x":0,"y":0,"z":-960,"type":0}
                string postBodyData = await new StreamReader(req.Body).ReadToEndAsync();
                Console.WriteLine("[SoraKGPS] " + postBodyData);
                var BodyData = JsonConvert.DeserializeObject<rcvBodyData>(postBodyData);

                string strnull = "null";



                //------------------------------------
                //DBäiî[èàóù
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = "tldb001.database.windows.net";
                builder.UserID = "matu";
                builder.Password = "masa2203!!";
                builder.InitialCatalog = "thingsline";
                //  builder.InitialCatalog = "Server=tcp:tldb001.database.windows.net,1433;Initial Catalog=thingsline;Persist Security Info=False;User ID=matu;Password=masa2203!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {

                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("insert ");
                    sb.Append("into  [dbo].[DD_Soracom000]("
                        + " operatorId"
                        + ",dt"
                        + ",imsi"
                        + ",imei"
                        + ",d_lat"
                        + ",d_lon"
                        + ",d_bat"
                        + ",d_rs"
                        + ",d_temp"
                        + ",d_humi"
                        + ",d_a_x"
                        + ",d_a_y"
                        + ",d_a_z"
                        + ",d_type"
                        + ")"
                        );
                    sb.Append("VALUES  ( "
                        + " '" + JWTdata.ctx.operatorId + "'"
                        + ",dateadd(hour,9,'" + DateTime.Now + "')"
                        + "," + JWTdata.ctx.imsi + ""
                        + "," + JWTdata.ctx.imei + ""
                        + "," + (BodyData.lat ?? strnull) + ""
                        + "," + (BodyData.lon ?? strnull) + ""
                        + "," + BodyData.bat + ""
                        + "," + BodyData.rs + ""
                        + "," + BodyData.temp + ""
                        + "," + BodyData.humi + ""
                        + "," + BodyData.x + ""
                        + "," + BodyData.y + ""
                        + "," + BodyData.z + ""
                        + "," + BodyData.type + ""
                        + ")"
                        );
                    String sql = sb.ToString();
                    Console.WriteLine(sql);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("[SoraKGPS] " + "{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }

                }
                if ((BodyData.lat ?? strnull) != null && (BodyData.lon ?? strnull) != null)
                {
                    
                    Console.WriteLine("[SoraKGPS] EventCHK ST " + JWTdata.ctx.imsi + ":" + BodyData.lat + ":" + BodyData.lon);
                    //------------------------------------
                    //EVENTCHKèàóù
                    if (JWTdata.ctx.imsi !=null && BodyData.lat != null && BodyData.lon != null)
                    {
                        Boolean bRet = mDeviceAPI.EventCHK(JWTdata.ctx.imsi, double.Parse(BodyData.lat), double.Parse(BodyData.lon));
                        if (bRet == false)
                        {
                            Console.Error.WriteLine("[SoraKGPS] EventCHK ErrEND ");
                        }
                    }
                    Console.WriteLine("[SoraKGPS] EventCHK END ");
                }

                Console.WriteLine("-----OUTPUT-----");
                Console.WriteLine("operatorId:" + JWTdata.ctx.operatorId.ToString());
                Console.WriteLine("dt        :" + DateTime.Now.ToString());
                Console.WriteLine("imsi      :" + JWTdata.ctx.imsi.ToString());
                /*
                                Console.WriteLine("imei      :" + JWTdata.ctx.imei.ToString());
                                Console.WriteLine("lat       :" + (BodyData.lat ?? strnull).ToString());
                                Console.WriteLine("lon       :" + (BodyData.lon ?? strnull).ToString());
                                Console.WriteLine("bat       :" + BodyData.bat.ToString());
                                //                Console.WriteLine("rs        :" + BodyData.rs.ToString());
                                Console.WriteLine("temp      :" + BodyData.temp.ToString());
                                Console.WriteLine("humi      :" + BodyData.humi.ToString());
                                Console.WriteLine("x         :" + BodyData.x.ToString());
                                Console.WriteLine("y         :" + BodyData.y.ToString());
                                Console.WriteLine("z         :" + BodyData.z.ToString());
                                Console.WriteLine("type      :" + BodyData.type.ToString());
                */
                Console.WriteLine("-----OUTPUT_END-----");
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                Console.Error.WriteLine("[SoraKGPS] END ERR " + ex);
                mSYS.Log2DBERR("ThingsLineAPIs.Device", "SoraKGPS", "ERR", ex.ToString());
                return new BadRequestResult();
            }
            Console.WriteLine("[SoraKGPS] END OK");
            return new OkResult();
        }
    }
}
