using System.Configuration;
using StockExchange.Extensions;

namespace StockExchange
{
    public static class ConfigurationProvider
    {
        private const string ConnectionStringKey = Constants.ConnectionStringKey;

        public static string GetConnectionString(string v, string defaultVal)
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[v];
            return (null != connectionString) ? connectionString.ConnectionString : defaultVal;
        }

        public static string ConnectionString
        {
            get
            {
                string configValueFromConnectionString = GetConnectionString(ConnectionStringKey, string.Empty);

                string.IsNullOrWhiteSpace(configValueFromConnectionString).BreakOnTrue("Connection string is not set");

                return configValueFromConnectionString;
            }
        }
    }
}