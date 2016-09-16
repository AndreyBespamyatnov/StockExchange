using System;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.Services.Protocols;
using StockExchange.Authentication;
using StockExchange.BL;

namespace StockExchange
{
    [ScriptService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [WebService(Description = "Stock Exchange Web Service", Namespace = XmlNs)]
    public class StockExchangeService : WebService, IStockExchangeProvider
    {
        const string XmlNs = "http://asmx.crossover.com/";

        public SecuredWebServiceHeader SoapHeader;

        readonly AuthenticationHelper _authenticationHelper;
        readonly StockExchangeProvider _stockExchangeProvider;

        public StockExchangeService()
        {
            _authenticationHelper = new AuthenticationHelper();
            _stockExchangeProvider = new StockExchangeProvider();
        }

        [WebMethod]
        [SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AuthenticateUser()
        {
            var authenticateUserResult = _authenticationHelper.AuthenticateUser(SoapHeader);
            return authenticateUserResult;
        }

        [WebMethod]
        [SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool IsUserValid()
        {
            var isUserValid = _authenticationHelper.IsUserValid(SoapHeader);
            return isUserValid;
        }

        [WebMethod]
        [SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void LogOff()
        {
            _authenticationHelper.LogOff(SoapHeader);
        }

        [WebMethod]
        [SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Guid GetCurrentUserId()
        {
            var userId = _authenticationHelper.GetCurrentUserId(SoapHeader);
            return userId;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public MembershipCreateStatus CreateUser(Models.RegisterUserModel registerUserModel)
        {
            var membershipCreateStatus = _authenticationHelper.MembershipCreateUser(registerUserModel);
            return membershipCreateStatus;
        }

        [WebMethod]
        [SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddStockToUser(Guid userId, string stockCode)
        {
            if (!_authenticationHelper.IsUserValid(SoapHeader))
            {
                return "Please call AuthenitcateUser() first.";
            }

            var result = _stockExchangeProvider.AddStockToUser(userId, stockCode);
            return result;
        }

        [WebMethod]
        [SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string RemoveStockFromUser(Guid userId, string stockCode)
        {
            if (!_authenticationHelper.IsUserValid(SoapHeader))
            {
                return "Please call AuthenitcateUser() first.";
            }

            var result = _stockExchangeProvider.RemoveStockFromUser(userId, stockCode);
            return result;
        }

        [WebMethod]
        [SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetStocks(string prefixFilter)
        {
            if (!_authenticationHelper.IsUserValid(SoapHeader))
            {
                return "Please call AuthenitcateUser() first.";
            }

            var result = _stockExchangeProvider.GetStocks(prefixFilter);
            return result;
        }

        [WebMethod]
        [SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetUserStocks(Guid userId)
        {
            if (!_authenticationHelper.IsUserValid(SoapHeader))
            {
                return "Please call AuthenitcateUser() first.";
            }

            var result = _stockExchangeProvider.GetUserStocks(userId);
            return result;
        }
    }
}