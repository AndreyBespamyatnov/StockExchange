using System.Collections.Generic;
using DapperExtensions;
using StockExchange.Extensions;
using StockExchange.Models;

namespace StockExchange.DAL
{
    public class StockEntriesCRUD : BaseCRUD
    {
        public List<StockEntry> GetAll()
        {
            var result = base.GetList<StockEntry>();

            if (result.HasValue)
            {
                return result.Value;
            }

            true.BreakOnTrue("List of Stoks is empty");
            return new List<StockEntry>(0);
        }

        public List<StockEntry> GetByCodes(IEnumerable<string> codes)
        {
            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            foreach (var code in codes)
            {
                pg.Predicates.Add(Predicates.Field<StockEntry>(f => f.Code, Operator.Eq, code));
            }

            var stockEntries = GetListSafe<StockEntry>(pg);

            return stockEntries;
        }
    }
}