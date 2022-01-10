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
using ThingsLineAPIs.Modules;
using ThingsLineAPIs.Models;
using ThingsLine.Modules;
using System.Text;
using static ThingsLine.Models.mdlLINE;

namespace ThingsLineAPIs.LINEAPIs
{
    public  class LinMsgLocMSG000
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
        public LinMsgLocMSG000(){ _httpClient = new HttpClient();}
        static readonly HttpClient client = new HttpClient();

        [FunctionName("LinMsgLocMSG000")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            Console.WriteLine("[LinMsgLocMSG000] Start ");
            modLINEMsg mLINE = new modLINEMsg();

            try
            {
                //-------------------------------
                // Get body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // Deserialize
                var data = JsonConvert.DeserializeObject<LINElocationMSG>(requestBody);
                Console.WriteLine("[LinMsgLocMSG000] requestBody: " + requestBody);

                //-------------------------------
                //必須チェック

                //-------------------------------
                //送信データ作成
                var sendJSONobj = new LINElocationMSG();
                Console.WriteLine("[LinMsgLocMSG000] : to: ---" + data.to + "---");

                //送信用JSON作成
                sendJSONobj = new LINElocationMSG()
                {
                    to = data.to,
                    replyToken = data.replyToken,
                    messages = new List<Location>()
                        {
                        new Location(){
                            type = data.messages[0].type
                            ,title= data.messages[0].title
                            ,address= data.messages[0].address
                            ,latitude= data.messages[0].latitude
                            ,longitude= data.messages[0].longitude
                        }
                    }
                };
                //-------------------------------
                //LINE に送信(to)
                var retST = mLINE.SendMessageAsyncTo(_httpClient, sendJSONobj);
                Console.WriteLine("[LinMsgLocMSG000] LINEresponse: " + retST);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[LinMsgLocMSG000] END ERR " + ex);
                return new BadRequestResult();
            }

            Console.WriteLine("[LinMsgLocMSG000] END(OK)");
            return new OkResult();
        }

    }
}