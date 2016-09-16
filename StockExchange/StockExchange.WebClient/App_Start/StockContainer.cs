using System;
using System.Web;
using StockExchange.WebClient.StockExchangeServiceReference;

namespace StockExchange.WebClient
{
    public class StockContainer
    {
        private static readonly Lazy<StockContainer> Lazy = new Lazy<StockContainer>(() => new StockContainer());

        private readonly StockExchangeServiceSoapClient _client;

        public static StockContainer Instance { get { return Lazy.Value; } }

        private StockContainer()
        {
            _client = new StockExchangeServiceSoapClient();
        }

        public string GetStocks(string prefix)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookiesName);
            if (cookie != null)
            {
                string token = cookie[Constants.UserTokenKeyName];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var stocks = _client.GetStocks(new SecuredWebServiceHeader {AuthenticatedToken = token}, prefix);
                    return stocks;
                }
            }

            return string.Empty;
        }

        public string AddStockToUser(string stockCode)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookiesName);
            if (cookie != null)
            {
                string token = cookie[Constants.UserTokenKeyName];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var userId = AuthenticationContainer.Instance.UserId;
                    var result = _client.AddStockToUser(new SecuredWebServiceHeader {AuthenticatedToken = token}, userId,
                        stockCode);
                    return result;
                }
            }

            return string.Empty;
        }

        public string RemoveStockFromUser(string stockCode)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookiesName);
            if (cookie != null)
            {
                string token = cookie[Constants.UserTokenKeyName];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var userId = AuthenticationContainer.Instance.UserId;
                    var result = _client.RemoveStockFromUser(new SecuredWebServiceHeader {AuthenticatedToken = token},
                        userId, stockCode);
                    return result;
                }
            }

            return string.Empty;
        }

        public string GetUserStocks()
        {
                        HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookiesName);
            if (cookie != null)
            {
                string token = cookie[Constants.UserTokenKeyName];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var userId = AuthenticationContainer.Instance.UserId;
                    var result = _client.GetUserStocks(new SecuredWebServiceHeader {AuthenticatedToken = token}, userId);
                    return result;
                }
            }

            return string.Empty;
        }
    }
}