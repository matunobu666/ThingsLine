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

namespace ThingsLine.Modules
{
    public class modSYS
    {
        //-------------------------------------------------
        //ThingsLine.Modules
        modSSystemMessage mSSystemMessage = new modSSystemMessage();

        ///--------------------------------------------------------------<summary>
        ///DBログへの出力(ERR)
        ///</summary>-------------------------------------------------------------- 
        /// <param name="s_namespace">s_namespace</param>
        /// <param name="s_member">s_member</param>
        /// <param name="Exc">Exc</param>
        /// <param name="comment01">comment01</param>
        /// <param name="comment02">comment02</param>
        /// <param name="comment03">comment03</param>
        /// <returns>なし</returns>
        public void Log2DBERR(string s_namespace , string s_member = "", string Exc = "", string comment01 = "", string comment02 = "", string comment03 = "")
        {
            try
            {
                Console.WriteLine("[Log2DBERR] start ");
                S_SystemMessage setData = new S_SystemMessage();
                setData.type = "ERR";
                setData.s_namespace = s_namespace;
                setData.s_member = s_member;
                setData.Exc = Exc;
                setData.comment01 = comment01;
                setData.comment02 = comment02;
                setData.comment03 = comment03;

                mSSystemMessage.SetSErrorMessage(setData);
                Console.WriteLine("[Log2DBERR] END ");

            }
            catch (Exception ex) {
                Console.Error.WriteLine("[Log2DBERR] ERR: " + ex.Message.ToString());
                Console.WriteLine("[Log2DBERR] END ");

            }
        }

    }
}
