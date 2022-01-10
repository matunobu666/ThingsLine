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
    public class LinMsgTextMSG000
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
        public LinMsgTextMSG000(){_httpClient = new HttpClient();}

        [FunctionName("LinMsgTextMSG000")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,ILogger log)
        {
            Console.WriteLine("[LinMsgTextMSG000] Start ");
            modLINEMsg mLINE = new modLINEMsg();

            try
            {
                // Get body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // Deserialize
                var data = JsonConvert.DeserializeObject<LINEtextMSG>(requestBody);
                Console.WriteLine("[LinMsgTextMSG000] requestBody: " + requestBody);


                //-------------------------------
                //送信データ作成
                var sendJSONobj = new LINEtextMSG();
                if (data.replyToken != null)
                {
                    //-------------------------------
                    //リプレイトークンの場合
                    Console.WriteLine("[LinMsgLocMSG000] : replyToken: " + data.replyToken);

                    //送信用JSON作成
                    sendJSONobj = new LINEtextMSG()
                    {
                        replyToken = data.replyToken,
                        messages = new List<Message>()
                            {
                            new Message(){
                                type = "text",
                                text = data.messages[0].text
                            }
                        }

                    };
                }
                else if (data.to != null)
                {
                    //-------------------------------
                    //Toの場合
                    Console.WriteLine("[LinMsgLocMSG000] : to: " + data.to);
                    //送信用JSON作成
                    sendJSONobj = new LINEtextMSG()
                    {
                        to = data.to,
                        messages = new List<Message>()
                            {
                            new Message(){
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
                //var response = await _httpClient.PostAsJsonAsync<mdlLineMsg.LinMsgTextMSG000>(sURL, sendJSONobj);
                //response.EnsureSuccessStatusCode();
                //Console.WriteLine("[LinMsgLocMSG000] LINEresponse: " + response);

                var retST = mLINE.SendMessageAsyncTo(_httpClient, sendJSONobj);
                Console.WriteLine("[LinMsgTextMSG000] LINEresponse: " + retST);

            }
            catch (Exception ex) {
                Console.Error.WriteLine("[LinMsgTextMSG000] END ERR " + ex);
                return new BadRequestResult();
            }

            Console.WriteLine("[LinMsgTextMSG000] END OK");
            return new OkResult();
        }

    }
}