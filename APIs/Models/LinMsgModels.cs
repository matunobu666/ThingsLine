using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace LINEAPI
{

    public class LinMsgModels
    {

        // Webhook受け取り用
        public class LineWebhookMessageObject
        {
            public string destination { get; set; }
            public List<Event> events { get; set; }
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



        public const string _secret = "41553b983f9ad34c11825304908de02d";



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

        public class LinMsgTextMSG000
        {
            public string to { get; set; }
            public string replyToken { get; set; }
            public List<Message> messages { get; set; }
        }



        public class LinMsgLocMSG000
        {
            public string to { get; set; }
            public string replyToken { get; set; }
            public List<Location> messages { get; set; }
            public Source sources { get; set; }

            public static implicit operator Stream(LinMsgLocMSG000 v)
            {
                throw new NotImplementedException();
            }
        }


        public class Location
        {
            public string type { get; set; }
            public string title { get; set; }
            public string address { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
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

        public class QRMessage
        {
            public string id { get; set; }
            public string type { get; set; }
            public string text { get; set; }
            public QRQuickReply quickReply { get; set; }
        }

        public class QRQuickReply
        {
            public List<QRItem> items { get; set; }
        }

        public class QRItem
        {
            public string type { get; set; }
            public string imageUrl { get; set; }
            public QRAction action { get; set; }
        }


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


        public class richmenulink
        {
            public string richMenuId { get; set; }
            public string userIds { get; set; }
        }








        internal Task SendTextReplyAsync(string replyToken, string text)
        {
            throw new NotImplementedException();
        }





    }


}
