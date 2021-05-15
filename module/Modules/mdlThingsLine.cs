using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace module
{
    public class mdlThingsLine
    {


        /*------------------------------------*/
        /* SELECT（LIST）
        /*------------------------------------*/
        public List<modThingsLine.AspNetUsers> GetAspNetUsers(modThingsLine.AspNetUsers getSQL)
        {
            module.mdlSQLServer mdlSQLServer = new mdlSQLServer();
            StringBuilder sSQL = null ;
            try
            {

                //  基本情報
                sSQL.Clear();
                sSQL.Append("SELECT"
                                + " [Id] ,[Email],[UserName] "
                        + " FROM"
                                + " [dbo].[AspNetUsers] "
                        + " WHERE"
                                + " Email = '" + getSQL.Email + "'");
                
                Console.WriteLine("[GetAspNetUsers] sSQL: " + sSQL.ToString());
                List<modThingsLine.AspNetUsers> retData = mdlSQLServer.GetSQL<modThingsLine.AspNetUsers>(sSQL);
                return retData;
            }
            catch (Exception ex){
                Console.Error.WriteLine("[GetAspNetUsers] sSQL: " + sSQL.ToString());
                Console.Error.WriteLine("[GetAspNetUsers] ERR: " + ex.Message.ToString());
                Console.Error.WriteLine("[GetAspNetUsers] END(Err) ");
                return null;
            }

        }





    }
}
