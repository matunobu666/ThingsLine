using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ThingsLine.Event;
using ThingsLine.Models;
using ThingsLine.Modules;
using ThingsLineAPIs.Models;
using ThingsLineAPIs.Modules;
using static System.Console;
using static ThingsLine.Models.mdlDevice;

namespace ThingsLineAPIs.Events
{
    public static class Timer10
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            //-------------------------------------------------
            //ThingsLineAPIs.Modules
            //LINE
            mdlLineMsg LineAPIObj = new mdlLineMsg();
            modLINEMsg mLINE = new modLINEMsg();
            //SQLserver
            modSQLServer mSQLServer = new modSQLServer();
            StringBuilder sSQL = new StringBuilder();
            //Thingsline
            modThingsLine mThingsLine = new modThingsLine();
            modUserBaseData mUserBaseData = new modUserBaseData();
            modSSystemMessage mSSystemMessage = new modSSystemMessage();
            modDevice mDevice = new modDevice();
            modUMessageTask mUMessageTask = new modUMessageTask();
            modUSetting mUSetting = new modUSetting();
            modBatteryCheck mBatteryCheck = new modBatteryCheck();
            modBikeDeviceBaseData mBikeDeviceBaseData = new modBikeDeviceBaseData();
            modTheftMonitoring mTheftMonitoring = new modTheftMonitoring();
            //���b�Z�[�W�����p
            modSYS mSYS = new modSYS();
            var fullClassName = "ThingsLineAPIs.Events";            // ���O��Ԃ܂Ŋ܂߂ăN���X�����擾
            var methodName = MethodBase.GetCurrentMethod().Name;            // ���\�b�h�����擾
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);


            try
           {
                //�f�o�C�X���X�g�擾�i�Ď����[�hOFF�̂݁j
                //----------------------
                //�f�o�C�X���X�g�擾
                List<UserBaseData> retmUserBaseData = mUserBaseData.DeviceList();

                //----------------------------------------------
                //�����imsi��10���J�E���g������ē���̉񐔈ȉ��Ȃ��ԂƔ��f
                foreach (UserBaseData d in retmUserBaseData)
                {
                    //�ŏI�ʐM�f�[�^�擾
                    DeviceBaseData DBD = mDevice.LastData("data",d.imsi);

                    //�����imsi��10���J�E���g������ē���̉񐔈ȉ��Ȃ��ԂƔ��f
                    if (DBD!=null)
                    {
                        //�����imsi��10���J�E���g������ē���̉񐔈ȉ��Ȃ��ԂƔ��f
                        if (DBD.dt <= DateTime.Now.AddMinutes(-10))
                        {
                            //�o�^�ς݃z�[���|�C���g���X�g
                            List<U_Setting>  retUS = mUSetting.GetList(d.userID, 2, 1);
                            foreach (U_Setting US in retUS)
                            {
                                //�ʒu�v�Z
                                double hu = mThingsLine.hubeny_distance(DBD.d_lat, DBD.d_lon,   US.valueflt01, US.valueflt02);
                                if(hu < US.valueInt01)
                                {
                                    //�Ď����[�hON
                                    //�Ď��J�n����
                                    Exception EX = mTheftMonitoring.TheftMonitoringStart(d.userID, d.imsi);

                                }
                            }
                        }
                    }
                }












                stepMSG = "END"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

            }
            catch (Exception ex)
            {
                mSYS.Log2DBERR("ThingsLineAPIs.Events", methodName, ex.ToString(), stepMSG);

            }
        }

    }
}
