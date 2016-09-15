using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StockExchange.Authentication;
using StockExchange.BL;
using StockExchange.DAL;
using StockExchange.Models;

namespace StockExchange.Test.StockExchangeServiceTest
{
    [TestClass]
    public class StockExchangeServiceTest
    {
        const string Password = "P@ssw0rd!@#!@#";

        private readonly StockEntriesCRUD _crud;
        private readonly Random _rnd;

        public StockExchangeServiceTest()
        {
            _crud = new StockEntriesCRUD();
            _rnd = new Random();
        }

        [TestMethod]
        public void CreateNewUser()
        {
            var securedWebServiceHeader = new SecuredWebServiceHeader { Username = Guid.NewGuid().ToString(), Password = Password };

            var client = new StockExchangeService();

            var statusResult = client.CreateUser(new RegisterUserModel
            {
                UserName = securedWebServiceHeader.Username,
                UserPassword = securedWebServiceHeader.Password,
                UserEmail = "asd@asd.ru",
                UserQuestion = "asdasd",
                UserAnswer = "asdasd"
            });

            Assert.IsTrue(statusResult == MembershipCreateStatus.Success);
        }

        [TestMethod]
        public void AuthenticateUser()
        {
            var username = Guid.NewGuid().ToString();
            var securedWebServiceHeader = new SecuredWebServiceHeader { Username = username, Password = Password };

            var client = new StockExchangeService();
            client.CreateUser(new RegisterUserModel
            {
                UserName = securedWebServiceHeader.Username,
                UserPassword = securedWebServiceHeader.Password,
                UserEmail = "asd@asd.ru",
                UserQuestion = "asdasd",
                UserAnswer = "asdasd"
            });

            client.SoapHeader = securedWebServiceHeader;

            var token = client.AuthenticateUser();
            securedWebServiceHeader.AuthenticatedToken = token;

            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void AddStockToUser()
        {
            var username = Guid.NewGuid().ToString();
            var securedWebServiceHeader = new SecuredWebServiceHeader { Username = username, Password = Password };

            var client = new StockExchangeService();
            client.CreateUser(new RegisterUserModel
            {
                UserName = securedWebServiceHeader.Username,
                UserPassword = securedWebServiceHeader.Password,
                UserEmail = "asd@asd.ru",
                UserQuestion = "asdasd",
                UserAnswer = "asdasd"
            });

            client.SoapHeader = securedWebServiceHeader;

            var token = client.AuthenticateUser();
            securedWebServiceHeader.AuthenticatedToken = token;
            securedWebServiceHeader.UserId = client.GetCurrentUserId();

            var allData = _crud.GetAll();
            var stockCode = allData[_rnd.Next(allData.Count)].Code;

            string stringResult = client.AddStockToUser(securedWebServiceHeader.UserId, stockCode);
            var result = JsonConvert.DeserializeObject<StockExchangeProvider.StockResult<PersonalizedUserList>>(stringResult);

            Assert.IsNull(result.Error);
            Assert.IsTrue(result.ResultType == StockExchangeProvider.ResultType.Ok);
            Assert.IsTrue(result.Data.StockCode == stockCode);
            Assert.IsTrue(result.Data.UserId == securedWebServiceHeader.UserId);
            Assert.IsTrue(result.Data.Price > 0);
        }

        [TestMethod]
        public void RemoveStockFromUser()
        {
            var username = Guid.NewGuid().ToString();
            var securedWebServiceHeader = new SecuredWebServiceHeader { Username = username, Password = Password };

            var client = new StockExchangeService();
            client.CreateUser(new RegisterUserModel
            {
                UserName = securedWebServiceHeader.Username,
                UserPassword = securedWebServiceHeader.Password,
                UserEmail = "asd@asd.ru",
                UserQuestion = "asdasd",
                UserAnswer = "asdasd"
            });

            client.SoapHeader = securedWebServiceHeader;

            var token = client.AuthenticateUser();
            securedWebServiceHeader.AuthenticatedToken = token;
            securedWebServiceHeader.UserId = client.GetCurrentUserId();

            var allData = _crud.GetAll();
            var stockCode = allData[_rnd.Next(allData.Count)].Code;

            var addStringResult = client.AddStockToUser(securedWebServiceHeader.UserId, stockCode);
            var addResult = JsonConvert.DeserializeObject<StockExchangeProvider.StockResult<PersonalizedUserList>>(addStringResult);

            string stringResult = client.RemoveStockFromUser(securedWebServiceHeader.UserId, stockCode);
            var result = JsonConvert.DeserializeObject<StockExchangeProvider.StockResult<PersonalizedUserList>>(stringResult);

            Assert.IsNull(addResult.Error);
            Assert.IsTrue(addResult.ResultType == StockExchangeProvider.ResultType.Ok);
            Assert.IsTrue(addResult.Data.StockCode == stockCode);
            Assert.IsTrue(addResult.Data.UserId == securedWebServiceHeader.UserId);
            Assert.IsTrue(addResult.Data.Price > 0);

            Assert.IsNull(result.Error);
            Assert.IsTrue(result.ResultType == StockExchangeProvider.ResultType.Ok);
            Assert.IsTrue(result.Data == default(PersonalizedUserList));
        }

        [TestMethod]
        public void GetStocks()
        {
            var username = Guid.NewGuid().ToString();
            var securedWebServiceHeader = new SecuredWebServiceHeader { Username = username, Password = Password };

            var client = new StockExchangeService();
            client.CreateUser(new RegisterUserModel
            {
                UserName = securedWebServiceHeader.Username,
                UserPassword = securedWebServiceHeader.Password,
                UserEmail = "asd@asd.ru",
                UserQuestion = "asdasd",
                UserAnswer = "asdasd"
            });

            client.SoapHeader = securedWebServiceHeader;

            var token = client.AuthenticateUser();
            securedWebServiceHeader.AuthenticatedToken = token;
            securedWebServiceHeader.UserId = client.GetCurrentUserId();

            var stringResult = client.GetStocks();
            var result = JsonConvert.DeserializeObject<StockExchangeProvider.StockResult<List<Stock>>>(stringResult);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Error);
            Assert.IsTrue(result.ResultType == StockExchangeProvider.ResultType.Ok);
            Assert.IsTrue(result.Data.Any());
        }

        [TestMethod]
        public void GetUserStocks()
        {
            var username = Guid.NewGuid().ToString();
            var securedWebServiceHeader = new SecuredWebServiceHeader { Username = username, Password = Password };

            var client = new StockExchangeService();
            client.CreateUser(new RegisterUserModel
            {
                UserName = securedWebServiceHeader.Username,
                UserPassword = securedWebServiceHeader.Password,
                UserEmail = "asd@asd.ru",
                UserQuestion = "asdasd",
                UserAnswer = "asdasd"
            });

            client.SoapHeader = securedWebServiceHeader;

            var token = client.AuthenticateUser();
            securedWebServiceHeader.AuthenticatedToken = token;
            securedWebServiceHeader.UserId = client.GetCurrentUserId();

            var allData = _crud.GetAll();
            var stockCode = allData[_rnd.Next(allData.Count)].Code;

            string addStringResult = client.AddStockToUser(securedWebServiceHeader.UserId, stockCode);
            var addResult = JsonConvert.DeserializeObject<StockExchangeProvider.StockResult<PersonalizedUserList>>(addStringResult);

            string stringResult = client.GetUserStocks(securedWebServiceHeader.UserId);
            var result = JsonConvert.DeserializeObject<StockExchangeProvider.StockResult<List<PersonalizedUserList>>>(stringResult);

            Assert.IsNull(result.Error);
            Assert.IsTrue(result.ResultType == StockExchangeProvider.ResultType.Ok);
            Assert.IsTrue(result.Data.Any(list => list.StockCode == stockCode));
            Assert.IsTrue(result.Data.Any(list => list.UserId == securedWebServiceHeader.UserId));
        }
    }
}
