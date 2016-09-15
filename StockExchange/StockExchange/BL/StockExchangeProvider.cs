using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StockExchange.DAL;
using StockExchange.Extensions;
using StockExchange.Models;

namespace StockExchange.BL
{
    public class StockExchangeProvider : IStockExchangeProvider
    {
        private readonly PersonalizedUserListCRUD _userStockCrud;
        private readonly StockEntriesCRUD _stockCrud;
        private readonly Random _rnd;

        public StockExchangeProvider()
        {
            _userStockCrud = new PersonalizedUserListCRUD();
            _stockCrud = new StockEntriesCRUD();
            _rnd = new Random();
        }

        public string AddStockToUser(Guid userId, string stockCode)
        {
            var result = new StockResult<PersonalizedUserList>();
            try
            {
                result = new StockResult<PersonalizedUserList>();
                StatusValuePair<PersonalizedUserList> deleteResult = _userStockCrud.Create(userId, stockCode);

                if (!deleteResult.HasValue)
                {
                    throw new Exception(string.Format("Can't create Stoke with Code '{0}' for user '{1}', error code '{2}'", stockCode, userId, deleteResult.ErrorCode));
                }

                // Set Price for stocks
                UpdatePrices(deleteResult.Value);

                result.Data = deleteResult.Value;
                result.ResultType = ResultType.Ok;
            }
            catch (Exception ex)
            {
                result.ResultType = ResultType.Error;
                result.Error = new ServiceError { Message = ex.Message, Type = ErrorType.Undefined };
            }

            string jsonResult = JsonConvert.SerializeObject(result);
            return jsonResult;
        }

        public string RemoveStockFromUser(Guid userId, string stockCode)
        {
            var result = new StockResult<PersonalizedUserList>();
            try
            {
                result = new StockResult<PersonalizedUserList>();
                StatusValuePair<PersonalizedUserList> deleteResult = _userStockCrud.Delete(userId, stockCode);

                if (deleteResult.ErrorCode != ErrorCode.NoError)
                {
                    throw new Exception(string.Format("Can't delete Stoke with Code '{0}' for user '{1}', error code '{2}'", stockCode, userId, deleteResult.ErrorCode));
                }

                result.ResultType = ResultType.Ok;
            }
            catch (Exception ex)
            {
                result.ResultType = ResultType.Error;
                result.Error = new ServiceError { Message = ex.Message, Type = ErrorType.Undefined };
            }

            string jsonResult = JsonConvert.SerializeObject(result);
            return jsonResult;
        }

        public string GetStocks()
        {
            var result = new StockResult<IEnumerable<Stock>>();
            try
            {
                result = new StockResult<IEnumerable<Stock>>();
                IEnumerable<Stock> stocks = _stockCrud.GetAll();
                result.Data = stocks;
                result.ResultType = ResultType.Ok;
            }
            catch (Exception ex)
            {
                result.ResultType = ResultType.Error;
                result.Error = new ServiceError {Message = ex.Message, Type = ErrorType.Undefined};
            }

            string jsonResult = JsonConvert.SerializeObject(result);
            return jsonResult;
        }

        public string GetUserStocks(Guid userId)
        {
            var result = new StockResult<IEnumerable<PersonalizedUserList>>();
            try
            {
                result = new StockResult<IEnumerable<PersonalizedUserList>>();
                IEnumerable<PersonalizedUserList> stocks = _userStockCrud.GetUserStocks(userId);

                // Set Price for stocks
                UpdatePrices(stocks);

                result.Data = stocks;
                result.ResultType = ResultType.Ok;
            }
            catch (Exception ex)
            {
                result.ResultType = ResultType.Error;
                result.Error = new ServiceError { Message = ex.Message, Type = ErrorType.Undefined };
            }

            string jsonResult = JsonConvert.SerializeObject(result);
            return jsonResult;
        }

        private void UpdatePrices(PersonalizedUserList personalizedUserList)
        {
            if (personalizedUserList != null) personalizedUserList.Price = _rnd.Next(1, 1000);
        }

        private void UpdatePrices(IEnumerable<PersonalizedUserList> personalizedUserLists)
        {
            personalizedUserLists.ForEach(UpdatePrices);
        }

        public class ServiceError
        {
            public ErrorType Type { get; set; }
            public string Message { get; set; }
        }

        public enum ResultType
        {
            Error,
            Ok
        }

        public enum ErrorType
        {
            Undefined,
            DbConnection,
            InvalidArgument,
            Forbidden
        }

        public class StockResult<T>
        {
            public ResultType ResultType { get; set; }
            public ServiceError Error { get; set; }
            public T Data { get; set; }
        }
    }
}