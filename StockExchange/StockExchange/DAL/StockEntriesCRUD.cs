using System.Collections.Generic;
using DapperExtensions;
using StockExchange.Extensions;
using StockExchange.Models;

namespace StockExchange.DAL
{
    public class StockEntriesCRUD : BaseCRUD
    {
        public List<Stock> GetAll(string filterPrefix = "")
        {
            var p = Predicates.Field<Stock>(f => f.Code, Operator.Like, filterPrefix + "%");

            var result = string.IsNullOrWhiteSpace(filterPrefix) ? base.GetList<Stock>() : base.GetList<Stock>(p);

            if (result.HasValue)
            {
                return result.Value;
            }

            true.BreakOnTrue("List of Stoks is empty");
            return new List<Stock>(0);
        }

        public List<Stock> GetByCodes(IEnumerable<string> codes)
        {
            var pg = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };

            foreach (var code in codes)
            {
                pg.Predicates.Add(Predicates.Field<Stock>(f => f.Code, Operator.Eq, code));
            }

            var stockEntries = GetListSafe<Stock>(pg);

            return stockEntries;
        }
    }
}