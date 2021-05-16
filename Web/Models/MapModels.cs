using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace thingslineWeb.Models
{

    /*MAP用*/
    public class MapViweModel
    {

        /*Keyデータ*/
        [Display(Name = " 検索日 ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        public DateTime SearchCndDate { get; set; }
        public string UserID { get; set; }
        public string GroupID { get; set; }
        public string IMSI { get; set; }
        public string afIMSI { get; set; }
        public HttpPostedFileWrapper uploadFile { get; set; }
        
        /*MAP用データ*/
        /*履歴データ*/
        public string MapData { get; set; }
        /*イメージデータ*/
        public string imgMapData { get; set; }

        /*最終位置データ*/
        public string Maplat { get; set; }
        public string Maplon { get; set; }

        /*サイドバー用*/
        /*デバイスリスト*/
        public IEnumerable<UserDevice> UserDevice { get; set; }
        /*デバイスデータ*/
        public IEnumerable<DD> DD { get; set; }
        public IEnumerable<DD> imgDD { get; set; }
        public IEnumerable<LastDD> LastDD { get; set; }
    }


    public class LastDD
    {
        public DateTime dt { get; set; }
        public string imsi { get; set; }
        public string imei { get; set; }
        public string operatorId { get; set; }
        public Double d_lat { get; set; }
        public Double d_lon { get; set; }

        public int d_bat { get; set; }
        public int d_rs { get; set; }
        public Double d_temp { get; set; }
        public Double d_humi { get; set; }
        public int d_a_x { get; set; }
        public int d_a_y { get; set; }
        public int d_a_z { get; set; }
        public int d_type { get; set; }

    }





    public class DD
    {
        public DateTime dt { get; set; }
        public string imsi { get; set; }
        public string imei { get; set; }
        public string operatorId { get; set; }
        public Double d_lat { get; set; }
        public Double d_lon { get; set; }

        public int d_bat { get; set; }
        public int d_rs { get; set; }
        public Double d_temp { get; set; }
        public Double d_humi { get; set; }
        public int d_a_x { get; set; }
        public int d_a_y { get; set; }
        public int d_a_z { get; set; }
        public int d_type { get; set; }



        public string I_fileID { get; set; }
        public string I_folderID { get; set; }
        public DateTime I_uploadDt { get; set; }
        public Double I_lat { get; set; }
        public Double I_lon { get; set; }
        public int I_dataType { get; set; }
        public int I_releaseLV { get; set; }


        public Double bf_lat { get; set; }
        public Double bf_lon { get; set; }

        public Double diff_dt { get; set; }
        public Double diff_lat { get; set; }
        public Double diff_lon { get; set; }

        public Double Rownum { get; set; }
    }

    public class UserDevice
    {
        public DateTime userID { get; set; }
        public string imsi { get; set; }
        public string deviceType { get; set; }
        public string deviceName { get; set; }

        public Double Rownum { get; set; }
    }



    public class Imagelatlon
    {
        public DateTime fileID { get; set; }
        public DateTime folderID { get; set; }
        public DateTime uploadDt { get; set; }
        public Double lat { get; set; }
        public Double lon { get; set; }
        public string dataType { get; set; }
        public string releaseLV { get; set; }


    }









}