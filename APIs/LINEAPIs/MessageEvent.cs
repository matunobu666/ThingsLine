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
using System.Net.Http;
using System.Text;
using ThingsLine.Modules;
using ThingsLineAPIs.Modules;
using ThingsLineAPIs.Models;
using ThingsLine.Models;

namespace ThingsLineAPIs.LINEAPIs
{
    public class MessageEvent
    {
        //LINE関連
        //-------------------------------------------------
        //ThingsLineAPIs.Modules
        modLINEMsg mLINEMsg = new modLINEMsg();
        mdlLineMsg LineAPIObj = new mdlLineMsg();
        modLINE mLINE = new modLINE();
        modLINEGroup mLINEGroup = new modLINEGroup();
        modSYS mSYS = new modSYS();
        //SQLserver
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        modBikeLocation mBikeLocation = new modBikeLocation();
        modTheftMonitoring mTheftMonitoring = new modTheftMonitoring();
        //-------------------------------------------------
        //LINEメッセージイベントAPI
        //-------------------------------------------------
        private readonly HttpClient _httpClient;
        public MessageEvent() { _httpClient = new HttpClient(); }

        [FunctionName("MessageEvent")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            Console.WriteLine("[MessageEvent] Start ");
            Console.WriteLine("[MessageEvent] Req: " + req.Body.ToString());

            //--------------------------
            // ヘッダー取得 (signature)
            req.Headers.TryGetValue("X-Line-Signature", out var xlinesignature);
            Console.WriteLine("[MessageEvent] Headers: " + req.Headers.ToString());

            //--------------------------
            // ボディ取得
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Console.WriteLine("[MessageEvent] requestBody: " + requestBody.ToString());



            //-------------------------------
            //シグネイチャチェック
            if (mLINE.SingatureCHK(xlinesignature, requestBody))
            {
                Console.WriteLine("[MessageEvent] signatureCHK OK");

                try
                {

                    // Deserialize
                    var requestBodydata = JsonConvert.DeserializeObject<mdlLineMsg.LineWebhookMessageObject>(requestBody);









                    //--------------------------
                    // メッセージイベント
                    if (requestBodydata.events[0].type == "message")
                    {
                        //---------------------------------------------------------------
                        // テキストイベント
                        //---------------------------------------------------------------
                        if (requestBodydata.events[0].message.type == "text")
                        {
                            //---------------------
                            // 「監視開始」イベント
                            if (requestBodydata.events[0].message.text == "監視開始")
                            {
                                //監視開始処理
                                Exception EX = mTheftMonitoring.TheftMonitoringStart(requestBodydata.events[0].source.userId);
                            }

                            //---------------------
                            // 「監視解除」イベント
                            else if (requestBodydata.events[0].message.text == "監視解除")
                            {
                                Exception EX = mTheftMonitoring.TheftMonitoringEnd(requestBodydata.events[0].source.userId, "");
                            }

                            //---------------------
                            // 「状態確認」イベント
                            else if (requestBodydata.events[0].message.text == "状態確認")
                            {
                                Exception EX = mBikeLocation.BikeLocation(requestBodydata.events[0].source.userId);
                            }


                            //---------------------
                            // 不適合MSG処理
                            else
                            {
                                Console.WriteLine("[LinMsgCHK000] 不明MSG ");
                                //MSG送信
                                Task<Exception> EX = mLINEMsg.sendTextMSGAsync(requestBodydata.events[0].replyToken, "不明なメッセージです");
                            }
                        }
                        //---------------------------------------------------------------
                        // イメージイベント
                        //---------------------------------------------------------------
                        else if (requestBodydata.events[0].message.type == "image")
                        {
                            Console.WriteLine("[LinMsgCHK000] image");
                            var sendJSONobj = new mdlLineMsg.LinImg000();
                            sendJSONobj.Event = requestBodydata.events[0];
                            var response = new HttpResponseMessage();
                            response = await _httpClient.PostAsJsonAsync<mdlLineMsg.LinImg000>(mdlLineMsg.stURL_LinImg000, sendJSONobj);
                        }
                        // 動画メッセージ
                        if (requestBodydata.events[0].message.type == "video")
                        {
                            Console.WriteLine("[MessageEvent] Message is : video :" + requestBodydata.events[0].message.contentProvider.originalContentUrl);
                            Console.WriteLine("[MessageEvent] Message is : video :" + requestBodydata.events[0].message.contentProvider.previewImageUrl);
                        }
                        // 位置情報メッセージ
                        if (requestBodydata.events[0].message.type == "location")
                        {
                            Console.WriteLine("[MessageEvent] Message is : location :" + requestBodydata.events[0].message.address);
                            Console.WriteLine("[MessageEvent] Message is : location :" + requestBodydata.events[0].message.latitude);
                            Console.WriteLine("[MessageEvent] Message is : location :" + requestBodydata.events[0].message.longitude);
                        }
                    }
                    //--------------------------
                    // フォローイベント
                    if (requestBodydata.events[0].type == "follow")
                    {
                        Console.WriteLine("[MessageEvent] Message is : follow :" + requestBodydata.events[0].message.contentProvider.originalContentUrl);
                        Console.WriteLine("[MessageEvent] Message is : follow type :" + requestBodydata.events[0].source.type);
                        Console.WriteLine("[MessageEvent] Message is : follow userId :" + requestBodydata.events[0].source.userId);
                    }
                    //--------------------------
                    //  フォロー解除イベント
                    if (requestBodydata.events[0].type == "unfollow")
                    {
                        Console.WriteLine("[MessageEvent] Message is : unfollow :" + requestBodydata.events[0].message.contentProvider.originalContentUrl);
                        Console.WriteLine("[MessageEvent] Message is : unfollow type :" + requestBodydata.events[0].source.type);
                        Console.WriteLine("[MessageEvent] Message is : unfollow userId :" + requestBodydata.events[0].source.userId);
                    }
                    //--------------------------
                    //  参加イベント
                    if (requestBodydata.events[0].type == "join")
                    {
                        Console.WriteLine("[MessageEvent] Message is : join :" + requestBodydata.events[0].message.contentProvider.originalContentUrl);
                        Console.WriteLine("[MessageEvent] Message is : join type :" + requestBodydata.events[0].source.type);
                        Console.WriteLine("[MessageEvent] Message is : join groupId :" + requestBodydata.events[0].source.groupId);
                    }
                    //--------------------------
                    //  退出イベント
                    if (requestBodydata.events[0].type == "leave")
                    {
                        Console.WriteLine("[MessageEvent] Message is : leave :" + requestBodydata.events[0].message.contentProvider.originalContentUrl);
                        Console.WriteLine("[MessageEvent] Message is : leave type :" + requestBodydata.events[0].source.type);
                        Console.WriteLine("[MessageEvent] Message is : leave groupId :" + requestBodydata.events[0].source.groupId);
                    }
                    //--------------------------
                    //  メンバー参加イベント
                    if (requestBodydata.events[0].type == "memberJoined")
                    {
                        Console.WriteLine("[MessageEvent] Message is : memberJoined :" + requestBodydata.events[0].message.contentProvider.originalContentUrl);
                        Console.WriteLine("[MessageEvent] Message is : memberJoined type :" + requestBodydata.events[0].source.type);
                        Console.WriteLine("[MessageEvent] Message is : memberJoined groupId :" + requestBodydata.events[0].source.groupId);
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
                        + ", '" + requestBodydata.events[0].type + "'"
                        + ", '" + requestBody + "'"
                        + ")"
                        );
                    Console.WriteLine("[MessageEvent] sql : " + sSQL.ToString());

                    Exception retEX = mSQLServer.setSQL(sSQL.ToString());

                    Console.WriteLine("[MessageEvent] retB : " + retEX);

                    //--------------------------
                    //  チェックAPI呼び出し
                    //--------------------------
                    Console.WriteLine("[MessageEvent] CHKAPI Start!");
                    Console.WriteLine("[MessageEvent] END OK");
                    return new OkResult();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("[MessageEvent] END ERR " + ex);
                    Console.Error.WriteLine("[MessageEvent]  NG");
                    mSYS.Log2DBERR(GetType().FullName, "MessageEvent", ex.ToString());
                   return new BadRequestResult();
                }
            }
            else
            {
                Console.Error.WriteLine("[MessageEvent]  NG");
                return new BadRequestResult();
            }
        }
    }
}