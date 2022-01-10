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
using ThingsLine.Models;
using static ThingsLine.Models.mdlLINE;

namespace ThingsLine.Modules
{
    public class modLINEGroup
    {
        readonly modLGroupInfo mLGroupInfo = new modLGroupInfo();
        readonly modLGroup mLGroup = new modLGroup();

        /// <summary>
        /// l_GroupInfo　テーブルへの追加処理
        /// </summary>
        /// <param name="GroupID">GroupID</param>
        /// <returns>なし</returns>  
        public Exception GroupInfoCHKaINS(string tmpGroupID)
        {
            //既存判定（l_GroupInfo）
            List<l_GroupInfo> retGroupInfo = mLGroupInfo.getLGroupInfo(tmpGroupID);
            Console.WriteLine("[MessageEvent] retGroupInfo.Count : " + retGroupInfo.Count.ToString());

            if (retGroupInfo.Count == 0)
            {
                //追加処理
                //追加処理
                l_GroupInfo setGroupInfo = null;
                setGroupInfo.groupId = tmpGroupID;
                setGroupInfo.groupName = "";
                setGroupInfo.type = "1";
                setGroupInfo.Status = "0";

                Exception retEX = mLGroupInfo.SetlGroupInfo(setGroupInfo);
            }
            return null;
        }


        /// <summary>
        /// l_Group　テーブルへの追加処理
        /// </summary>
        /// <param name="GroupID">GroupID</param>
        /// <param name="userId">userId</param>
        /// <returns>なし</returns>
        public Exception GroupCHKaINS(string GroupID, string userId)
        {


            //既存判定（l_Group）
            List<l_Group> retGroup = mLGroup.getLGroup(GroupID, userId);
            Console.WriteLine("[MessageEvent] retGroup.Count : " + retGroup.Count.ToString());

            if (retGroup.Count == 0)
            {
                l_Group setGroup = null;
                setGroup.groupId =GroupID;
                setGroup.userId = userId;
                setGroup.type = "1";
                setGroup.Status = "0";

                Exception retEX = mLGroup.SetlGroup(setGroup);

            }
            return null;
        }


    }
}
