using System;
using System.Collections.Generic;
using DapperExtensions;
using StockExchange.Models;

namespace StockExchange.DAL
{
    public class PersonalizedUserListCRUD : BaseCRUD
    {
        public List<PersonalizedUserList> GetUserStocks(Guid userId)
        {
            // Get all Stock codes for user
            var p = Predicates.Field<PersonalizedUserList>(f => f.UserId, Operator.Eq, userId);
            var result = GetListSafe<PersonalizedUserList>(p);

            return result;
        }

        public StatusValuePair<PersonalizedUserList> Create(Guid userId, string stockCode)
        {
            var result = Create(new PersonalizedUserList
            {
                StockCode = stockCode,
                UserId = userId
            });

            return result;
        }

        public StatusValuePair<PersonalizedUserList> Delete(Guid userId, string stockCode)
        {
            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<PersonalizedUserList>(f => f.UserId, Operator.Eq, userId));
            pg.Predicates.Add(Predicates.Field<PersonalizedUserList>(f => f.StockCode, Operator.Eq, stockCode));

            var result = Delete<PersonalizedUserList>(pg);
            return result;
        }
    }
}