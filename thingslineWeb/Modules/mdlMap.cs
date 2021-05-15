using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using thingslineWeb.Models;
using Azure.Storage.Blobs;

using module;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace thingslineWeb.Modules
{
    public class mdlMap
    {

        private static readonly string ConnectString = ConfigurationManager.AppSettings.Get("Storage_ConnectString");
        private static readonly string PhotoContainerName = ConfigurationManager.AppSettings.Get("Storage_Container_Photo");



        public async System.Threading.Tasks.Task<string> UploadImgAsync(HttpPostedFileWrapper fileHttpPostedFileWrapper,string userID)
        {

            CloudStorageAccount account = CloudStorageAccount.Parse(ConnectString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("imagetmp");
            container.CreateIfNotExistsAsync().Wait();
            /*
                        var blobName = Guid.NewGuid().ToString("D") + System.IO.Path.GetExtension(fileHttpPostedFileWrapper.FileName);
                        CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
                        blob.Properties.ContentType = fileHttpPostedFileWrapper.ContentType;
                        await blob.UploadFromStreamAsync(fileHttpPostedFileWrapper.InputStream);
            var blobName = userID + DateTime.Now.ToString("yyyyMMddHHmmss");
            */

            var blobName = userID + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".JPG";
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            blob.Properties.ContentType = fileHttpPostedFileWrapper.ContentType;
            await blob.UploadFromStreamAsync(fileHttpPostedFileWrapper.InputStream);
            container.GetBlockBlobReference(blobName);

            return fileHttpPostedFileWrapper.FileName;

        }




        public MapViweModel getMapData(MapViweModel getMapViweModel)
        {
            
            module.mdlSQLServer mComm = new module.mdlSQLServer();



            StringBuilder strSql = new StringBuilder();
            MapViweModel retMapViweModel = new MapViweModel();
            getMapViweModel.MapData = "";

            /*------------------------------*/
            /*　Keyデータ*/
            //retMapViweModel.SearchCndDate
            string schkDateST = DateTime.Now.ToString("yyyy-MM-dd");
            string schkDateED = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //string schkDate = "2021/03/07 00:00:00";

            if (getMapViweModel != null)
            {
                schkDateST = getMapViweModel.SearchCndDate.ToString("yyyy/MM/dd");
                schkDateED = getMapViweModel.SearchCndDate.AddDays(1).ToString("yyyy/MM/dd");
            }

            //retMapViweModel.IMSI
            string schkDateIMSI = "440103224700366";
            if (getMapViweModel.IMSI != null)
            {
                schkDateIMSI = getMapViweModel.IMSI.ToString();
            }
            if (getMapViweModel.IMSI == null && getMapViweModel.afIMSI != null)
            {
                schkDateIMSI = getMapViweModel.afIMSI.ToString();
            }

            retMapViweModel.IMSI = schkDateIMSI;
            retMapViweModel.UserID = getMapViweModel.UserID;
            retMapViweModel.GroupID = getMapViweModel.GroupID;

            /*------------------------------*/
            /*　MAP用データリスト(ホーム)*/
            /*------------------------------*/
            //retMapViweModel.Maplat
            //retMapViweModel.Maplon
            strSql.Clear();
            strSql.Append("SELECT  top(1) dt,imsi,imei,operatorId,d_lat,d_lon,d_bat,d_rs,d_temp,d_humi,d_a_x,d_a_y,d_a_z,d_type "
                     );
            strSql.Append(" FROM [dbo].[DD_Soracom000]");
            strSql.Append(" Where d_lat IS NOT NULL ");
            strSql.Append(" and ");
            strSql.Append("  imsi = '" + schkDateIMSI + "' ");
            strSql.Append(" order by dt desc");

            List<LastDD> retLastDD = mComm.GetSQL<LastDD>(strSql);
            if (retLastDD.Count > 0)
            {
                retMapViweModel.LastDD = retLastDD;
            }
            else
            {
                retMapViweModel.LastDD = new List<LastDD>();
            }

            /*------------------------------*/
            /*　MAP用データリスト(ポイント)*/
            /*------------------------------*/
            //retMapViweModel.MapData
/*            strSql.Clear();
            strSql.Append("SELECT  "
                    + " CONVERT(int,ROW_NUMBER() OVER(ORDER BY dt ASC)) Id"
                    + ",CONVERT(VARCHAR,dt,120) as PlaceName"
                    + ", CONVERT(varchar,d_temp) as OpeningHours"
                    + ",str(d_lon,10,10) as GeoLong"
                    + ",str(d_lat,10,10) as GeoLat"
                     );
            strSql.Append(" FROM [dbo].[DD_Soracom000]");
            strSql.Append(" Where d_lat IS NOT NULL ");
            strSql.Append(" and ");
            strSql.Append(" dt >= '" + schkDateST + "' and dt < '" + schkDateED + "' ");
            strSql.Append(" and imsi = '" + schkDateIMSI + "' ");
            strSql.Append(" order by dt desc");

            List<mdlDevicesData.GMPoint> retDD_Soracom000 = mComm.GetSQL<mdlDevicesData.GMPoint>(strSql);

            //データ変換
            var jsonString = JsonConvert.SerializeObject(retDD_Soracom000, Formatting.None);
            string tmp = jsonString.Replace(@"\", "-");
            retMapViweModel.MapData = new string(tmp.Where(c => !char.IsControl(c)).ToArray());
*/
            /*------------------------------*/
            /*　MAP用データリスト(画像)*/
            /*------------------------------*/
/*            //retMapViweModel.MapData
            strSql.Clear();
            strSql.Append("SELECT  "
                    + " CONVERT(int,ROW_NUMBER() OVER(ORDER BY DD.dt ASC)) Id"
                    + ",CONVERT(VARCHAR,DD.dt,120) as PlaceName"
                    + ", CONVERT(varchar,DD.d_temp) as OpeningHours"
                    + ",str(DD.d_lon,10,10) as GeoLong"
                    + ",str(DD.d_lat,10,10) as GeoLat"
                     );

            strSql.Append(" FROM dbo.DD_Soracom000 as DD right join dbo.I_Imagelatlon as IMG on (IMG.dt = DD.dt)");
            strSql.Append(" Where DD.d_lat IS NOT NULL ");
            strSql.Append(" and ");
            strSql.Append(" DD.dt >= '" + schkDateST + "' and DD.dt < '" + schkDateED + "' ");
            strSql.Append(" and DD.imsi = '" + schkDateIMSI + "' ");

            strSql.Append(" order by DD.dt");
            List<mdlDevicesData.GMPoint> retimgDD_Soracom000 = mComm.GetSQL<mdlDevicesData.GMPoint>(strSql);

            //データ変換
            jsonString = JsonConvert.SerializeObject(retimgDD_Soracom000, Formatting.None);
            tmp = jsonString.Replace(@"\", "-");
            retMapViweModel.imgMapData = new string(tmp.Where(c => !char.IsControl(c)).ToArray());
            */
            /*------------------------------*/
            /*　サイドバー用データリスト(ポイント)*/
            /*------------------------------*/
            //位置情報
            //retMapViweModel.DD

            strSql.Clear();
            strSql.Append("SELECT  DD.dt,DD.imsi,DD.imei,DD.operatorId,DD.d_lat,DD.d_lon,DD.d_bat,DD.d_rs,DD.d_temp,DD.d_humi,DD.d_a_x,DD.d_a_y,DD.d_a_z,DD.d_type ");

            strSql.Append(" ,(IMG.Folder + '/' + IMG.fileID) as I_fileID,IMG.Folder as I_folderID,IMG.uploadDt as I_uploadDt,IMG.lat as I_lat ,IMG.lon as I_lon ,IMG.dataType as I_dataType,IMG.releaseLV as I_releaseLV");

            strSql.Append(",(MIN(DD.[d_lat]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS bf_lat");

            strSql.Append(", (MIN(DD.[d_lon]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS bf_lon");


            strSql.Append(", datediff(second, MIN(DD.[dt]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING),DD.[dt]) AS diff_dt");

            strSql.Append(",(DD.[d_lat] - MIN(DD.[d_lat]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS diff_lat");

            strSql.Append(", (DD.[d_lon] -MIN(DD.[d_lon]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS diff_lon");
            strSql.Append(", ROW_NUMBER() OVER(ORDER BY DD.dt ASC) Rownum");

            strSql.Append(" FROM dbo.DD_Soracom000 as DD left join dbo.I_Imagelatlon as IMG on (IMG.dt = DD.dt)");
            strSql.Append(" Where DD.d_lat IS NOT NULL ");
            strSql.Append(" and ");
            strSql.Append(" DD.dt >= '" + schkDateST + "' and DD.dt < '" + schkDateED + "' ");
            strSql.Append(" and DD.imsi = '" + schkDateIMSI + "' ");

            strSql.Append(" order by DD.dt");

            List<DD> retDD = mComm.GetSQL<DD>(strSql);

            if (retDD.Count > 0)
            {
                retMapViweModel.DD = retDD;
            }
            else
            {
                retMapViweModel.DD = new List<DD>();
            }
            //データ変換
           var jsonString = JsonConvert.SerializeObject(retDD, Formatting.None);
            string tmp = jsonString.Replace(@"\", "-");
            retMapViweModel.MapData = new string(tmp.Where(c => !char.IsControl(c)).ToArray());

            /*------------------------------*/
            /*　サイドバー用データリスト(イメージ)*/
            /*------------------------------*/
            //画像情報
            //retMapViweModel.imgDD

            strSql.Clear();
            strSql.Append("SELECT  DD.dt,DD.imsi,DD.imei,DD.operatorId,DD.d_lat,DD.d_lon,DD.d_bat,DD.d_rs,DD.d_temp,DD.d_humi,DD.d_a_x,DD.d_a_y,DD.d_a_z,DD.d_type ");

            strSql.Append(" , (IMG.Folder + '/' + IMG.fileID) as I_fileID, IMG.Folder  as I_folderID,IMG.uploadDt as I_uploadDt,IMG.lat as I_lat ,IMG.lon as I_lon ,IMG.dataType as I_dataType,IMG.releaseLV as I_releaseLV");

            strSql.Append(",(MIN(DD.[d_lat]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS bf_lat");

            strSql.Append(", (MIN(DD.[d_lon]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS bf_lon");


            strSql.Append(", datediff(second, MIN(DD.[dt]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING),DD.[dt]) AS diff_dt");

            strSql.Append(",(DD.[d_lat] - MIN(DD.[d_lat]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS diff_lat");

            strSql.Append(", (DD.[d_lon] -MIN(DD.[d_lon]) OVER(ORDER BY DD.[dt] ASC");
            strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS diff_lon");
            strSql.Append(", ROW_NUMBER() OVER(ORDER BY DD.dt ASC) Rownum");

            strSql.Append(" FROM dbo.DD_Soracom000 as DD right join dbo.I_Imagelatlon as IMG on (IMG.dt = DD.dt)");
            strSql.Append(" Where DD.d_lat IS NOT NULL ");
            strSql.Append(" and ");
            strSql.Append(" DD.dt >= '" + schkDateST + "' and DD.dt < '" + schkDateED + "' ");
            strSql.Append(" and DD.imsi = '" + schkDateIMSI + "' ");

            strSql.Append(" order by DD.dt");

            List<DD> retimgDD = mComm.GetSQL<DD>(strSql);

            if (retDD.Count > 0)
            {
                retMapViweModel.imgDD = retimgDD;
            }
            else
            {
                retMapViweModel.imgDD = new List<DD>();
            }






            //リターン
            return retMapViweModel;


        }



    }
}