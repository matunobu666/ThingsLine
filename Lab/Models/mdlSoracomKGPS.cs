using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ThingsLine.Models
{

    public class mdlDeviceSoracomKGPS
    {
        public class DeviceSoracomKGPS
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

    }
}
