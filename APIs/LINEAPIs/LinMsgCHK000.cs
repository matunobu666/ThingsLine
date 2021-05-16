using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.Logging;


namespace LINEAPI
{
    public class LinMsgCHK000
    {
        private const string _secret = "41553b983f9ad34c11825304908de02d";
        private const string _token = "PAG0ZG/1HqwEw2CF9LL6l+UYBHPQdWR584bM+V0Dcw4GVdlCn+ScKGBK05eGjCQqtPzG4NSDr3i0Me4IKFZtkxh7/PcFj4qJC1iFO3QytzX7ZrdiONlN93bICeLD3/5pSOWrsy5fToIPj5L8lCilngdB04t89/1O/w1cDnyilFU=";
        private readonly HttpClient _httpClient;
        public LinMsgCHK000() { _httpClient = new HttpClient(); }

        public class DD_Soracom000
        {
            public DateTime dt { get; set; }
            public string imsi { get; set; }
            public string imei { get; set; }
            public string operatorId { get; set; }
            public Double d_lat { get; set; }
            public Double d_lon { get; set; }
            
            public int d_bat { get; set; }
            public int d_rs { get; set; }
            public Double d_temp { get; set; }
            public Double d_humi { get; set; }
            public int d_a_x { get; set; }
            public int d_a_y { get; set; }
            public int d_a_z { get; set; }
            public int d_type { get; set; }
            public DD_Soracom000()
            {

            }
        }
        public class baseData
        {
            //   UDevice.userID, UDevice.imsi,UDevice.imei"
            public string userID { get; set; }
            public string imsi { get; set; }
            public baseData()
            {

            }
        }

        public class l_GroupInfo
        {
            //   UDevice.userID, UDevice.imsi,UDevice.imei"
            public string groupId { get; set; }
            public string groupName { get; set; }
        }
        public class l_Group
        {
            //   UDevice.userID, UDevice.imsi,UDevice.imei"
            public string groupId { get; set; }
            public string userId { get; set; }
        }


        private const string stURL_LinMsgTextMSG000 = "https://thingslineapis.azurewebsites.net/api/LinMsgTextMSG000?code=A7A/23M//B7Rd/93oLgkuA3sOoHQFEMomZ6syanJsI7FV8OISW32aw==";
        private const string stURL_LinMsgLocMSG000 = "https://thingslineapis.azurewebsites.net/api/LinMsgLocMSG000?code=SBPFGND3Gvf8kOUYnS3azsf8clb3jVN3g0YIC7Ki5LKCwerAqilS9g==";
        private const string stURL_LinMsgQReplyMSG000 = "https://thingslineapis.azurewebsites.net/api/LinMsgQReplyMSG000?code=eiJI3/wAaq1IIa7z6XNYrHzhJi8qSHVVmdLb2M8TQMfCuSuZbYQmZA==";
        private const string stURL_LinImg000 = "https://thingslineapis.azurewebsites.net/api/LinImg000?code=5tfXRc4j8rFXqIrkr6RfdAnxruho5anVeMMnVwRl/p85GE5d4rFxXQ==";

        
        [FunctionName("LinMsgCHK000")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("[LinMsgCHK000] >>>>>>>>>>>>>>>>>>>>>Start ");
            LinMsgModels LineAPIObj = new LinMsgModels();

            module.mdlSQLServer mSQLServer = new module.mdlSQLServer();
            StringBuilder sSQL = new StringBuilder();

            module.modThingsLine mThingsLine = new module.modThingsLine();

            //---------------------
            // body取得
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation("[LinMsgCHK000] requestBody: " + requestBody);

            var response = new HttpResponseMessage();

            try {
                // Deserialize
                var requestBodydata = JsonConvert.DeserializeObject<LinMsgModels.LineWebhookMessageObject>(requestBody);
                string sendJSON = "";


                //---------------------
                // 必須チェック
                log.LogInformation("[LinMsgCHK000] replyToken:  " + requestBodydata.events[0].replyToken);
                if (requestBodydata.events[0].replyToken == "" || requestBodydata.events[0].replyToken == null) {
                    log.LogInformation("[LinMsgCHK000][ERROR] e001");
                    return new BadRequestResult();
                }
                //---------------------------------------------------------------
                // グループ処理
                //---------------------------------------------------------------
                string tmpGroupID = requestBodydata.events[0].source.roomId;
                if (requestBodydata.events[0].source.groupId != null && requestBodydata.events[0].source.groupId != "") {
                     tmpGroupID = requestBodydata.events[0].source.groupId;
                }

                log.LogInformation("[LinMsgCHK000] tmpGroupID: " + tmpGroupID);

                if (tmpGroupID != null && tmpGroupID != "")
                {
                    //既存判定（l_GroupInfo）
                    sSQL.Clear();
                    sSQL.Append("SELECT groupId, groupName"
                                        + " FROM[dbo].[l_GroupInfo]"
                                        + "  where groupId = '" + tmpGroupID + "'");

                    log.LogInformation("[MessageEvent] sql : " + sSQL.ToString());
                    List<l_GroupInfo> retGroupInfo = mSQLServer.GetSQL<l_GroupInfo>(sSQL);
                    log.LogInformation("[MessageEvent] retGroupInfo.Count : " + retGroupInfo.Count.ToString());

                    if (retGroupInfo.Count == 0) {
                        //追加処理
                        sSQL.Clear();
                        sSQL.Append("insert into  [dbo].[l_GroupInfo]("
                            + "  [groupId]"
                            + " ,[groupName]"
                            + " ,[type]"
                            + " ,[timestamp]"
                            + " ,[Status]"
                            + ")"
                            + " VALUES  ( "
                            + "  '" + tmpGroupID + "'"
                            + ", '" + "" + "'"
                            + ",'1'"
                            + ",dateadd(hour,9,'" + DateTime.Now + "')"
                            + ",'0'"
                            + ")"
                            );

                        log.LogInformation("[MessageEvent] sql : " + sSQL.ToString());
                        Exception retB1 = mSQLServer.setSQL(sSQL.ToString());
                    }



                    //既存判定（l_Group）
                    sSQL.Clear();
                    sSQL.Append("SELECT groupId, userId"
                                        + " FROM[dbo].[l_Group]"
                                        + "  where groupId = '" + tmpGroupID + "'"
                                        + "  and userId = '" + requestBodydata.events[0].source.userId + "'");


                    log.LogInformation("[MessageEvent] sql : " + sSQL.ToString());
                    List<l_Group> retGroup = mSQLServer.GetSQL<l_Group>(sSQL);
                    log.LogInformation("[MessageEvent] retGroup.Count : " + retGroup.Count.ToString());
                    if (retGroup.Count == 0)
                    {
                        //追加処理
                        sSQL.Clear();
                        sSQL.Append("insert into  [dbo].[l_Group]("
                            + "  [groupId]"
                            + " ,[userId]"
                            + " ,[type]"
                            + " ,[timestamp]"
                            + " ,[Status]"
                            + ")"
                            + " VALUES  ( "
                            + "  '" + tmpGroupID + "'"
                            + ", '" + requestBodydata.events[0].source.userId + "'"
                            + ",'1'"
                            + ",dateadd(hour,9,'" + DateTime.Now + "')"
                            + ",'0'"
                            + ")"
                            );
                        log.LogInformation("[MessageEvent] sql : " + sSQL.ToString());
                        Exception retB1 = mSQLServer.setSQL(sSQL.ToString());
                    }
                }




                //---------------------------------------------------------------
                // テキストイベント
                //---------------------------------------------------------------
                if (requestBodydata.events[0].message.type == "text")
                {

                    //---------------------
                    // 監視開始イベント（仮置き
                    if (requestBodydata.events[0].message.text == "監視開始")
                    {
                        log.LogInformation("[LinMsgCHK000] テキスト＞監視開始 ");
                        //返信JSON作成
                        var sendJSONobj = new LinMsgModels.LinMsgTextMSG000()
                        {
                            replyToken = requestBodydata.events[0].replyToken,
                            messages = new List<LinMsgModels.Message>()
                        {
                            new LinMsgModels.Message()
                            {
                                type = "text",
                                text = requestBodydata.events[0].message.text
                            }
                        }
                        };
                        //MSG送信処理(LinMsgTextMSG000)
                        response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgTextMSG000>(stURL_LinMsgTextMSG000, sendJSONobj);
                        sendJSON = sendJSONobj.ToString();

                    }

                    //---------------------
                    // 監視解除イベント（仮置き
                    else if (requestBodydata.events[0].message.text == "監視解除")
                    {
                        //返信JSON作成
                        var sendJSONobj = new LinMsgModels.LinMsgTextMSG000()
                        {
                            replyToken = requestBodydata.events[0].replyToken,
                            messages = new List<LinMsgModels.Message>()
                        {
                            new LinMsgModels.Message()
                            {
                                type = "text",
                                text = requestBodydata.events[0].message.text
                            }
                        }
                        };
                        //MSG送信処理(LinMsgTextMSG000)
                        response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgTextMSG000>(stURL_LinMsgTextMSG000, sendJSONobj);
                        sendJSON = sendJSONobj.ToString();

                    }

                    //---------------------
                    // 状態確認イベント（仮置き
                    else if (requestBodydata.events[0].message.text == "状態確認")
                    {
                        log.LogInformation("[LinMsgCHK000] テキスト＞状態確認 ");
                        //必要データ収集
                        //  基本情報
                        sSQL.Clear();
                        sSQL.Append( "SELECT UDevice.userID, UDevice.imsi"
                                            + " FROM[dbo].[U_Device] as UDevice"
                                            + " where UDevice.userID = (SELECT [Id] FROM[dbo].[AspNetUsers] as ANUser"
                                            + " where(ANUser.[Email] = '" + requestBodydata.events[0].source.userId  + "@line.com')"
                                            + " and UDevice.stopFLG = 0)");
                        List<baseData> rets = mSQLServer.GetSQL<baseData>(sSQL);



                        if (rets.Count == 0 ) {
                            sSQL.Clear();
                            sSQL.Append("SELECT UDevice.userID, UDevice.imsi"
                                                + " FROM[dbo].[U_Device] as UDevice"
                                                + " where UDevice.userID = (SELECT [Id] FROM[dbo].[AspNetUsers] as ANUser"
                                                + " where(ANUser.[Email] = 'U25758193796510fa7103410e0e9558d9' + '@line.com')"
                                                + " and UDevice.stopFLG = 0)");
                             rets = mSQLServer.GetSQL<baseData>(sSQL);

                        }

                        log.LogInformation("[LinMsgCHK000] sSQL: " + rets[0].userID + ":" + rets[0].imsi);
                        //---------------------
                        //  最終位置
                        sSQL.Clear();
                        sSQL.Append("SELECT TOP (1) dt,imsi,imei,operatorId,d_lat,d_lon,d_bat,d_rs,d_temp,d_humi,d_a_x,d_a_y,d_a_z,d_type "
                                + " FROM[dbo].[DD_Soracom000]"
                                + "  where imsi = '" + rets[0].imsi + "'"
                                + "     and d_lat IS NOT NULL"
                                + "     and d_lon IS NOT NULL"
                                + " order by dt desc");
                        
                        List<DD_Soracom000> retDD_Soracom000 = mSQLServer.GetSQL<DD_Soracom000>(sSQL);

                        //返信JSON作成
                        string stitle = retDD_Soracom000[0].dt.ToString("yyyy/MM/dd HH:mm:ss");
                        string saddress = ""
                                        + " Type:" + retDD_Soracom000[0].d_type.ToString()
                                        + " 温度:" + retDD_Soracom000[0].d_temp.ToString()
                                        + " 湿度:" + retDD_Soracom000[0].d_humi.ToString()
                                        + " BAT:" + retDD_Soracom000[0].d_bat.ToString()
                                        ;
                        Double dlatitude = retDD_Soracom000[0].d_lat;
                        Double dlongitude = retDD_Soracom000[0].d_lon;
                        //---------------------
                        //---------------------
                        var sendJSONobj = new LinMsgModels.LinMsgLocMSG000()
                        {
                            replyToken = requestBodydata.events[0].replyToken,
                            messages = new List<LinMsgModels.Location>()
                        {
                            new LinMsgModels.Location()
                            {
                                type = "location",
                                title= stitle,
                                address= saddress,
                                latitude= dlatitude,
                                longitude= dlongitude
                            }
                        },
                            sources = new LinMsgModels.Source()
                            {

                                type = "text",
                                userId = requestBodydata.events[0].source.userId

                            }

                        };
                        //MSG送信処理(LinMsgLocMSG000)
                        response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgLocMSG000>(stURL_LinMsgLocMSG000, sendJSONobj);
                        sendJSON = sendJSONobj.ToString();

                    }
                    //---------------------
                    // テストイベント（仮置き
                    else if (requestBodydata.events[0].message.text == "てすと")
                    {
                        log.LogInformation("[LinMsgCHK000] テキスト＞てすと ");

                        //返信JSON作成
                        var sendJSONobj = new LinMsgModels.LinMsgTextMSG000()
                        {
                            replyToken = requestBodydata.events[0].replyToken,
                            messages = new List<LinMsgModels.Message>()
                        {
                            new LinMsgModels.Message()
                            {
                                type = "text",
                                text = requestBodydata.events[0].message.text
                            }
                        }
                        };

                        //MSG送信処理(LinMsgQReplyMSG000)
                        response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgTextMSG000>(stURL_LinMsgQReplyMSG000, sendJSONobj);
                        sendJSON = sendJSONobj.ToString();

                    }

                    //---------------------
                    // 不適合MSG処理
                    else
                    {
                        log.LogInformation("[LinMsgCHK000] 不明MSG ");
                        //返信JSON作成
                        var sendJSONobj = new LinMsgModels.LinMsgTextMSG000()
                        {
                            replyToken = requestBodydata.events[0].replyToken,
                            messages = new List<LinMsgModels.Message>()
                        {
                            new LinMsgModels.Message()
                            {
                                type = "text",
                                text = "不明なメッセージです"
                            }
                        }
                        };
                        //MSG送信処理(LinMsgTextMSG000)
                        response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgTextMSG000>(stURL_LinMsgLocMSG000, sendJSONobj);
                        sendJSON = sendJSONobj.ToString();

                    }
                    //---------------------
                    // 監視開始イベント（仮置き
                }
                //---------------------------------------------------------------
                // イメージイベント
                //---------------------------------------------------------------
                else if (requestBodydata.events[0].message.type == "image")
                {
                    log.LogInformation("[LinMsgCHK000] イメージ処理-----------------------------------------");
                    //返信JSON作成
                    var sendJSONobj = new LinMsgModels.LinImg000();

                    sendJSONobj.Event = requestBodydata.events[0];

                    //MSG送信処理(LinMsgTextMSG000)
                    response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinImg000>(stURL_LinImg000, sendJSONobj);
                    sendJSON = sendJSONobj.ToString();
                }
                //---------------------------------------------------------------
                // 参加イベント
                //---------------------------------------------------------------
                else if (requestBodydata.events[0].type == "join")
                {
                    log.LogInformation("[LinMsgCHK000] 参加イベント処理-----------------------------------------");
                    //返信JSON作成
                    var sendJSONobj = new LinMsgModels.LinMsgTextMSG000()
                    {
                        replyToken = requestBodydata.events[0].replyToken,
                        messages = new List<LinMsgModels.Message>()
                        {
                            new LinMsgModels.Message()
                            {
                                type = "text",
                                text = "参加イベント"
                            }
                        }
                    };
                    //MSG送信処理(LinMsgTextMSG000)
                    response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgTextMSG000>(stURL_LinMsgTextMSG000, sendJSONobj);
                    sendJSON = sendJSONobj.ToString();

                }
                //---------------------------------------------------------------
                // 退出イベント
                //---------------------------------------------------------------
                else if (requestBodydata.events[0].type == "leave")
                {
                    log.LogInformation("[LinMsgCHK000] 退出イベント処理-----------------------------------------");
                    //返信JSON作成
                    var sendJSONobj = new LinMsgModels.LinMsgTextMSG000()
                    {
                        replyToken = requestBodydata.events[0].replyToken,
                        messages = new List<LinMsgModels.Message>()
                        {
                            new LinMsgModels.Message()
                            {
                                type = "text",
                                text = "退出イベント"
                            }
                        }
                    };
                    //MSG送信処理(LinMsgTextMSG000)
                    response = await _httpClient.PostAsJsonAsync<LinMsgModels.LinMsgTextMSG000>(stURL_LinMsgTextMSG000, sendJSONobj);
                    sendJSON = sendJSONobj.ToString();

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
                    + ",'2'"
                    + ",'LinMsgCHK000'"
                    + ",''"
                    + ", '" + requestBody + "'"
                    + ", '" + sendJSON + "'"
                    + ",''"
                    + ")"
                    );
                log.LogInformation("[MessageEvent] sql : " + sSQL.ToString());

                Exception retB = mSQLServer.setSQL(sSQL.ToString());

                //---------------------
                // 成功処理
                response.EnsureSuccessStatusCode();
                log.LogInformation("[LinMsgCHK000] LINEresponse: " + response);
                log.LogInformation("[LinMsgCHK000] END OK");
                return new OkResult();
                
            }
            catch(Exception ex)
            {
                //---------------------
                // エラー処理
                log.LogError("[LinMsgCHK000] END NG :" + ex);
                return new BadRequestResult();
            }
        }




    }
}
