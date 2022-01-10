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

namespace ThingsLineAPIs.Device
{
    public static class SoraKBeacon
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
            public int type { get; set; }
            public int bat { get; set; }
            public string timestamp { get; set; }
            public string loc_data { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string ns { get; set; }
            public string ew { get; set; }
            public string major_axis { get; set; }
            public string minor_axis { get; set; }

            public string uuid { get; set; }
            public string major { get; set; }
            public string minor { get; set; }
            public string rssi_b { get; set; }

            public int attr { get; set; }
            public string sns_valid_no { get; set; }
            public string adv_add { get; set; }
            public string sns1 { get; set; }
            public string sns2 { get; set; }
            public string sns3 { get; set; }
            public string sns4 { get; set; }
            public string sns5 { get; set; }
            public string sns6 { get; set; }
            public string sns7 { get; set; }
            public string sns8 { get; set; }
            public string sns9 { get; set; }
            public string sns10 { get; set; }
            public string sns11 { get; set; }
            public string sns12 { get; set; }
            public string sns13 { get; set; }
            public string sns14 { get; set; }
            public string sns15 { get; set; }
            public string sns16 { get; set; }
            public string sns17 { get; set; }
            public string sns18 { get; set; }
            public string sns19 { get; set; }
            public string sns20 { get; set; }
            public string sns21 { get; set; }
            public string sns22 { get; set; }
            public string sns23 { get; set; }
            public string sns24 { get; set; }

        }

        [FunctionName("SoraKBeacon")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[SoraKBeacon] Start1 ");
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
                var BodyData = JsonConvert.DeserializeObject<rcvBodyData>(postBodyData);

               // string strnull = "null";

                log.LogInformation("-----OUTPUT-----");
                log.LogInformation("dt        :" + DateTime.Now.ToString());
                log.LogInformation("imsi      :" + JWTdata.ctx.imsi.ToString());
//                log.LogInformation("imei      :" + JWTdata.ctx.imei.ToString());
//                log.LogInformation("operatorId:" + JWTdata.ctx.operatorId.ToString());
/*
                log.LogInformation("type      :" + BodyData.type.ToString());
                log.LogInformation("bat      :" + BodyData.bat.ToString());
                log.LogInformation("timestamp      :" + BodyData.timestamp.ToString());
                log.LogInformation("loc_data      :" + (BodyData.loc_data ?? strnull).ToString());
                log.LogInformation("lat      :" + (BodyData.lat ?? strnull).ToString());
                log.LogInformation("lon      :" + (BodyData.lon ?? strnull).ToString());
                log.LogInformation("ns      :" + (BodyData.ns ?? strnull).ToString());
                log.LogInformation("ew      :" + (BodyData.ew ?? strnull).ToString());
                log.LogInformation("major_axis      :" + (BodyData.major_axis ?? strnull).ToString());
                log.LogInformation("minor_axis      :" + (BodyData.minor_axis ?? strnull).ToString());
                log.LogInformation("uuid      :" + (BodyData.uuid ?? strnull).ToString());
                log.LogInformation("major      :" + (BodyData.major ?? strnull).ToString());
                log.LogInformation("minor      :" + (BodyData.minor ?? strnull).ToString());
                log.LogInformation("rssi_b      :" + (BodyData.rssi_b ?? strnull).ToString());
                log.LogInformation("attr      :" + BodyData.attr.ToString());
                log.LogInformation("type      :" + (BodyData.sns_valid_no ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.adv_add ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns1 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns2 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns3 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns4 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns5 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns6 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns7 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns8 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns9 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns10 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns11 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns12 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns13 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns14 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns15 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns16 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns17 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns18 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns19 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns20 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns21 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns22 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns23 ?? strnull).ToString());
                log.LogInformation("type      :" + (BodyData.sns24 ?? strnull).ToString());
*/
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
                    sb.Append("INSERT INTO [dbo].[DD_SoraKBeacon000]("
                        + " [dt]"
                        + ",[imsi]"
                        + ",[imei]"
                        + ",[operatorId]"
                        + ",[type]"
                        + ",[bat]"
                        + ",[timestamp]"
                        + ",[loc_data]"
                        + ",[lat]"
                        + ",[lon]"
                        + ",[ns]"
                        + ",[ew]"
                        + ",[major_axis]"
                        + ",[minor_axis]"
                        + ",[uuid]"
                        + ",[major]"
                        + ",[minor]"
                        + ",[rssi_b]"
                        + ",[attr]"
                        + ",[sns_valid_no]"
                        + ",[adv_add]"
                        + ",[sns1]"
                        + ",[sns2]"
                        + ",[sns3]"
                        + ",[sns4]"
                        + ",[sns5]"
                        + ",[sns6]"
                        + ",[sns7]"
                        + ",[sns8]"
                        + ",[sns9]"
                        + ",[sns10]"
                        + ",[sns11]"
                        + ",[sns12]"
                        + ",[sns13]"
                        + ",[sns14]"
                        + ",[sns15]"
                        + ",[sns16]"
                        + ",[sns17]"
                        + ",[sns18]"
                        + ",[sns19]"
                        + ",[sns20]"
                        + ",[sns21]"
                        + ",[sns22]"
                        + ",[sns23]"
                        + ",[sns24]"
                        + ")"
                        );
                    sb.Append("VALUES  ( "
                        + " dateadd(hour,9,'" + DateTime.Now + "')"
                        + "," + JWTdata.ctx.imsi + ""
                        + "," + JWTdata.ctx.imei + ""
                        + ",'" + JWTdata.ctx.operatorId + "'"
                        + "," + BodyData.type + ""
                        + "," + BodyData.bat + ""
                        + ",'" +  "'"
                        + ",'" + BodyData.loc_data + "'"
                        + ",'" + BodyData.lat + "'"
                        + ",'" + BodyData.lon + "'"
                        + ",'" + BodyData.ns + "'"
                        + ",'" + BodyData.ew + "'"
                        + ",'" + BodyData.major_axis + "'"
                        + ",'" + BodyData.minor_axis + "'"
                        + ",'" + BodyData.uuid + "'"
                        + ",'" + BodyData.major + "'"
                        + ",'" + BodyData.minor + "'"
                        + ",'" + BodyData.rssi_b + "'"
                        + ",'" + BodyData.attr + "'"
                        + ",'" + BodyData.sns_valid_no + "'"
                        + ",'" + BodyData.adv_add + "'"
                        + ",'" + BodyData.sns1 + "'"
                        + ",'" + BodyData.sns2 + "'"
                        + ",'" + BodyData.sns3 + "'"
                        + ",'" + BodyData.sns4 + "'"
                        + ",'" + BodyData.sns5 + "'"
                        + ",'" + BodyData.sns6 + "'"
                        + ",'" + BodyData.sns7 + "'"
                        + ",'" + BodyData.sns8 + "'"
                        + ",'" + BodyData.sns9 + "'"
                        + ",'" + BodyData.sns10 + "'"
                        + ",'" + BodyData.sns11 + "'"
                        + ",'" + BodyData.sns12 + "'"
                        + ",'" + BodyData.sns13 + "'"
                        + ",'" + BodyData.sns14 + "'"
                        + ",'" + BodyData.sns15 + "'"
                        + ",'" + BodyData.sns16 + "'"
                        + ",'" + BodyData.sns17 + "'"
                        + ",'" + BodyData.sns18 + "'"
                        + ",'" + BodyData.sns19 + "'"
                        + ",'" + BodyData.sns20 + "'"
                        + ",'" + BodyData.sns21 + "'"
                        + ",'" + BodyData.sns22 + "'"
                        + ",'" + BodyData.sns23 + "'"
                        + ",'" + BodyData.sns24 + "'"
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
                log.LogInformation("[SoraKBeacon] END ERR " + ex);
                return new BadRequestResult();
            }

            log.LogInformation("[SoraKBeacon] END OK");
            return new OkResult();
        }
    }
}

