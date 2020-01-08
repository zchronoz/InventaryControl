using IC.Data.Context;
using IC.Data.Repositories;
using IC.Domain.Entities;
using IC.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace IC.UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var mockSet = new Mock<DbSet<Equipment>>();

            mockSet.Setup(s => s.Create())
                .Returns(() =>
                {
                    Mock<Equipment> mock = new Mock<Equipment>();
                    return mock.Object;
                });

            var mockContext = new Mock<InventoryControlContext>();
            mockContext.Setup(m => m.Set<Equipment>()).Returns(mockSet.Object);
            var mockRepository = new Mock<EquipmentRepository>();

            var service = new EquipmentService(mockRepository.Object);
            service.Add(new Equipment() { Code = "abcdef", DateAcquisition = DateTime.Now, ModelEquipment = "modelo", TypeEquipment = "tipo", ValueAcquisition = 1000 });

            var equip = service.GetByCode("abcdef");

            Assert.IsTrue(equip != null);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var mockSet = new Mock<DbSet<Equipment>>();

            mockSet.Setup(s => s.Create())
                .Returns(() =>
                {
                    Mock<Equipment> mock = new Mock<Equipment>();
                    return mock.Object;
                });

            var mockContext = new Mock<InventoryControlContext>();
            mockContext.Setup(m => m.Set<Equipment>()).Returns(mockSet.Object);
            var mockRepository = new Mock<EquipmentRepository>();

            var service = new EquipmentService(mockRepository.Object);
            var equip = service.GetByCode("abcdef");
            equip.TypeEquipment = "UNITTEST";
            service.Update(equip);

            Assert.IsTrue(equip.TypeEquipment.Equals("UNITTEST"));
        }

        [TestMethod]
        public void TestSelect()
        {
            var mockSet = new Mock<DbSet<Equipment>>();

            mockSet.Setup(s => s.Create())
                .Returns(() =>
                {
                    Mock<Equipment> mock = new Mock<Equipment>();
                    return mock.Object;
                });

            var mockContext = new Mock<InventoryControlContext>();
            mockContext.Setup(m => m.Set<Equipment>()).Returns(mockSet.Object);
            var mockRepository = new Mock<EquipmentRepository>();

            var service = new EquipmentService(mockRepository.Object);
            var equip = service.GetByCode("abcdef");

            Assert.IsTrue(equip != null);
        }

        [TestMethod]
        public void TestList()
        {
            var mockSet = new Mock<DbSet<Equipment>>();

            mockSet.Setup(s => s.Create())
                .Returns(() =>
                {
                    Mock<Equipment> mock = new Mock<Equipment>();
                    return mock.Object;
                });

            var mockContext = new Mock<InventoryControlContext>();
            mockContext.Setup(m => m.Set<Equipment>()).Returns(mockSet.Object);
            var mockRepository = new Mock<EquipmentRepository>();

            var service = new EquipmentService(mockRepository.Object);
            var equips = service.GetAll();

            if (equips.GetType().Equals(typeof(List<Equipment>))) ;
        }

        [TestMethod]
        public void TestDelete()
        {
            var mockSet = new Mock<DbSet<Equipment>>();

            mockSet.Setup(s => s.Create())
                .Returns(() =>
                {
                    Mock<Equipment> mock = new Mock<Equipment>();
                    return mock.Object;
                });

            var mockContext = new Mock<InventoryControlContext>();
            mockContext.Setup(m => m.Set<Equipment>()).Returns(mockSet.Object);
            var mockRepository = new Mock<EquipmentRepository>();

            var service = new EquipmentService(mockRepository.Object);
            var equip = service.GetByCode("abcdef");
            service.Remove(equip);

            equip = service.GetByCode("abcdef");
            Assert.IsTrue(equip == null);
        }
    }
}