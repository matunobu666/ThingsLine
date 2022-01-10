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
    public class modLINE
    {
        //-------------------------------
        //シグネイチャチェック
        //-------------------------------        
        public Boolean SingatureCHK(string xlinesignature, string requestBody)
        {
            try
            {
                Console.WriteLine("[SingatureCHK] start ");

                // しぐねいちゃチェック
                if (IsSingatureCHK(xlinesignature, requestBody, mdlLINE._secret))
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
