using System;

namespace StockExchange.BL
{
    public interface IStockExchangeProvider
    {
        string AddStockToUser(Guid userId, string stockCode);
        string RemoveStockFromUser(Guid userId, string stockCode);
        string GetStocks(string prefixFilter);
        string GetUserStocks(Guid userId);
    }
}