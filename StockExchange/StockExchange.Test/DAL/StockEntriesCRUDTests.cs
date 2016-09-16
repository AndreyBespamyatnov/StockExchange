using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockExchange.DAL;

namespace StockExchange.Test.DAL
{
    [TestClass]
    public class StockEntriesCRUDTests
    {
        private readonly StockEntriesCRUD _crud;
        private readonly Random _rnd;

        public StockEntriesCRUDTests()
        {
            _crud = new StockEntriesCRUD();
            _rnd = new Random();
        }

        [TestMethod]
        public void GetAllData()
        {
            var entries = _crud.GetAll();
            Assert.IsNotNull(entries);
            Assert.IsTrue(entries.Any());
        }

        [TestMethod]
        public void GetAllDataByFilter()
        {
            var entries = _crud.GetAll("A");
            Assert.IsNotNull(entries);
            Assert.IsTrue(entries.Any());
        }

        [TestMethod]
        public void GetByCodes()
        {
            var allData = _crud.GetAll();

            var stock1 = allData[_rnd.Next(allData.Count)].Code;
            var stock2 = allData[_rnd.Next(allData.Count)].Code;
            var stock3 = allData[_rnd.Next(allData.Count)].Code;
            var stock4 = allData[_rnd.Next(allData.Count)].Code;

            var entries = _crud.GetByCodes(new List<string> { stock1, stock2, stock3, stock4});

            Assert.IsNotNull(entries);
            Assert.IsTrue(entries.Any(stock => stock.Code == stock1));
            Assert.IsTrue(entries.Any(stock => stock.Code == stock2));
            Assert.IsTrue(entries.Any(stock => stock.Code == stock3));
            Assert.IsTrue(entries.Any(stock => stock.Code == stock4));
        }
    }
}
