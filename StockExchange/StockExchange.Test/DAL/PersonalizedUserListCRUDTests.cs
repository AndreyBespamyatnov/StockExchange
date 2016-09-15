using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockExchange.DAL;
using StockExchange.Models;

namespace StockExchange.Test.DAL
{
    [TestClass]
    public class PersonalizedUserListCRUDTests
    {
        private readonly PersonalizedUserListCRUD _crud;
        private readonly StockEntriesCRUD _stockCrud;
        private readonly Guid _currentUserId;
        private readonly Random _rnd;

        public PersonalizedUserListCRUDTests()
        {
            _crud = new PersonalizedUserListCRUD();
            _stockCrud = new StockEntriesCRUD();
            _currentUserId = Guid.NewGuid();
            _rnd = new Random();
        }

        [TestMethod]
        public void Create()
        {
            var rndStockCode = GetRndStockCode();
            var entity = _crud.Create(_currentUserId, rndStockCode);

            Assert.IsTrue(entity.HasValue);
            Assert.IsTrue(entity.Value.StockCode == rndStockCode);
        }

        [TestMethod]
        public void Delete()
        {
            var rndStockCode = GetRndStockCode();
            _crud.Create(_currentUserId, rndStockCode);

            var entity = _crud.Delete(_currentUserId, rndStockCode);
            var entityNotExist = _crud.GetUserStocks(_currentUserId);

            Assert.IsTrue(entity.ErrorCode == ErrorCode.NoError);
            Assert.IsFalse(entityNotExist.Exists(stock => stock.StockCode == rndStockCode));
        }

        [TestMethod]
        public void GetUserStocks()
        {
            var rndStockCode = GetRndStockCode();
            _crud.Create(_currentUserId, rndStockCode);

            var entityExist = _crud.GetUserStocks(_currentUserId);
            Assert.IsTrue(entityExist.Exists(stock => stock.StockCode == rndStockCode));
        }

        private string GetRndStockCode()
        {
            var allData = _stockCrud.GetAll();
            var stockCode = allData[_rnd.Next(allData.Count)].Code;
            return stockCode;
        }
    }
}
