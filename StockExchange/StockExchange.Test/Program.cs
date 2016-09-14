using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockExchange.Test.ServiceReference1;

namespace StockExchange.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var securedWebServiceHeader = new SecuredWebServiceHeader();
            securedWebServiceHeader.Username = "asdasd";
            securedWebServiceHeader.Password = "P@ssw0rd!@#!@#";

            var client = new StockExchangeServiceSoapClient();

            client.Open();

            var user = client.CreateUser(new RegisterUserModel
            {
                UserName = securedWebServiceHeader.Username, 
                UserPassword = securedWebServiceHeader.Password, 
                UserEmail = "asd@asd.ru", 
                UserQuestion = "asdasd", 
                UserAnswer = "asdasd"
            });

            Console.WriteLine("User creation status - {0}", user);

            var token = client.AuthenticateUser(securedWebServiceHeader);
            securedWebServiceHeader.AuthenticatedToken = token;

            string stockPrices = client.GetStockPrices(securedWebServiceHeader, new []{1,2});

            Console.WriteLine(stockPrices);

            Console.ReadKey();
        }
    }
}
