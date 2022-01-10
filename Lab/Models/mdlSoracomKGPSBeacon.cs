using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ThingsLine.Models
{

    public class mdlDeviceSoracomKGPSBeacon
    {
        public class rcvJWTdataH
        {
            public string alg { get; set; }
            public string typ { get; set; }
            public string kid { get; set; }
        }

        public class Ctx
        {
            public string operatorId { get; set; }
            public string coverage { get; set; }
            public string resourceType { get; set; }
            public string resourceId { get; set; }
            public string sourceProtocol { get; set; }
            public string srn { get; set; }
            public string imsi { get; set; }
            public string imei { get; set; }
        }

        public class rcvJWTdata
        {
            public string iss { get; set; }
            public string aud { get; set; }
            public string jti { get; set; }
            public int iat { get; set; }
            public string typ { get; set; }
            public string sub { get; set; }
            public Ctx ctx { get; set; }
        }

        public class rcvBodyData
        {
            public int type { get; set; }
            public int bat { get; set; }
            public string timestamp { get; set; }
            public string loc_data { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string ns { get; set; }
            public string ew { get; set; }
            public string major_axis { get; set; }
            public string minor_axis { get; set; }

            public string uuid { get; set; }
            public string major { get; set; }
            public string minor { get; set; }
            public string rssi_b { get; set; }

            public int attr { get; set; }
            public string sns_valid_no { get; set; }
            public string adv_add { get; set; }
            public string sns1 { get; set; }
            public string sns2 { get; set; }
            public string sns3 { get; set; }
            public string sns4 { get; set; }
            public string sns5 { get; set; }
            public string sns6 { get; set; }
            public string sns7 { get; set; }
            public string sns8 { get; set; }
            public string sns9 { get; set; }
            public string sns10 { get; set; }
            public string sns11 { get; set; }
            public string sns12 { get; set; }
            public string sns13 { get; set; }
            public string sns14 { get; set; }
            public string sns15 { get; set; }
            public string sns16 { get; set; }
            public string sns17 { get; set; }
            public string sns18 { get; set; }
            public string sns19 { get; set; }
            public string sns20 { get; set; }
            public string sns21 { get; set; }
            public string sns22 { get; set; }
            public string sns23 { get; set; }
            public string sns24 { get; set; }

        }

    }
}
