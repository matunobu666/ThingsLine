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
using System.Collections.Generic;
using System.Net.Http.Headers;
using LINEAPI;


namespace ThingsLine.LINEAPI
{
    public  class LinMsgLocMSG000
    {

        private readonly HttpClient _httpClient;

        public LinMsgLocMSG000()
        {
            _httpClient = new HttpClient();
        }

        static readonly HttpClient client = new HttpClient();

        [FunctionName("LinMsgLocMSG000")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("[LinMsgLocMSG000] Start ");
            module.mdlLINE mLINE = new module.mdlLINE();

            try
            {
                //-------------------------------
                // Get body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // Deserialize
                var data = JsonConvert.DeserializeObject<LinMsgModels.LinMsgLocMSG000>(requestBody);
                log.LogInformation("[LinMsgLocMSG000] requestBody: " + requestBody);


                //-------------------------------
                //必須チェック

                //-------------------------------
                //送信データ作成
                var sendJSONobj = new LinMsgModels.LinMsgLocMSG000();
                if (data.replyToken != "" || data.replyToken != null)
                {
                    //-------------------------------
                    //リプレイトークンの場合
                    log.LogInformation("[LinMsgLocMSG000] : replyToken: " + data.replyToken);

                    //送信用JSON作成
                    sendJSONobj = new LinMsgModels.LinMsgLocMSG000()
                    {
                        replyToken = data.replyToken,
                        messages = new List<LinMsgModels.Location>()
                            {
                            new LinMsgModels.Location(){
                                type = data.messages[0].type
                                ,title= data.messages[0].title
                                ,address= data.messages[0].address
                                ,latitude= data.messages[0].latitude
                                ,longitude= data.messages[0].longitude
                            }
                        }
                    };


                }
                else if (data.to != "" || data.to != null)
                {
                    //-------------------------------
                    //Toの場合(未作成）
                    log.LogInformation("[LinMsgLocMSG000] : to: " + data.to);
                    //送信用JSON作成
                    sendJSONobj = new LinMsgModels.LinMsgLocMSG000()
                    {
                        to = data.to,
                        messages = new List<LinMsgModels.Location>()
                            {
                            new LinMsgModels.Location(){
                                type = "location",
                                title= "バイク名",
                                address="温度　湿度",
                                latitude= 35.688806,
                                longitude= 139.701739
                            }
                        }
                    };


                }



                //-------------------------------
                //LINE に送信
                //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _CAtoken);
                //var response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgLocMSG000>(sURL, sendJSONobj);
                //response.EnsureSuccessStatusCode();

                var retST = mLINE.SendMessageAsync(_httpClient, sendJSONobj);


                log.LogInformation("[LinMsgLocMSG000] LINEresponse: " + retST);

            }
            catch (Exception ex)
            {
                log.LogError("[LinMsgLocMSG000] END ERR " + ex);
                return new BadRequestResult();
            }

            log.LogInformation("[LinMsgLocMSG000] END(OK)");
            return new OkResult();
        }

    }
}