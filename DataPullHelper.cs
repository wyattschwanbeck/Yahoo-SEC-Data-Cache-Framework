using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using SecuritiesExchangeCommission.Edgar;
using Xbrl;
using Xbrl.FinancialStatement;
using EdgarCacheFramework.DataAccess;
using Yahoo.Finance;
using System.Collections.Generic;
using EdgarCacheFramework.Models;

namespace EdgarCacheFramework
{
    public class DataPullHelper
    {
       
        private HistoricalDataProvider yahooDl;
        public DataPullHelper(string UserName)
        {

            SecuritiesExchangeCommission.Edgar.SecRequestManager.Instance.UserAgent = "EdgarCacheFrmework-{0}/1.0.1";
            yahooDl = new HistoricalDataProvider();

    }
        /// <summary>
        /// Report type can be 10-Q or 10-K
        /// Queries local database to see if it already has been cached. Downloads data directly from SEC's EDGAR and caches the data otherwise.
        /// </summary>
        /// <param name="ticker" ></param>
        /// <param name="reportType"></param>
        /// <param name="yearsPrior"></param>
        /// <returns></returns>
        public async Task<FinancialStatement[]> GetFinancialStatements(string ticker, string reportType, int yearsPrior)
        {

            EdgarSearch edgarTask = await EdgarSearch.CreateAsync(ticker, reportType);
            int reportCount = reportType.StartsWith("10-Q") ? 4 * yearsPrior : yearsPrior;
            int AvailableYears = Math.Min(reportCount, edgarTask.Results.Length);

            FinancialStatement[] financialStatements = new FinancialStatement[AvailableYears];
            int blankRepAdj=0;
            for (int i = 0; i < Math.Min(reportCount+blankRepAdj, edgarTask.Results.Length); i++)
            {
                financialStatements[i-blankRepAdj] = DbHelper.GetFinancialStatement(ticker, reportType, edgarTask.Results[i].FilingDate);
                if (financialStatements[i-blankRepAdj] == null)
                {
                    XbrlInstanceDocument doc;
                    Stream s;
                    try
                    {
                        s = await edgarTask.Results[i].DownloadXbrlDocumentAsync();
                        StreamReader srCheck = new StreamReader(s);
                        string xml = srCheck.ReadToEnd();
                        if (xml != "")
                            doc = XbrlInstanceDocument.Create(s);
                        else
                            doc = null;
                        if (doc != null)
                        {
                            try
                            {
                                financialStatements[i - blankRepAdj] = doc.CreateFinancialStatement();
                                DbHelper.SaveDocument(financialStatements[i-blankRepAdj], ticker, edgarTask.Results[i].FilingDate.Date, xml);

                            } catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                blankRepAdj++;
                            }
                            
                        }
                        else
                        {
                            blankRepAdj++;
                        }
                    }
                    catch (TaskCanceledException e)
                    {
                        Console.WriteLine(e.Message);
                        s = await edgarTask.Results[i].DownloadXbrlDocumentAsync();
                        StreamReader srCheck = new StreamReader(s);
                        string xml = srCheck.ReadToEnd();
                        if (xml  != "")
                            doc = XbrlInstanceDocument.Create(s);
                        else
                            doc = null;

                        if (doc != null)
                        {
                            financialStatements[i-blankRepAdj] = doc.CreateFinancialStatement();
                            DbHelper.SaveDocument(financialStatements[i], ticker, edgarTask.Results[i].FilingDate.Date, xml);
                        } else
                        {
                            blankRepAdj++;
                        }
                    }
                }
            }
            //Edgar filings are pulled starting with newest, reverse so date is in ascending order
            return financialStatements.Reverse().ToArray();

        }

        public async Task<HistoricalDataRecord[]> GetStockPrice(string Ticker, DateTime StartDate, DateTime EndDate)
        {
            //Stock market is only Mon-Friday. Adjust to previous friday if Sunday or Saturday entered
            StartDate = StartDate.DayOfWeek==DayOfWeek.Sunday? StartDate.AddDays(-2): StartDate.DayOfWeek == DayOfWeek.Saturday? StartDate.AddDays(-1):StartDate;
            EndDate= EndDate.DayOfWeek == DayOfWeek.Sunday ? EndDate.AddDays(-2) : EndDate.DayOfWeek == DayOfWeek.Saturday ? EndDate.AddDays(-1) : EndDate;
            //pull historicaldataInstances to have access to the Downloaded time
            StockPriceInstance[] cachedStockPrices = DbHelper.GetStockData(Ticker, StartDate, EndDate);
            if(cachedStockPrices.Length >0)
            {
                //At least partially downloaded already

                StockPriceInstance oldestCached = cachedStockPrices[0];
                StockPriceInstance newestCached = cachedStockPrices.Last();
                DateTime oldestDate = DateTime.FromFileTimeUtc(oldestCached.Date);
                DateTime newestDate = DateTime.FromFileTimeUtc(newestCached.Date);
                DateTime cacheDate = DateTime.FromFileTimeUtc(oldestCached.DownloadDate);
                
                bool cacheResult = true;
                //If more than 3 months have passed, download the earliest date cached to ensure stock price hasn't been adjusted
                if (DateTime.Compare(cacheDate, DateTime.Today.AddDays(-90)) < 0)
                { 
                    await yahooDl.DownloadHistoricalDataAsync(Ticker, oldestDate, oldestDate.AddDays(1));
                   cacheResult = (int)oldestCached.AdjustedClose == (int)yahooDl.HistoricalData[0].AdjustedClose;
                }
                if(cacheResult)
                    //Cahced data already downloaded within 90 days or has the same adjusted close as provided from yahoo
                    if (oldestDate == StartDate && newestDate== EndDate)
                        //Cached data fulfilled the request, return
                        return DbHelper.ConvertFromInstances(cachedStockPrices);
                    else
                    {
                        List<HistoricalDataRecord> stockPriceResults = new List<HistoricalDataRecord>();
                        stockPriceResults.AddRange(DbHelper.ConvertFromInstances(cachedStockPrices));

                        //Values before start or after end cached data need downloaded
                        if (StartDate<oldestDate)
                        {
                            await yahooDl.DownloadHistoricalDataAsync(Ticker, StartDate, oldestDate.AddDays(-1));
                            if (yahooDl.HistoricalData != null)
                            {
                                HistoricalDataRecord[] preHistoricRecords = new HistoricalDataRecord[yahooDl.HistoricalData.Length];
                                yahooDl.HistoricalData.CopyTo(preHistoricRecords, 0);
                                stockPriceResults.InsertRange(0, preHistoricRecords);
                                DbHelper.SaveStockPrices(preHistoricRecords, Ticker);
                            }
                        }
                        if (EndDate.ToFileTimeUtc() > newestCached.Date)
                        {
                            //Date might not be ready for the end date yet. Skip pulling it until the next day
                            if (newestDate.AddDays(1) != EndDate)
                            {
                                await yahooDl.DownloadHistoricalDataAsync(Ticker, newestDate.AddDays(1), EndDate);
                                if (yahooDl.HistoricalData != null)
                                {
                                    HistoricalDataRecord[] postHistoricRecords = new HistoricalDataRecord[yahooDl.HistoricalData != null ? yahooDl.HistoricalData.Length : 0];
                                    yahooDl.HistoricalData.CopyTo(postHistoricRecords, 0);
                                    stockPriceResults.AddRange(postHistoricRecords);
                                    DbHelper.SaveStockPrices(postHistoricRecords, Ticker);
                                }
                            }
                        }
                        return stockPriceResults.ToArray();
                    }
                else
                {
                    //Records need to be cleared
                    DbHelper.ClearStockPrices(Ticker);
                    //No stock data cached, download all
                    await yahooDl.DownloadHistoricalDataAsync(Ticker, StartDate, EndDate);
                    HistoricalDataRecord[] historicalDataRecords = yahooDl.HistoricalData;
                    DbHelper.SaveStockPrices(historicalDataRecords, Ticker);
                    return historicalDataRecords;
                }
            }
            else
            {
                //No stock data cached, download all
                await yahooDl.DownloadHistoricalDataAsync(Ticker, StartDate, EndDate);
                HistoricalDataRecord[] historicalDataRecords = yahooDl.HistoricalData;
                DbHelper.SaveStockPrices(historicalDataRecords, Ticker);
                return historicalDataRecords;
            }
        }

      



    }

}
