using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RatesStore.BLL.Utils;
using RatesStore.DAL.Models;
using RatesStore.DAL.Repository;
using System.Threading;
using System.Diagnostics;

namespace RatesStore.BLL.Utils
{
    [TestClass]
    public class LoggerTest
    {
        private static readonly UnitOfWork unitOfWork = new UnitOfWork();

        [TestMethod]
        public void CreateValueInTable()
        {
            var count = unitOfWork.Repository<Log>().GetCount();
            DbLogger.LogError(new Exception(), "test");
            DbLogger.LogMessage("test");

            //since logging is async operations
            Thread.Sleep(1000);
            
            Assert.AreNotEqual(count, unitOfWork.Repository<Rate>().GetCount());
        }
        
        [TestMethod]
        public void LoadTestingLogger()
        {
            var count = unitOfWork.Repository<Log>().GetCount();

            for (int i = 0; i < 10000; i++)
                DbLogger.LogError(new Exception(), "test" + i);

            Thread.Sleep(5000);

            var updatedCount = unitOfWork.Repository<Log>().GetCount();
            Assert.AreNotEqual(count, unitOfWork.Repository<Log>().GetCount(),
                $"{count},{updatedCount}");
            
        }
    }
}
