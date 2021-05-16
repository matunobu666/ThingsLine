using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using LINEAPI;
using System.Text;
using System.Collections.Generic;

namespace ThingsLine.LINEAPI
{
    public class LinImg000
    {
        private readonly HttpClient _httpClient;

        public LinImg000()
        {
            _httpClient = new HttpClient();
        }
        
        [FunctionName("LinImg000")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("[LinImg000] Start ");

            //SQLserver
            module.mdlSQLServer mSQLServer = new module.mdlSQLServer();
            StringBuilder sSQL = new StringBuilder();

            module.mdlStorage mBlob = new module.mdlStorage();
            module.mdlLINE mLINE = new module.mdlLINE();


            try
            {

                //-------------------------------
                // Get body
                //-------------------------------
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // Deserialize
                var data = JsonConvert.DeserializeObject<LinMsgModels.LinImg000>(requestBody);
                log.LogInformation("[LinImg000] requestBody: " + requestBody);

                //-----------------------------------
                //基本データの取得
                //-------------------------------
                //lineIDからuserID取得
                var lineid = data.Event.source.userId;
                log.LogInformation("[LinImg000] lineid: " + lineid);
                                //  基本情報
                                sSQL.Clear();
                                sSQL.Append("SELECT"
                                                + " Id "
                                        + " FROM"
                                                + " [dbo].[AspNetUsers] "
                                        + " WHERE"
                                                + " Email = '" + lineid + "@line.com'");

                                string retuserID = mSQLServer.GetSQL(sSQL);
                /*
                module.modThingsLine.AspNetUsers setAspNetUsers = new module.modThingsLine.AspNetUsers
                {
                    Email = lineid + "@line.com"
                };

                log.LogInformation("[LinImg000] setAspNetUsers: " + setAspNetUsers.Email);
                List<module.modThingsLine.AspNetUsers> retsetAspNetUsers =  mThingsLine.GetAspNetUsers(setAspNetUsers);

                if (retsetAspNetUsers == null )
                {
                    throw new FormatException(setAspNetUsers.Email +"：ヒットしませんでした");
                }
                if (retsetAspNetUsers.Count != 1)
                {
                    throw new FormatException(setAspNetUsers.Email　+ "：複数ヒットしました");
                }

                log.LogInformation("[LinImg000] retuserID: " + retsetAspNetUsers[0].Id);
                */

                //-----------------------------------
                //画像データの取得
                //-------------------------------
                Stream response = await mLINE.GetImageAsync(_httpClient, data.Event.message.id);

                //-----------------------------------
                //ファイル名作成
                //-------------------------------
                string setFolder = "imagetmp";
                string setFileName = "";


                if (data.Event.source.groupId == null && data.Event.source.roomId == null)
                {
                    //ユーザーのみ
                    setFileName = retuserID + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".JPG";
                    log.LogInformation("[LinImg000] upload: " + setFileName);
                }
                else if (data.Event.source.groupId != null)
                {
                    //グループ
                    setFileName = retuserID + data.Event.source.groupId + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".JPG";
                    log.LogInformation("[LinImg000] upload: " + setFileName);
                }
                else if (data.Event.source.roomId != null)
                {
                    //ルーム
                    setFileName = retuserID + data.Event.source.roomId + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".JPG";
                    log.LogInformation("[LinImg000] upload: " + setFileName);
                }

                //-----------------------------------
                //画像の保存
                //-------------------------------
                if (setFileName != "" || setFileName != null)
                {
                    Boolean retBoolean = mBlob.BlobSave(response, setFolder, setFileName);
                    log.LogInformation("[LinImg000] UPOK-----------------------------------");
                }else{
                    log.LogError("[LinImg000] UPNG-----------------------------------");
                }


                //-------------------------------------------------
                //DB登録
                sSQL.Clear();
                sSQL.Append("insert into  [dbo].[Z_test2]("
                    + "  [dt]"
                    + " ,[type]"
                    + " ,[app]"
                    + " ,[data1]"
                    + " ,[data2]"
                    + " ,[data3]"
                    + " ,[data4]"
                    + ")"
                    + " VALUES  ( "
                    + " dateadd(hour,9,'" + DateTime.Now + "')"
                    + ",'3'"
                    + ",'LinImg000'"
                    + ",''"
                    + ", '" + requestBody + "'"
                    + ", '" + setFolder + "'"
                    + ", '" + setFileName + "'"
                    + ")"
                    );
                log.LogInformation("[MessageEvent] sql : " + sSQL.ToString());

                Exception retB = mSQLServer.setSQL(sSQL.ToString());



            }
            catch (Exception ex)
            {
                log.LogError("[LinImg000] END ERR " + ex);
                log.LogError("[LinImg000] END(Err) ");
                return new BadRequestResult();
            }



            log.LogInformation("[LinImg000] END OK");
            return new OkResult();
        }
    }
}