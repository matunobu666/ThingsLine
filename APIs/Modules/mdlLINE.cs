using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using System.Net;
using LINEAPI;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;

namespace module
{
    public class mdlLINE
    {
        //        private static readonly string _CAtoken = ConfigurationManager.AppSettings.Get("_CAtoken");
        //        private static readonly string GetImgURL1 = ConfigurationManager.AppSettings.Get("GetImgURL1").ToString();
        //        private static readonly string GetImgURL2 = ConfigurationManager.AppSettings.Get("GetImgURL2").ToString();

        private static readonly string GetImgURL1 = "https://api-data.line.me/v2/bot/message/";
        private static readonly string GetImgURL2 = "/content";
        private static readonly string _CAtoken = "PAG0ZG/1HqwEw2CF9LL6l+UYBHPQdWR584bM+V0Dcw4GVdlCn+ScKGBK05eGjCQqtPzG4NSDr3i0Me4IKFZtkxh7/PcFj4qJC1iFO3QytzX7ZrdiONlN93bICeLD3/5pSOWrsy5fToIPj5L8lCilngdB04t89/1O/w1cDnyilFU=";
        private static readonly string _secret = "41553b983f9ad34c11825304908de02d";

        private static readonly string ReplyURL = "https://api.line.me/v2/bot/message/reply";

        //-------------------------------
        //シグネイチャチェック
        //-------------------------------        
        public Boolean SingatureCHK(string xlinesignature, string requestBody)
        {
            try
            {
                Console.WriteLine("[SingatureCHK] start ");
                LinMsgModels LineAPIObj = new LinMsgModels();

                // しぐねいちゃチェック
                if (IsSingatureCHK(xlinesignature, requestBody, _secret))
                {
                    Console.WriteLine("[SingatureCHK] OK ");
                    Console.WriteLine("[SingatureCHK] END ");
                    return true;

                }
                Console.WriteLine("[SingatureCHK] END ");
                Console.WriteLine("[SingatureCHK] NG");
                return false;

            }
            catch(Exception ex) {
                Console.Error.WriteLine("[SingatureCHK] ERR: " + ex.Message.ToString());
                Console.WriteLine("[SingatureCHK] END ");
                return false;

            }
        }


        //-------------------------------
        //LINE に送信
        //-------------------------------        
        public Task<HttpResponseMessage> SendMessageAsync(HttpClient _httpClient, object req)
        {
            Console.WriteLine("[SendMessageAsync] start ");
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _CAtoken);
                Task<HttpResponseMessage> response =  _httpClient.PostAsJsonAsync(ReplyURL , req);

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
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _CAtoken);

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

        /// <summary>
        /// Return if signature matches
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsSingatureCHK(string signature, string text, string key)
        {

            var textBytes = Encoding.UTF8.GetBytes(text);
            var keyBytes = Encoding.UTF8.GetBytes(key);

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                var hash = hmac.ComputeHash(textBytes, 0, textBytes.Length);
                var hash64 = Convert.ToBase64String(hash);

                return signature == hash64;
            }
        }


    }
}
