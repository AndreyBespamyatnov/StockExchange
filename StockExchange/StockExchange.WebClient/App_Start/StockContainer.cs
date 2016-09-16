using System;
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
            if (AuthenticationContainer.Instance.Header != null)
            {
                var stocks = _client.GetStocks(AuthenticationContainer.Instance.Header, prefix);
                return stocks;
            }

            return string.Empty;
        }

        public string AddStockToUser(string stockCode)
        {
            if (AuthenticationContainer.Instance.Header != null)
            {
                var userId = AuthenticationContainer.Instance.UserId;
                var result = _client.AddStockToUser(AuthenticationContainer.Instance.Header, userId, stockCode);
                return result;
            }

            return string.Empty;
        }
    }
}