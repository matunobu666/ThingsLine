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
using System.Security.Cryptography;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using ThingsLineAPIs.Modules;
using ThingsLineAPIs.Models;
using ThingsLine.Modules;
using static ThingsLine.Models.mdlLINE;

namespace ThingsLineAPIs.LINEAPIs
{
    public  class LinMsgQReplyMSG000
    {

        //-------------------------------------------------
        //ThingsLineAPIs.Modules
        //LINE
        mdlLineMsg LineAPIObj = new mdlLineMsg();
        modLINEMsg mLINE = new modLINEMsg();
        //SQLserver
        modSQLServer mSQLServer = new modSQLServer();
        StringBuilder sSQL = new StringBuilder();

        //-------------------------------------------------
        private readonly HttpClient _httpClient;
        public LinMsgQReplyMSG000(){_httpClient = new HttpClient();}

        [FunctionName("LinMsgQReplyMSG000")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("[LinMsgQReplyMSG000] Start ");
            mdlLineMsg LineAPIObj = new mdlLineMsg();


            try
            {
                // Get body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // Deserialize
                var data = JsonConvert.DeserializeObject<LINEtextMSG>(requestBody);
                log.LogInformation("[LinMsgQReplyMSG000] requestBody: " + requestBody);



                if (data.replyToken != "" || data.replyToken != null)
                {
                    log.LogInformation("[LinMsgQReplyMSG000] : replyToken: " + data.replyToken);
                    //リプレイトークンの場合
                    var response = await _httpClient.PostAsJsonAsync<mdlLineMsg.LinMsgQReplyMSG000>("https://api.line.me/v2/bot/message/reply"
                    , new mdlLineMsg.LinMsgQReplyMSG000()
                    {
                        replyToken = data.replyToken,

                        messages = new List<mdlLineMsg.QRMessage>()
                        {
                            new mdlLineMsg.QRMessage()
                            {
                                type="text",
                                text = "クイックリプライ",
                                quickReply  = new mdlLineMsg.QRQuickReply()
                                {
                                    items = new List<mdlLineMsg.QRItem>()
                                    {
                                        new mdlLineMsg.QRItem(){
                                            type = "action",
                                            action = new mdlLineMsg.QRAction()
                                            {
                                                type = "camera",
                                                label = "カメラ"
                                            }
                                        },
                                        new mdlLineMsg.QRItem(){
                                            type = "action",
                                            action = new mdlLineMsg.QRAction()
                                            {
                                                type = "cameraRoll",
                                                label = "カメラロール"
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    });
                    response.EnsureSuccessStatusCode();
                    log.LogInformation("[LinMsgQReplyMSG000] LINEresponse: " + response);
                }


                //-------------------------------
                //LINE に送信
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mdlLineMsg._token);
                log.LogInformation("[LinMsgQReplyMSG000] : 01");

                /*
                else if (data.to != "" || data.to != null)
                {
                    log.LogInformation("[LinMsgQReplyMSG000] : replyToken: " + data.replyToken);
                    //リプレイトークンの場合
                    var response = await _httpClient.PostAsJsonAsync<mdlLineMsg.LinMsgQReplyMSG000>("https://api.line.me/v2/bot/message/reply"
                    , new mdlLineMsg.LinMsgQReplyMSG000()
                    {
                        to = data.to,
                        messages = new List<mdlLineMsg.Message>()
                            {
                            new mdlLineMsg.Message(){
                                type = "text",
                                text = data.messages[0].text
                            }
                        }
                    });
                    response.EnsureSuccessStatusCode();
                    log.LogInformation("[LinMsgQReplyMSG000] LINEresponse: " + response);
                }
                */

            }
            catch (Exception ex)
            {
                log.LogInformation("[LinMsgQReplyMSG000] END ERR " + ex);
                return new BadRequestResult();
            }

            log.LogInformation("[LinMsgQReplyMSG000] END OK");
            return new OkResult();
        }

    }
}