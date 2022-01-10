using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ThingsLineAPIs.Models
{

    public class mdlLineMsg
    {
        // LINE API
        public const string _secret = "41553b983f9ad34c11825304908de02d";
        public const string _token = "PAG0ZG/1HqwEw2CF9LL6l+UYBHPQdWR584bM+V0Dcw4GVdlCn+ScKGBK05eGjCQqtPzG4NSDr3i0Me4IKFZtkxh7/PcFj4qJC1iFO3QytzX7ZrdiONlN93bICeLD3/5pSOWrsy5fToIPj5L8lCilngdB04t89/1O/w1cDnyilFU=";
        public static string PushURL = "https://api.line.me/v2/bot/message/push";
        public static string ReplyURL = "https://api.line.me/v2/bot/message/reply";

        public static string CheckURL = "https://thingslineapis.azurewebsites.net/api/LinMsgCHK000?code=MyFwzhBQ4SEdMIZdicuUjXb1QeTHY0d7pXESghsqmBGH3aPsz42HNA==";
        public const string stURL_LinMsgLocMSG000 = "https://thingslineapis.azurewebsites.net/api/LinMsgLocMSG000?code=SBPFGND3Gvf8kOUYnS3azsf8clb3jVN3g0YIC7Ki5LKCwerAqilS9g==";
        public const string stURL_LinMsgQReplyMSG000 = "https://thingslineapis.azurewebsites.net/api/LinMsgQReplyMSG000?code=eiJI3/wAaq1IIa7z6XNYrHzhJi8qSHVVmdLb2M8TQMfCuSuZbYQmZA==";
        public const string stURL_LinImg000 = "https://thingslineapis.azurewebsites.net/api/LinImg000?code=5tfXRc4j8rFXqIrkr6RfdAnxruho5anVeMMnVwRl/p85GE5d4rFxXQ==";
        public const string stURL_LinMsgTextMSG000 = "https://thingslineapis.azurewebsites.net/api/LinMsgTextMSG000?code=A7A/23M//B7Rd/93oLgkuA3sOoHQFEMomZ6syanJsI7FV8OISW32aw==";

        // Webhook受け取り用
        public class LineWebhookMessageObject
        {
            public string destination { get; set; }
            public List<Event> events { get; set; }
        }
        public class baseData
        {
            //   UDevice.userID, UDevice.imsi,UDevice.imei"
            public string userID { get; set; }
            public string imsi { get; set; }
            public string bikeName { get; set; }

            public baseData(){}
        }




        public class Event
        {
            public string replyToken { get; set; }
            public string type { get; set; }
            public object timestamp { get; set; }
            public Source source { get; set; }
            public Message message { get; set; }
            public Joined joined { get; set; }
            
            public string mode { get; set; }
        }
        public class Source
        {
            public string type { get; set; }
            public string userId { get; set; }
            public string groupId { get; set; }
            public string roomId { get; set; }
        }

        public class Message
        {
            public string id { get; set; }
            public string type { get; set; }
            public string text { get; set; }

            public string address { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public contentProvider contentProvider { get; set; }
        }

        public class contentProvider
        {
            public string type { get; set; }
            public string originalContentUrl { get; set; }
            public string previewImageUrl { get; set; }
        }

        public class Joined
        {
            public Members members { get; set; }
        }
        public class Members
        {
            public string type { get; set; }
            public string userId { get; set; }
        }

        // 送信用（TXT
        public class LineTextReplyObject
        {
            public string replyToken { get; set; }
            public List<Message> messages { get; set; }
            public bool notificationDisabled { get; set; }
        }

        // 送信用（TXT
        public class LineTextPushObject
        {
            public string to { get; set; }
            public List<Message> messages { get; set; }
            public bool notificationDisabled { get; set; }
        }






        public class LinImg000
        {
            public string to { get; set; }
            public string replyToken { get; set; }
            public Event Event { get; set; }
        }



        public class LinMsgQReplyMSG000
        {
            public string to { get; set; }
            public string replyToken { get; set; }
            public List<QRMessage> messages { get; set; }
        }



        //クイックリプライメッセージ
        public class QRMessage
        {
            public string id { get; set; }
            public string type { get; set; }
            public string text { get; set; }
            //クイックリプライ
            public QRQuickReply quickReply { get; set; }
        }

        //クイックリプライ
        public class QRQuickReply
        {
            //クイックリプライ　アイテム
            public List<QRItem> items { get; set; }
        }

        //クイックリプライ　アイテム
        public class QRItem
        {
            public string type { get; set; }
            public string imageUrl { get; set; }
            //クイックリプライ　アクション
            public QRAction action { get; set; }
        }
        //クイックリプライ　アクション
        public class QRAction
        {
            public string type { get; set; }
            public string label { get; set; }
            public string text { get; set; }
            public string data { get; set; }
            public string displayText { get; set; }
            public string mode { get; set; }
            public string initial { get; set; }
            public string max { get; set; }
            public string min { get; set; }
        }


        internal Task SendTextReplyAsync(string replyToken, string text)
        {
            throw new NotImplementedException();
        }





    }


}
