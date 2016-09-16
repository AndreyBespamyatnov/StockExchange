"# StockExchange" 


1. Application uses MS Sql Server, you can read more and download here https://msdn.microsoft.com/en-us/sqlserver2014express.aspx
2. Please use DB_script.sql to create initial state of the database.
3. Decompress sources and open StockExchange.sln file with Visual Studio 2013.
4. Make sure to recover all the packages via NuGet Package Manager Console.
5. To run StockExchange Application solution should be configured to have 2 start-up projects - StockExchange and StockExchange.WebClient
6. Please configure connectionString for 'StockExchange' project in Web.config file for use your MSSQL connection string.
7. Please configure <system.serviceModel> -> <client> -> <endpoint> - 'address' property for 'StockExchange.WebClient' project in Web.config file for use your StockExchange.asmx service addres.
8. To my regret, no web installer packages are available.

