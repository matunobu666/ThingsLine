using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Data.SqlClient;

namespace SoraKGPS
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

        [FunctionName("SoraKGPS")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {

            log.LogInformation("[SoraKGPS] Start ");
            try
            {

                string coll = req.Method;
                coll = req.Headers["x-soracom-token"];
                var stream = coll;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                log.LogInformation("[SoraKGPS] tokenS:" + tokenS.ToString());

                //àÍíUSTRÇ≈èàóùÅiÇ©Ç¡Ç±ÇÌÇÈÇ¢
                string str = @jsonToken.ToString();
                string[] sp = str.Split("}.{");
                string postJWTdata = "{" + sp[1];
                var JWTdata = JsonConvert.DeserializeObject<rcvJWTdata>(postJWTdata.ToString());

                log.LogInformation("[SoraKGPS] JWT END");




                //POSTBODY
                //{"lat":35.766131,"lon":139.794101,"bat":3,"rs":4,"temp":30.7,"humi":53.3,"x":0,"y":0,"z":-960,"type":0}
                string postBodyData = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation("[SoraKGPS] " + postBodyData);
                var BodyData = JsonConvert.DeserializeObject<rcvBodyData>(postBodyData);

                string strnull = "null";

                log.LogInformation("-----OUTPUT-----");
                log.LogInformation("operatorId:" + JWTdata.ctx.operatorId.ToString());
                log.LogInformation("dt        :" + DateTime.Now.ToString());
                log.LogInformation("imsi      :" + JWTdata.ctx.imsi.ToString());
                log.LogInformation("imei      :" + JWTdata.ctx.imei.ToString());
                log.LogInformation("lat       :" + (BodyData.lat ?? strnull).ToString());
                log.LogInformation("lon       :" + (BodyData.lon ?? strnull).ToString());
                log.LogInformation("bat       :" + BodyData.bat.ToString());
                log.LogInformation("rs        :" + BodyData.rs.ToString());
                log.LogInformation("temp      :" + BodyData.temp.ToString());
                log.LogInformation("humi      :" + BodyData.humi.ToString());
                log.LogInformation("x         :" + BodyData.x.ToString());
                log.LogInformation("y         :" + BodyData.y.ToString());
                log.LogInformation("z         :" + BodyData.z.ToString());
                log.LogInformation("type      :" + BodyData.type.ToString());
                log.LogInformation("-----OUTPUT_END-----");

                //DBäiî[
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
                    log.LogInformation(sql);

                    using (SqlCommand command = new SqlCommand(sql, connection))
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
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                log.LogInformation("[SoraKGPS] END ERR " + ex);
                return new BadRequestResult();
            }

            log.LogInformation("[SoraKGPS] END OK");
            return new OkResult();

        }
    }
}
