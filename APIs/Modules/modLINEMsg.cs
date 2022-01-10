using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Configuration;
using ThingsLine.Modules;
using ThingsLine.Models;
using ThingsLineAPIs.Modules;
using ThingsLineAPIs.Models;
using static ThingsLine.Models.mdlLINE;

namespace ThingsLineAPIs.Modules
{
    public class modLINEMsg
    {
        private static readonly string GetImgURL1 = "https://api-data.line.me/v2/bot/message/";
        private static readonly string GetImgURL2 = "/content";

        private readonly HttpClient _httpClient;
        public modLINEMsg() { _httpClient = new HttpClient(); }



        //-------------------------------
        //LINE に送信
        //-------------------------------        
        public Task<HttpResponseMessage> SendMessageAsync(HttpClient _httpClient, object req)
        {
            Console.WriteLine("[SendMessageAsync] start ");
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mdlLineMsg._token);
                Task<HttpResponseMessage> response = _httpClient.PostAsJsonAsync(mdlLineMsg.ReplyURL, req);

                return response;

            }
            catch (Exception ex)
            {

                Console.Error.WriteLine("[SendMessageAsync] ERR: " + ex.Message.ToString());
                Console.Error.WriteLine("[SendMessageAsync] END(Err) ");

                Console.WriteLine("[GetImageAsync] END(OK) ");

                return null;

            }

        }
        //-------------------------------
        //LINE に送信(to)
        //-------------------------------        
        public Task<HttpResponseMessage> SendMessageAsyncTo(HttpClient _httpClient, object req)
        {
            Console.WriteLine("[SendMessageAsyncTo] start ");
            Console.WriteLine("[SendMessageAsyncTo] req : " + req.ToString()); 

            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mdlLineMsg._token);
                Task<HttpResponseMessage> response =  _httpClient.PostAsJsonAsync(mdlLineMsg.PushURL , req);

                Console.WriteLine("[SendMessageAsyncTo] END(OK) ");
                return response;

            }
            catch (Exception ex)
            {

                Console.Error.WriteLine("[SendMessageAsyncTo] ERR: " + ex.Message.ToString());
                Console.Error.WriteLine("[SendMessageAsyncTo] END(Err) ");
                return null;

            }

}
//-----------------------------------
//画像の保存
//-------------------------------        
public async Task<Stream> GetImageAsync(HttpClient _httpClient, string getMessageID)
        {

            Console.WriteLine("[GetImageAsync] start ");
            try
            {
                Console.WriteLine("[GetImageAsync] getMessageID: " + getMessageID);


                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mdlLineMsg._token);

                Console.WriteLine("[GetImageAsync] GetImgURL: " + GetImgURL1 + getMessageID + GetImgURL2);

                Stream GetImage = await _httpClient.GetStreamAsync(GetImgURL1 + getMessageID + GetImgURL2);

                Console.WriteLine("[GetImageAsync] END(OK) ");
                return GetImage;
            }
            catch (Exception ex)
            {

                Console.Error.WriteLine("[GetImageAsync] ERR: " + ex.Message.ToString());
                Console.Error.WriteLine("[GetImageAsync] END(Err) ");

                return null;

            }
        }

        public JObject GetResponse(string apiUrl, string jsonParameter)
        {
            JObject response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/json;";
                // カスタムヘッダーが必要の場合(認証トークンとか)
                request.Headers.Add("custom-api-param", "value");

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonParameter);
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();

                // HttpStatusCodeの判断が必要なら
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("API呼び出しに失敗しました。");
                }

                /*using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = JObject.Parse(streamReader.ReadToEnd());
                }
                */
                //                response["status"]:"success"
            }
            catch (WebException wex)
            {
                // 200以外の場合
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            response = JObject.Parse(reader.ReadToEnd());
                        }
                    }
                }
            }

            return response;
        }



        ///-------------------------------<summary>
        /// LINETXT送信(replyToken)</summary> 
        /// <param name="getreplyToken">replyToken</param>
        /// <param name="SendTxt">メッセージ</param>
        /// <returns>Exception</returns>
        public async Task<Exception> sendTextMSGAsync(string getreplyToken, string SendTxt)
        {
            try
            {
                var response = new HttpResponseMessage();
                //返信JSON作成
                var sendJSONobj = new LINEtextMSG()
                {
                    replyToken = getreplyToken,
                    messages = new List<Message>()
                        {
                            new Message()
                            {
                                type = "text",
                                text =SendTxt
                            }
                        }
                };
                //MSG送信処理(LinMsgTextMSG000)
                response = await _httpClient.PostAsJsonAsync(mdlLineMsg.stURL_LinMsgTextMSG000, sendJSONobj);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        ///-------------------------------<summary>
        /// LINETXT送信(To)</summary> 
        /// <param name="userLineId">ラインID</param>
        /// <param name="SendTxt">メッセージ</param>
        /// <returns>Exception</returns>
        public async Task<Exception> sendTextMSGAsyncTo(string userLineId, string SendTxt)
        {
            try
            {
                var response = new HttpResponseMessage();
                //返信JSON作成
                var sendJSONobj = new LINEtextMSG()
                {
                    to = userLineId,
                    messages = new List<Message>()
                        {
                            new Message()
                            {
                                type = "text",
                                text =SendTxt
                            }
                        }
                };
                //MSG送信処理(LinMsgTextMSG000)
                response = await _httpClient.PostAsJsonAsync(mdlLineMsg.stURL_LinMsgTextMSG000, sendJSONobj);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        /*------------------------------------*/
        /* MSGとしてのTXT送信
        /*------------------------------------*/
        public async Task<Exception> sendLocationMSGAsyncTo(string userLineId, string stitle, string saddress, Double dlatitude, Double dlongitude)
        {
            try
            {
                var response = new HttpResponseMessage();
                //返信JSON作成
                var sendJSONobj = new LINElocationMSG()
                {
                    to = userLineId,
                    messages = new List<Location>()
                                            {
                                                new Location()
                                                {
                                                    type = "location",
                                                    title= stitle,
                                                    address= saddress,
                                                    latitude= dlatitude,
                                                    longitude= dlongitude
                                                }
                                            },
                };
                //MSG送信処理(LinMsgLocMSG000)
                response = await _httpClient.PostAsJsonAsync<LINElocationMSG>(mdlLineMsg.stURL_LinMsgLocMSG000, sendJSONobj);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /*------------------------------------*/
        /* MSGとしてのTXT送信
        /*------------------------------------*/
        public async Task<Exception> sendQReplyMSGAsyncTo(string userLineId="", string replyToken = "", string text = "")
        {
            try
            {
                var response = new HttpResponseMessage();
                //返信JSON作成
                var sendJSONobj = new mdlLineMsg.LinMsgQReplyMSG000()
                {
                        replyToken = replyToken,
                        messages = new List<mdlLineMsg.QRMessage>()
                        {
                            new mdlLineMsg.QRMessage()
                            {
                                type="text",
                                text = text,
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
                };


                //MSG送信処理(LinMsgLocMSG000)
                response = await _httpClient.PostAsJsonAsync<mdlLineMsg.LinMsgQReplyMSG000>(mdlLineMsg.stURL_LinMsgLocMSG000, sendJSONobj);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }



    }
}
