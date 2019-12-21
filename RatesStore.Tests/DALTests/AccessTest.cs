using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RatesStore.DAL.Models;

namespace RatesStore.DAL.Repository
{
    [TestClass]
    public class UnitOfWorkTests
    {
        private static readonly UnitOfWork unitOfWork = new UnitOfWork();

        [TestMethod]
        public void CreateValueInTable()
        {
            var count = unitOfWork.Repository<Rate>().GetCount();
            unitOfWork.Repository<Rate>().Add(new Rate() { Name = "test" });
            unitOfWork.Save();
            Assert.AreNotEqual(count, unitOfWork.Repository<Rate>().GetCount());
        }
    }
}
