using System;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockExchange.Authentication;
using StockExchange.DAL;
using StockExchange.Models;

namespace StockExchange.Test.StockExchangeServiceTest
{
    [TestClass]
    public class StockExchangeServiceTest
    {
        const string Password = "P@ssw0rd!@#!@#";

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
    }
}
