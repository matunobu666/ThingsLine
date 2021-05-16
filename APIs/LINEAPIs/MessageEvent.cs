using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net;
using LINEAPI;
using System.Net.Http;
using System.Text;

namespace LINEAPI
{
    public class MessageEvent
    {
        private readonly HttpClient _httpClient;

        public MessageEvent()
        {
            _httpClient = new HttpClient();
        }

        private static readonly string CheckURL = "https://thingslineapis.azurewebsites.net/api/LinMsgCHK000?code=MyFwzhBQ4SEdMIZdicuUjXb1QeTHY0d7pXESghsqmBGH3aPsz42HNA==";

        [FunctionName("MessageEvent")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[MessageEvent] Start ");
            log.LogInformation("[MessageEvent] Req: " + req.Body.ToString());

            LinMsgModels LineAPIObj = new LinMsgModels();
            module.mdlLINE mLINE = new module.mdlLINE();

            //SQLserver
            module.mdlSQLServer mSQLServer = new module.mdlSQLServer();
            StringBuilder sSQL = new StringBuilder();

            //--------------------------
            // ヘッダー取得 (signature)
            //--------------------------
            req.Headers.TryGetValue("X-Line-Signature", out var xlinesignature);
            log.LogInformation("[MessageEvent] Headers: " + req.Headers.ToString());

            //--------------------------
            // ボディ取得
            //--------------------------
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation("[MessageEvent] requestBody: " + requestBody.ToString());




            //--------------------------
            // しぐねいちゃチェック
            //--------------------------
            if (mLINE.SingatureCHK(xlinesignature, requestBody))
            {
                log.LogInformation("[MessageEvent] signatureCHK OK");
                // Deserialize
                var data = JsonConvert.DeserializeObject<LinMsgModels.LineWebhookMessageObject>(requestBody);
                //--------------------------
                // メッセージイベント
                if (data.events[0].type == "message") {
                    // テキストメッセージ
                    if (data.events[0].message.type == "text")// テキスト
                    {
                        log.LogInformation("[MessageEvent] Message is : text : " + data.events[0].message.text);
                    }
                    // 画像メッセージ
                    if (data.events[0].message.type == "image")
                    {
                        log.LogInformation("[MessageEvent] Message is : image :" + data.events[0].message.contentProvider.originalContentUrl);
                        log.LogInformation("[MessageEvent] Message is : image :" + data.events[0].message.contentProvider.previewImageUrl);
                    }
                    // 動画メッセージ
                    if (data.events[0].message.type == "video")
                    {
                        log.LogInformation("[MessageEvent] Message is : video :" + data.events[0].message.contentProvider.originalContentUrl);
                        log.LogInformation("[MessageEvent] Message is : video :" + data.events[0].message.contentProvider.previewImageUrl);
                    }
                    // 位置情報メッセージ
                    if (data.events[0].message.type == "location")
                    {
                        log.LogInformation("[MessageEvent] Message is : location :" + data.events[0].message.address);
                        log.LogInformation("[MessageEvent] Message is : location :" + data.events[0].message.latitude);
                        log.LogInformation("[MessageEvent] Message is : location :" + data.events[0].message.longitude);
                    }
                }
                //--------------------------
                // フォローイベント
                if (data.events[0].type == "follow")
                {
                        log.LogInformation("[MessageEvent] Message is : follow :" + data.events[0].message.contentProvider.originalContentUrl);
                        log.LogInformation("[MessageEvent] Message is : follow type :" + data.events[0].source.type);
                        log.LogInformation("[MessageEvent] Message is : follow userId :" + data.events[0].source.userId);
                }
                //--------------------------
                //  フォロー解除イベント
                if (data.events[0].type == "unfollow")
                {
                    log.LogInformation("[MessageEvent] Message is : unfollow :" + data.events[0].message.contentProvider.originalContentUrl);
                    log.LogInformation("[MessageEvent] Message is : unfollow type :" + data.events[0].source.type);
                    log.LogInformation("[MessageEvent] Message is : unfollow userId :" + data.events[0].source.userId);
                }
                //--------------------------
                //  参加イベント
                if (data.events[0].type == "join")
                {
                    log.LogInformation("[MessageEvent] Message is : join :" + data.events[0].message.contentProvider.originalContentUrl);
                    log.LogInformation("[MessageEvent] Message is : join type :" + data.events[0].source.type);
                    log.LogInformation("[MessageEvent] Message is : join groupId :" + data.events[0].source.groupId);
                }
                //--------------------------
                //  退出イベント
                if (data.events[0].type == "leave")
                {
                    log.LogInformation("[MessageEvent] Message is : leave :" + data.events[0].message.contentProvider.originalContentUrl);
                    log.LogInformation("[MessageEvent] Message is : leave type :" + data.events[0].source.type);
                    log.LogInformation("[MessageEvent] Message is : leave groupId :" + data.events[0].source.groupId);
                }
                //--------------------------
                //  メンバー参加イベント
                if (data.events[0].type == "memberJoined")
                {
                    log.LogInformation("[MessageEvent] Message is : memberJoined :" + data.events[0].message.contentProvider.originalContentUrl);
                    log.LogInformation("[MessageEvent] Message is : memberJoined type :" + data.events[0].source.type);
                    log.LogInformation("[MessageEvent] Message is : memberJoined groupId :" + data.events[0].source.groupId);
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
                    + ")"
                    + " VALUES  ( "
                    + " dateadd(hour,9,'" + DateTime.Now + "')"
                    + ",'1'"
                    + ",'MessageEvent'"
                    + ", '" + data.events[0].type + "'"
                    + ", '" + requestBody + "'"
                    + ")"
                    );
                log.LogInformation("[MessageEvent] sql : " + sSQL.ToString());

                Exception retB = mSQLServer.setSQL(sSQL.ToString());

                //--------------------------
                // 必須チェック(もちっとパターンでてから作成
                //--------------------------

                //--------------------------
                //  チェックAPI呼び出し
                //--------------------------
                try
                {
                    var response = await _httpClient.PostAsJsonAsync<LinMsgModels.LineWebhookMessageObject>(CheckURL, data);
                    response.EnsureSuccessStatusCode();
                    log.LogInformation("[MessageEvent] LINEresponse: " + response);

                    log.LogInformation("[MessageEvent] END OK");
                    return new OkResult();
                }
                catch (Exception ex)
                {
                    log.LogInformation("[MessageEvent] END ERR " + ex);
                    return new BadRequestResult();
                }

            }
            else {
                log.LogInformation("[MessageEvent] signatureCHK NG");
                return new BadRequestResult();
            }
        }
    }
}