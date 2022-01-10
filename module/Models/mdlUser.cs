using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ThingsLine.Models
{

    public class mdlUser
    {
        public class U_MessageTask
        {
            public DateTime dt { get; set; }
            public string userID { get; set; }
            public int taskType { get; set; }
            public int taskCode { get; set; }
            public int taskCount { get; set; }
            public Double fdata01 { get; set; }
            public Double fdata02 { get; set; }
            public Double fdata03 { get; set; }
            public Double fdata04 { get; set; }
        }




    }
}
