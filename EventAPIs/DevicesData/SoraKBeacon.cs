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
using static SoraKGPS.SoraKGPS;

namespace WHookAPIs.DevicesData
{
    public static class SoraKBeacon
    {
        [FunctionName("SoraKBeacon")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
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
                log.LogInformation("[SoraKBeacon] tokenS:" + tokenS.ToString());

                //àÍíUSTRÇ≈èàóùÅiÇ©Ç¡Ç±ÇÌÇÈÇ¢
                string str = @jsonToken.ToString();
                string[] sp = str.Split("}.{");
                string postJWTdata = "{" + sp[1];
                var JWTdata = JsonConvert.DeserializeObject<rcvJWTdata>(postJWTdata.ToString());

                log.LogInformation("[SoraKBeacon] JWT END");




                //POSTBODY
                string postBodyData = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation("[SoraKBeacon] " + postBodyData);
                //  var BodyData = JsonConvert.DeserializeObject<rcvBodyData>(postBodyData);



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
                    sb.Append("into  [dbo].[Z_Test]("
                        + " dt"
                        + ",data1"
                        + ",data2"
                        + ")"
                        );
                    sb.Append("VALUES  ( "
                        + " dateadd(hour,9,'" + DateTime.Now + "')"
                        + ",'" + tokenS.ToString() + "'"
                        + ",'" + postBodyData + "'"
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

