
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThingsLine.Models;
using ThingsLine.Modules;

namespace ThingsLine.Modules.UnitTest
{
    [TestClass]
    public class modUMessageTask総合テスト
    {

        modUMessageTask mUMessageTask = new modUMessageTask();
        //-----------------------------
        //テストデータ作成
        U_MessageTask setU_MessageTask = new U_MessageTask
        {
            imsi = "999999999999999",
            userID = "99999999-9999-9999-aaaa-9999999999",
            msgType = 9,
            msgCode = 9,
            msgCount = 9
        };
        [TestMethod]
        public void a01_insert処理テスト()
        {
            Exception retEX = mUMessageTask.SetUMessageTask(setU_MessageTask);
            Assert.IsNull(retEX); // 成功
        }
        [TestMethod]
        public void a02_insert処理確認()
        {
            List<U_MessageTask> retU_MessageTask = mUMessageTask.GetUMessageTask(setU_MessageTask.imsi, setU_MessageTask.userID);
            int testse = retU_MessageTask.Count;
            Assert.AreEqual(testse,1);
        }
        [TestMethod]
        public void a03_delete処理テスト()
        {
            Exception retEX = mUMessageTask.DelUMessageTask(setU_MessageTask.imsi);
            Assert.IsNull(retEX); // 成功
        }
        [TestMethod]
        public void a04_delete処理確認()
        {
            List<U_MessageTask> retU_MessageTask = mUMessageTask.GetUMessageTask(setU_MessageTask.imsi, setU_MessageTask.userID);
            int testse = retU_MessageTask.Count;
            Assert.AreEqual(testse, 0);
        }
    }
}
