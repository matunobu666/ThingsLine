using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using thingslineWeb.Models;


using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using ThingsLine.Modules;

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
            
            modSQLServer mComm = new modSQLServer();

            StringBuilder strSql = new StringBuilder();
            MapViweModel retMapViweModel = new MapViweModel();
            getMapViweModel.MapData = "";


            string schkDateIMSI = "440103224700366";
            string schkDatedeviceType = "SKG001";
            string schkDatebikeName = "TigerExplorerXCA";



            /*------------------------------*/
            /*　Keyデータ*/
            //retMapViweModel.SearchCndDate
            string schkDateST = DateTime.Now.ToString("yyyy-MM-dd");
            string schkDateED = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //string schkDate = "2021/03/07 00:00:00";
                if (getMapViweModel.SearchCndDate == getMapViweModel.SearchCndDateED)
                {
                    schkDateST = getMapViweModel.SearchCndDate.ToString("yyyy/MM/dd");
                    schkDateED = getMapViweModel.SearchCndDate.AddDays(1).ToString("yyyy/MM/dd");
                }
                if (getMapViweModel.SearchCndDate < getMapViweModel.SearchCndDateED)
                {
                    schkDateST = getMapViweModel.SearchCndDate.ToString("yyyy/MM/dd");
                    schkDateED = getMapViweModel.SearchCndDateED.AddDays(1).ToString("yyyy/MM/dd");
            }
                if (getMapViweModel.SearchCndDate > getMapViweModel.SearchCndDateED)
                {
                    schkDateST = getMapViweModel.SearchCndDate.ToString("yyyy/MM/dd");
 //               schkDateED = getMapViweModel.SearchCndDate.AddDays(1).ToString("yyyy/MM/dd");
                schkDateED = getMapViweModel.SearchCndDate.AddDays(1).ToString("yyyy/MM/dd");
            }



            //retMapViweModel.IMSI
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
            /*　ユーザーデータ*/
            /*------------------------------*/
            strSql.Clear();
            strSql.Append(" SELECT UD.[userID],UD.[imsi],UD.[deviceType],UB.bikeName");
            strSql.Append(" FROM[dbo].[U_Device] as UD JOIN[dbo].[U_BIKE] as UB");
            strSql.Append("  on UD.userID = UB.userID and UD.bikeID = UB.bikeID");
            strSql.Append(" Where UD.userID = '" + getMapViweModel.UserID + "'");
            strSql.Append(" order by ");
            strSql.Append(" CASE UD.imsi WHEN '" + schkDateIMSI + "' THEN 1 ELSE 2 END");
            strSql.Append(" , UB.sortNO ");

            List<userDD> retuserDD = mComm.GetSQL<userDD>(strSql);
            if (retuserDD.Count > 0)
            {
                retMapViweModel.userDD = retuserDD;
                schkDateIMSI = retuserDD[0].imsi;
                schkDatedeviceType = retuserDD[0].deviceType;
                schkDatebikeName = retuserDD[0].bikeName;
            }
            else
            {
                retMapViweModel.userDD = new List<userDD>();
                schkDateIMSI = "440103224700366";
                schkDatedeviceType = "SKG001";
                schkDatebikeName = "TigerExplorerXCA";
            }


            /*------------------------------*/
            /*　MAP用データリスト(ホーム)*/
            /*------------------------------*/
            //retMapViweModel.Maplat
            //retMapViweModel.Maplon



            strSql.Clear();
            if (schkDatedeviceType == "SKG001")
            {
                strSql.Append("SELECT  top(1) dt,imsi,imei,operatorId,d_lat,d_lon,d_bat,d_rs,d_temp,d_humi,d_a_x,d_a_y,d_a_z,d_type ");
                strSql.Append(" FROM [dbo].[DD_Soracom000]");
                strSql.Append(" Where d_lat IS NOT NULL ");
                strSql.Append(" and ");
                strSql.Append("  imsi = '" + schkDateIMSI + "' ");
                strSql.Append(" order by dt desc");
            }
            else if (schkDatedeviceType == "SKB001")
            {
                strSql.Append("SELECT  top(1) dt,imsi,imei,operatorId,lat as d_lat,lon as d_lon,bat as d_bat,0 as d_rs,0 as d_temp,0 as d_humi,0 as d_a_x,0 as d_a_y,0 as d_a_z,0 as d_type ");
                strSql.Append(" FROM [dbo].[DD_SoraKBeacon000]");
                strSql.Append(" Where lat IS NOT NULL ");
                strSql.Append(" and ");
                strSql.Append("  imsi = '" + schkDateIMSI + "'  and type=1");
                strSql.Append(" order by dt desc");
            }
            else
            {
                strSql.Append("SELECT  top(1) dt,imsi,imei,operatorId,d_lat,d_lon,d_bat,d_rs,d_temp,d_humi,d_a_x,d_a_y,d_a_z,d_type ");
                strSql.Append(" FROM [dbo].[DD_Soracom000]");
                strSql.Append(" Where d_lat IS NOT NULL ");
                strSql.Append(" and ");
                strSql.Append("  imsi = '" + schkDateIMSI + "' ");
                strSql.Append(" order by dt desc");
            }






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
            /*　データリスト(ポイント)*/
            /*------------------------------*/
            //位置情報
            //retMapViweModel.DD


            strSql.Clear();
            if (schkDatedeviceType == "SKG001")
            {
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

                strSql.Append(" FROM dbo.DD_Soracom000 as DD left join dbo.I_Imagelatlon as IMG on (IMG.dt = DD.dt and (IMG.userID = '" + getMapViweModel.UserID + "' or IMG.groupID = '" + getMapViweModel.GroupID + "'   ))");



                strSql.Append(" Where DD.d_lat IS NOT NULL ");
                strSql.Append(" and ");
                strSql.Append(" DD.dt >= '" + schkDateST + "' and DD.dt < '" + schkDateED + "' ");
                strSql.Append(" and DD.imsi = '" + schkDateIMSI + "' ");

                strSql.Append(" order by DD.dt");
            }
            else if (schkDatedeviceType == "SKB001")
            {
                strSql.Append("SELECT  DD.dt,DD.imsi,DD.imei,DD.operatorId,DD.lat as d_lat,DD.lon as d_lon,DD.bat as d_bat,0 as d_rs,0 as d_temp,0 as d_humi,0 as d_a_x,0 as d_a_y,0 as d_a_z,DD.type as d_type ");
                strSql.Append(" ,(IMG.Folder + '/' + IMG.fileID) as I_fileID,IMG.Folder as I_folderID,IMG.uploadDt as I_uploadDt,IMG.lat as I_lat ,IMG.lon as I_lon ,IMG.dataType as I_dataType,IMG.releaseLV as I_releaseLV");
                strSql.Append(",(MIN(DD.[lat]) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS bf_lat");
                strSql.Append(", (MIN(DD.[lon]) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS bf_lon");
                strSql.Append(", datediff(second, MIN(DD.[dt]) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING),DD.[dt]) AS diff_dt");
                strSql.Append(",(DD.[lat] - MIN(DD.[lat]) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS diff_lat");
                strSql.Append(", (DD.[lon] -MIN(DD.[lon]) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS diff_lon");
                strSql.Append(", ROW_NUMBER() OVER(ORDER BY DD.dt ASC) Rownum");

                strSql.Append(" FROM dbo.DD_SoraKBeacon000 as DD left join dbo.I_Imagelatlon as IMG on (IMG.dt = DD.dt and (IMG.userID = '" + getMapViweModel.UserID + "' or IMG.groupID = '" + getMapViweModel.GroupID + "'   ))");

                strSql.Append(" Where DD.lat IS NOT NULL ");
                strSql.Append(" and ");
                strSql.Append(" DD.dt >= '" + schkDateST + "' and DD.dt < '" + schkDateED + "'  and DD.type=1");
                strSql.Append(" and DD.imsi = '" + schkDateIMSI + "' ");

                strSql.Append(" order by DD.dt");
            }
            else
            {
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

                strSql.Append(" FROM dbo.DD_Soracom000 as DD left join dbo.I_Imagelatlon as IMG on (IMG.dt = DD.dt and (IMG.userID = '" + getMapViweModel.UserID + "' or IMG.groupID = '" + getMapViweModel.GroupID + "'   ))");

                strSql.Append(" Where DD.d_lat IS NOT NULL ");
                strSql.Append(" and ");
                strSql.Append(" DD.dt >= '" + schkDateST + "' and DD.dt < '" + schkDateED + "' ");
                strSql.Append(" and DD.imsi = '" + schkDateIMSI + "' ");

                strSql.Append(" order by DD.dt");
            }


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
            if (schkDatedeviceType == "SKG001")
            {
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
            }
            else if (schkDatedeviceType == "SKB001")
            {
                strSql.Append("SELECT  DD.dt,DD.imsi,DD.imei,DD.operatorId,DD.lat as d_lat,DD.lon as d_lon,DD.bat as d_bat,0 as d_rs,0 as d_temp,0 as d_humi,0 as d_a_x,0 as d_a_y,0 as d_a_z,0 as d_type ");
                strSql.Append(" , (IMG.Folder + '/' + IMG.fileID) as I_fileID, IMG.Folder  as I_folderID,IMG.uploadDt as I_uploadDt,IMG.lat as I_lat ,IMG.lon as I_lon ,IMG.dataType as I_dataType,IMG.releaseLV as I_releaseLV");
                strSql.Append(",(MIN(DD.lat) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS bf_lat");
                strSql.Append(", (MIN(DD.lon) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS bf_lon");
                strSql.Append(", datediff(second, MIN(DD.[dt]) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING),DD.[dt]) AS diff_dt");
                strSql.Append(",(DD.lat - MIN(DD.lat) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS diff_lat");
                strSql.Append(", (DD.lon -MIN(DD.lon) OVER(ORDER BY DD.[dt] ASC");
                strSql.Append(" ROWS BETWEEN 1 PRECEDING AND 1 PRECEDING)) AS diff_lon");
                strSql.Append(", ROW_NUMBER() OVER(ORDER BY DD.dt ASC) Rownum");

                strSql.Append(" FROM dbo.DD_SoraKBeacon000 as DD right join dbo.I_Imagelatlon as IMG on (IMG.dt = DD.dt)");
                strSql.Append(" Where DD.lat IS NOT NULL ");
                strSql.Append(" and ");
                strSql.Append(" DD.dt >= '" + schkDateST + "' and DD.dt < '" + schkDateED + "'  and DD.type=1");
                strSql.Append(" and DD.imsi = '" + schkDateIMSI + "' ");

                strSql.Append(" order by DD.dt");
            }
            else
            {
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
            }



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