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
    public class MainTimer
    {

        [FunctionName("MainTimer")]
        public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
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
            modUMessageTask mUMessageTask  = new modUMessageTask();
            modUSetting       mUSetting=new          modUSetting();
            modBatteryCheck mBatteryCheck = new  modBatteryCheck();
            modBikeDeviceBaseData mBikeDeviceBaseData =new modBikeDeviceBaseData();
            //���b�Z�[�W�����p
            modSYS mSYS = new modSYS();
            var fullClassName = "ThingsLineAPIs.Events";            // ���O��Ԃ܂Ŋ܂߂ăN���X�����擾
            var methodName = MethodBase.GetCurrentMethod().Name;            // ���\�b�h�����擾
            string stepMSG = "Start"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);



            try
            {

                //----------------------
                //�f�o�C�X���X�g�擾
                List<UserBaseData> retmUserBaseData = mUserBaseData.DeviceList();

                //----------------------------------------------
                //�o�b�e���[�`�F�b�N
                stepMSG = "Bat LOW CHK"; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

                foreach (UserBaseData d in retmUserBaseData)
                {

                    Boolean retb = mBatteryCheck.SetUMessageTask(d.imsi, d.userID);
                    stepMSG = "Bat CHK" + retb; Console.WriteLine("[" + fullClassName + "] " + "[" + methodName + "] " + stepMSG);

                }

                //----------------------------------------------
                //���b�Z�[�W���M����

                //���b�Z�[�W���X�g���擾�iLIST
                List<U_MessageTask> �@getU_MessageTask = mUMessageTask.GetUMessageTask();

                foreach (U_MessageTask d in getU_MessageTask)
                {
                    //���MOK���f�i�J�E���^
                    if (d.msgCount > 0)
                    {


                        List<U_Setting> getU_Setting = mUSetting.GetList(d.userID);
                        foreach (U_Setting US in getU_Setting)
                        {
                            //����Ԏ擾�iNOW refVal_hh
                            int refVal_hh = DateTime.Now.AddHours(9).Hour;

                            //���[�U�[�����Ԃ����`�F�b�N
                            if (US.valueInt01 == refVal_hh || US.valueInt01 == -1)
                            {
                                List<UserBaseData> retUBD = mUserBaseData.imsi2UserBaseData(d.imsi);

                                List<BikeDeviceBaseData> retDBD = mBikeDeviceBaseData.BikeDeviceChk(retUBD[0].LINEID, d.imsi);
                                //���M����
                                //�e�L�X�g���M
                                if (d.msgType == 1 &&  d.msgCode == 1)
                                {
                                    Task<Exception> EX = mLINE.sendTextMSGAsyncTo(retUBD[0].LINEID
                                        , retDBD[0].bikeName + ""
                                        + "\n" + "�o�b�e���[�ʒm"
                                        + "\n" + "�o�b�e���[�������Ă��܂�"
                                        );
                                }else if (d.msgType == 1 && d.msgCode == 2)
                                {
                                    Task<Exception> EX = mLINE.sendTextMSGAsyncTo(retUBD[0].LINEID
                                        , retDBD[0].bikeName + ""
                                        + "\n" + "�o�b�e���[�ʒm"
                                        + "\n" + "�ʐM��24���Ԉȏ゠��܂���"
                                        + "\n" + "�o�b�e���[�؂�̉\��������܂�"
                                        );
                                }
                                //�J�E���^�_�E��
                                Exception retex = mUMessageTask.UpdateCount(d);

                                if (US.valueInt01 == -1) { break; }
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
