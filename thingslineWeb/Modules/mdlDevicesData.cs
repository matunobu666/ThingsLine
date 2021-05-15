using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace thingslineWeb.Modules
{
    public class mdlDevicesData
    {
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

        }


        public class GMPoint
        {
            public int Id { get; set; }
            public string PlaceName { get; set; }
            public string OpeningHours { get; set; }
            public string GeoLong { get; set; }
            public string GeoLat { get; set; }
        }






        public class rcvJWTdata
        {
            public string imsi { get; set; }
            public string imei { get; set; }
            public IEnumerable<DD> DD { get; set; }
        }



        public class DD
        {
            public DateTime dt { get; set; }
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