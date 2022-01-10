using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ThingsLine.Models
{

    public class mdlLINE
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

        //グループ情報
        public class l_GroupInfo
        {
            //   UDevice.userID, UDevice.imsi,UDevice.imei"
            public string groupId { get; set; }
            public string groupName { get; set; }
            public string type { get; set; }
            public DateTime timestamp { get; set; }
            public string Status { get; set; }
        }

       //グループユーザー
       public class l_Group
        {
            //   UDevice.userID, UDevice.imsi,UDevice.imei"
            public string groupId { get; set; }
            public string userId { get; set; }
            public string type { get; set; }
            public DateTime timestamp { get; set; }
            public string Status { get; set; }
        }

        //テキストメッセージ
        public class LINEtextMSG
        {
            public string to { get; set; }
            public string replyToken { get; set; }
            public List<Message> messages { get; set; }
        }
        //位置情報メッセージ
        public class LINElocationMSG
        {
            public string to { get; set; }
            public string replyToken { get; set; }
            public List<Location> messages { get; set; }
            public Source sources { get; set; }
        }

        public class Location
        {
            public string type { get; set; }
            public string title { get; set; }
            public string address { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
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
    }
}
