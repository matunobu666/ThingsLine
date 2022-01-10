using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ThingsLine.Models
{

    public class mdlDevice
    {
        public class DeviceBaseData
        {
            public DateTime dt { get; set; }
            public string imsi { get; set; }
            public Double d_lat { get; set; }
            public Double d_lon { get; set; }
            //-1~100 
            public int d_bat { get; set; }
            //
            public int d_type { get; set; }
        }

        public class U_Device
        {
            public string userID { get; set; }
            public string imsi { get; set; }
            public string deviceType { get; set; }
            public string deviceName { get; set; }
            public int errCNT { get; set; }
            public int stopFLG { get; set; }
            public string bikeID { get; set; }
        }




        public class DD_Soracom000
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
            public DD_Soracom000() { }
        }

    }
}
