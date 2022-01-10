using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ThingsLine.Modules;

namespace imageAPI
{
    public  class imageRegistration
    {
        public class DD
        {
            public DateTime dt { get; set; }
            public string imsi { get; set; }
            public string d_lat { get; set; }
            public string d_lon { get; set; }
        }

        [FunctionName("imageRegistration")]
        public async System.Threading.Tasks.Task RunAsync([BlobTrigger("imagetmp/{name}", Connection = "")]Stream myBlob, string name, ILogger log)
        {
            Console.WriteLine("[imageRegistration] Start------------------------------------");

            try {
                modSQLServer mdlSQL = new modSQLServer();
                StringBuilder sSQL = new StringBuilder();

                Console.WriteLine($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

                //-------------------------------------------------
                //DB用情報を取得
                string userID = "";
                string groupID = "";
                string dt = "";
                Console.WriteLine("[imageRegistration] name.Length  : " + name.Length);
                if (name.Length == 57)
                {
                    //ユーザーのみ
                    userID = name.Substring(0, 36);
                    Console.WriteLine("[imageRegistration] userID : " + userID);

                    dt = name.Substring(36, 17);
                    Console.WriteLine("[imageRegistration] dt : " + dt);
                }
                else if (name.Length == 90)
                {
                    //グループ
                    userID = name.Substring(0, 36);
                    Console.WriteLine("[imageRegistration] userID : " + userID);

                    groupID = name.Substring(36, 33);
                    Console.WriteLine("[imageRegistration] groupID : " + groupID);

                    dt = name.Substring(69, 17);
                    Console.WriteLine("[imageRegistration] dt : " + dt);
                }
                else {
                    throw new Exception("ERR nomelength:" + name.Length);
                }
                string dty = dt.Substring(0, 4);
                string dtm = dt.Substring(4, 2);
                string dtd = dt.Substring(6, 2);
                string dth = dt.Substring(8, 2);
                string dtm2 = dt.Substring(10, 2);
                string dts = dt.Substring(12, 2);
                string dtf = dt.Substring(14, 3);

                var fileID = name.ToString();

                string uploadDt = dty + "/" + dtm + "/" + dtd + " " + dth + ":" + dtm2 + ":" + dts + ":" + dtf;

                Console.WriteLine("[imageRegistration] uploadDt : " + uploadDt);

                //-------------------------------------------------
                //  基本情報
                sSQL.Clear();
                sSQL.Append("SELECT TOP(1)"
                                + "dt"
                                + " , imsi"
                                + " , str(d_lat,10,10) as d_lat"
                                + " , str(d_lon,10,10) as d_lon"
                                + " FROM [dbo].[DD_Soracom000]"
                                + " where dt <= dateadd(hour, 9, '" + uploadDt + "')"
                                + " and d_lat IS NOT NULL"
                                + " and d_lon IS NOT NULL"
                                + " and imsi = (SELECT UD.imsi FROM [dbo].[U_Device] as UD  join [dbo].[U_BIKE] as UB  on ( UD.bikeID = UB.bikeID )"
                                        + " where UD.userID = '" + userID + "' and UB.sortNO = 1)"
                                + " order by dt desc");
                Console.WriteLine("[imageRegistration] sql : " + sSQL.ToString());

                List<DD> retDD_Soracom000 = mdlSQL.GetSQL<DD>(sSQL);

                DateTime    dt2 = retDD_Soracom000[0].dt;
                var lat = retDD_Soracom000[0].d_lat;
                var lon = retDD_Soracom000[0].d_lon;
                var dataType = "1";
                var releaseLV = "1";

                //-------------------------------------------------
                //ファイルを移動

                string fromFolder = "imagetmp";

                string toFolder = "u" + userID.ToLower();
                if (groupID != "") {
                    toFolder = "g" + groupID.ToLower();
                }

                Console.WriteLine("[imageRegistration] fromFolder : " + fromFolder);
                Console.WriteLine("[imageRegistration] toFolder : " + toFolder);
                Console.WriteLine("[imageRegistration] fileID : " + fileID);
                modStorage mStorage = new modStorage();

                await mStorage.CopyBlobAsync(fromFolder, toFolder, fileID);



                //-------------------------------------------------
                //縮小ファイル作成＆移動
                /*
                                Bitmap bmp = new Bitmap(myBlob);

                                    int resizeWidth = 160;
                                    int resizeHeight = (int)(bmp.Height * ((double)resizeWidth / (double)bmp.Width));

                                    Bitmap resizeBmp = new Bitmap(resizeWidth, resizeHeight);
                                    Graphics g = Graphics.FromImage(resizeBmp);
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                    g.DrawImage(bmp, 0, 0, resizeWidth, resizeHeight);
                                    g.Dispose();
                */




                //-------------------------------------------------
                //DB登録
                sSQL.Clear();
                sSQL.Append("insert into  [dbo].[I_Imagelatlon](");
                sSQL.Append(""
                    + "  [userID]"
                    + " ,[groupID]"
                    + " ,[Folder]"
                    + " ,[fileID]"
                    + " ,[uploadDt]"
                    + " ,[dt]"
                    + " ,[lat]"
                    + " ,[lon]"
                    + " ,[dataType]"
                    + " ,[releaseLV]"
                    + " ,[stopFLG]"
                    + ")"
                    );
                sSQL.Append("VALUES  ( "
                    + " '" + userID + "'"
                    + ",'" + groupID + "'"
                    + ",'" + toFolder + "'"
                    + ", '" + fileID + "'"
                    + ", dateadd(hour,9,'" + uploadDt + "')"
                    + ", '" + dt2 + "'"
                    + ", " + lat + ""
                    + ", " + lon + ""
                    + ", " + dataType + ""
                    + ", " + releaseLV + ""
                    + ",0"
                    + ")"
                    );
                Console.WriteLine("[imageRegistration] sql : " + sSQL.ToString());

                Exception retB =  mdlSQL.setSQL(sSQL.ToString());
                Console.WriteLine("[imageRegistration] END------------------------------------ ");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[imageRegistration] " + ex.ToString());
                Console.Error.WriteLine("[imageRegistration] END(ERR)------------------------------------ ");
            }



        }

    }
}
