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

namespace EventAction
{
    public class LinMsgTextMSG000
    {

        private readonly HttpClient _httpClient;

        public LinMsgTextMSG000()
        {
            _httpClient = new HttpClient();
        }

        [FunctionName("LinMsgTextMSG000")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("[LinMsgTextMSG000] Start ");
            module.mdlLINE mLINE = new module.mdlLINE();

            try
            {
                // Get body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // Deserialize
                var data = JsonConvert.DeserializeObject<LinMsgModels.LinMsgTextMSG000>(requestBody);
                log.LogInformation("[LinMsgTextMSG000] requestBody: " + requestBody);


                //-------------------------------
                //送信データ作成
                var sendJSONobj = new LinMsgModels.LinMsgTextMSG000();
                if (data.replyToken != "" || data.replyToken != null)
                {
                    //-------------------------------
                    //リプレイトークンの場合
                    log.LogInformation("[LinMsgLocMSG000] : replyToken: " + data.replyToken);

                    //送信用JSON作成
                    sendJSONobj = new LinMsgModels.LinMsgTextMSG000()
                    {
                        replyToken = data.replyToken,
                        messages = new List<LinMsgModels.Message>()
                            {
                            new LinMsgModels.Message(){
                                type = "text",
                                text = data.messages[0].text
                            }
                        }

                    };
                }
                else if (data.to != "" || data.to != null)
                {
                    //-------------------------------
                    //Toの場合
                    log.LogInformation("[LinMsgLocMSG000] : to: " + data.to);
                    //送信用JSON作成
                    sendJSONobj = new LinMsgModels.LinMsgTextMSG000()
                    {
                        to = data.to,
                        messages = new List<LinMsgModels.Message>()
                            {
                            new LinMsgModels.Message(){
                                type = "text",
                                text = data.messages[0].text
                            }
                        }
                    };
                }

                //-------------------------------
                //LINE に送信
                //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _CAtoken);
                //var response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgTextMSG000>(sURL, sendJSONobj);
                //response.EnsureSuccessStatusCode();
                //log.LogInformation("[LinMsgLocMSG000] LINEresponse: " + response);

                var retST = mLINE.SendMessageAsync(_httpClient, sendJSONobj);
                log.LogInformation("[LinMsgTextMSG000] LINEresponse: " + retST);

            }
            catch (Exception ex) {
                log.LogInformation("[LinMsgTextMSG000] END ERR " + ex);
                return new BadRequestResult();
            }

            log.LogInformation("[LinMsgTextMSG000] END OK");
            return new OkResult();
        }

    }
}