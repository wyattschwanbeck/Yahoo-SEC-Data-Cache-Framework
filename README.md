# Yahoo-SEC-Data-Cache-Framework

This project minimizes external requests to Yahoo and SEC EDGAR XBRL filings by caching searched results in a code first, run-time generated SQLite database. 

Stock prices are validated after 90 days from caching in case of stock splits. SEC EDGAR XBRL filings over periods are cross referenced with the latest filings in case of adjustments (10-KA & 10-QA).


## Usage

    DataPullHelper pullHelper = new DataPullHelper();
    //Retreive financial statements
    FinancialStatement[] statements = await pullHelper.GetFinancialStatements("MSFT", "10-Q", 2);
    //Retreive stockprices
    HistoricalDataRecord[] stockPrices = await pullHelper.GetStockPrice("MSFT", DateTime.Parse("1/1/2020"), DateTime.Today);
    

## Dependencies

 - Xbrl
  www.nuget.org/packages/Xbrl/
 - SecuritiesExchangeCommission.Edgar
 https://www.nuget.org/package/SecuritiesExchangeCommission.Edgar
 - Yahoo.Finance
 https://www.nuget.org/packages/Yahoo.Finance 
 - SQLite
 https://github.com/praeclarum/sqlite-net
  
 - Microsoft.EntityFrameworkCore - Design, Sqlite, Tools
   https://learn.microsoft.com/en-us/ef/core/miscellaneous/platforms
