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
using LINEAPI;

namespace ThingsLine.LINEAPI
{
    public  class LinMsgQReplyMSG000
    {
        private const string _CAtoken = "PAG0ZG/1HqwEw2CF9LL6l+UYBHPQdWR584bM+V0Dcw4GVdlCn+ScKGBK05eGjCQqtPzG4NSDr3i0Me4IKFZtkxh7/PcFj4qJC1iFO3QytzX7ZrdiONlN93bICeLD3/5pSOWrsy5fToIPj5L8lCilngdB04t89/1O/w1cDnyilFU=";
        private readonly HttpClient _httpClient;

        public LinMsgQReplyMSG000()
        {
            _httpClient = new HttpClient();
        }

        [FunctionName("LinMsgQReplyMSG000")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("[LinMsgQReplyMSG000] Start ");
            LinMsgModels LineAPIObj = new LinMsgModels();

            try
            {
                // Get body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // Deserialize
                var data = JsonConvert.DeserializeObject<LinMsgModels.LinMsgTextMSG000>(requestBody);
                log.LogInformation("[LinMsgQReplyMSG000] requestBody: " + requestBody);

                //-------------------------------
                //LINE に送信
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _CAtoken);
                log.LogInformation("[LinMsgQReplyMSG000] : 01");

                if (data.replyToken != "" || data.replyToken != null)
                {
                    log.LogInformation("[LinMsgQReplyMSG000] : replyToken: " + data.replyToken);
                    //リプレイトークンの場合
                    var response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgQReplyMSG000>("https://api.line.me/v2/bot/message/reply"
                    , new LinMsgModels.LinMsgQReplyMSG000()
                    {
                        replyToken = data.replyToken,

                        messages = new List<LinMsgModels.QRMessage>()
                        {
                            new LinMsgModels.QRMessage()
                            {
                                type="text",
                                text = "クイックリプライ",

                                quickReply  = new LinMsgModels.QRQuickReply()
                                {
                                    items = new List<LinMsgModels.QRItem>()
                                    {
                                        new LinMsgModels.QRItem(){
                                            type = "action",
                                            action = new LinMsgModels.QRAction()
                                            {
                                                type = "postback",
                                                label = "購入",
                                                data = "action=buy",
                                                displayText = "購入をタップ"
                                            }
                                        },
                                        new LinMsgModels.QRItem()
                                        {
                                            type = "action",
                                            action = new LinMsgModels.QRAction()
                                            {
                                                type = "message",
                                                label= "メッセージ",
                                                text= "メッセージをタップ"
                                            }
                                        },
                                        new LinMsgModels.QRItem(){
                                            type = "action",
                                            action= new LinMsgModels.QRAction()
                                            {
                                                type = "datetimepicker",
                                                label= "日付を選択",
                                                data = "date=date",
                                                mode = "datetime",
                                                initial = "2019-09-09t00:00",
                                                max = "2020-02-01t23:59",
                                                min = "2019-09-09t00:00"
                                            }
                                        },
                                        new LinMsgModels.QRItem(){
                                            type = "action",
                                            action = new LinMsgModels.QRAction()
                                            {
                                                type = "camera",
                                                label = "カメラ"
                                            }
                                        },
                                        new LinMsgModels.QRItem(){
                                            type = "action",
                                            action = new LinMsgModels.QRAction()
                                            {
                                                type = "cameraRoll",
                                                label = "カメラロール"
                                            }
                                        },
                                        new LinMsgModels.QRItem(){
                                            type = "action",
                                            action = new LinMsgModels.QRAction()
                                            {
                                                type = "location",
                                                label = "ロケーション"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
                    response.EnsureSuccessStatusCode();
                    log.LogInformation("[LinMsgQReplyMSG000] LINEresponse: " + response);
                }
                /*
                else if (data.to != "" || data.to != null)
                {
                    log.LogInformation("[LinMsgQReplyMSG000] : replyToken: " + data.replyToken);
                    //リプレイトークンの場合
                    var response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgQReplyMSG000>("https://api.line.me/v2/bot/message/reply"
                    , new LinMsgModels.LinMsgQReplyMSG000()
                    {
                        to = data.to,
                        messages = new List<LinMsgModels.Message>()
                            {
                            new LinMsgModels.Message(){
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