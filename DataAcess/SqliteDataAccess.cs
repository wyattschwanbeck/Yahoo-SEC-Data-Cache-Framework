
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgarCacheFramework.Models;
using Xbrl;
//using System.Data;
using Microsoft.EntityFrameworkCore.Sqlite;
using Xbrl.FinancialStatement;
using Yahoo.Finance;

namespace EdgarCacheFramework.DataAccess
{

    public static class DbHelper
    {
        public static FinancialStatement GetFinancialStatement(string ticker, string reportType, DateTime periodStart)
        {
            //FinancialStatement[] financialStatements = new FinancialStatement[yearsPrior];
            using (var context = new FinancialStatements())
            {
                //long MinFileDate = DateTime.Today.ToFileTimeUtc() - (DateTime.Today.AddYears(-yearsPrior)).ToFileTimeUtc();
                var query = from st in context.FinancialStatementsInstances
                            where st.PullTime == periodStart.ToFileTimeUtc() && st.Ticker == ticker orderby st.PullTime descending
                            select st;
                List<FinancialStatementInstance> financialStatementInstances = query.ToList();

                if(financialStatementInstances.Count > 0)
                {
                    FinancialStatement financialStatement = ConvertFromInstance(financialStatementInstances[0]);
                    //}
                    return financialStatement;
                }
                else
                {
                    return null;
                }
                    
                //List<FinancialStatements> result = financialStatementInstances;
                //TODO update edgar db package to use a long numeric type for datetimes, uninstall nuget package and point at locally created one
            }

        }
        public static void SaveDocument(FinancialStatement statement, string ticker, DateTime filingDate, string xml)
        {
            using (var context = new FinancialStatements())
            {
                context.Add(ConvertToInstance(statement, ticker, filingDate, xml));
                context.SaveChanges();
            }
        }

        public static StockPriceInstance[] GetStockData(string ticker, DateTime periodStart, DateTime periodEnd)
        {
            //FinancialStatement[] financialStatements = new FinancialStatement[yearsPrior];
            using (var context = new FinancialStatements())
            {
                //long MinFileDate = DateTime.Today.ToFileTimeUtc() - (DateTime.Today.AddYears(-yearsPrior)).ToFileTimeUtc();
                var query = from sp in context.StockPriceInstances
                            where sp.Date >= periodStart.ToFileTimeUtc() && 
                            sp.Date <= periodEnd.ToFileTimeUtc() &&
                            sp.Ticker == ticker
                            orderby sp.Date ascending
                            select sp;
                //List<StockPriceInstance> stockPriceInstances = query.ToList();
                //HistoricalDataRecord[] stockData = new HistoricalDataRecord[stockPriceInstances.Count];
                //for(int i=0; i< stockPriceInstances.Count; i++)
                //{
                //    stockData[i] = ConvertFromInstance(stockPriceInstances[i]);
                    //}
                    
                //}
                return query.ToArray();
                

                //List<FinancialStatements> result = financialStatementInstances;
                //TODO update edgar db package to use a long numeric type for datetimes, uninstall nuget package and point at locally created one
            }

        }
        public static void SaveStockPrices(HistoricalDataRecord[] historicalDataRecords, string ticker)
        {
            using (var context = new FinancialStatements())
            {
                for(int i=0; i< historicalDataRecords.Length; i++)
                    context.Add(ConvertToInstance(historicalDataRecords[i], ticker));
                context.SaveChanges();
            }
        }

        public static void ClearStockPrices(string Ticker)
        {
            using (var context = new FinancialStatements())
            {
                //long MinFileDate = DateTime.Today.ToFileTimeUtc() - (DateTime.Today.AddYears(-yearsPrior)).ToFileTimeUtc();
                var query = from sp in context.StockPriceInstances
                            where sp.Ticker == Ticker
                            select sp;
                context.RemoveRange(query);
                context.SaveChanges();
            }
            }

        private static FinancialStatement ConvertFromInstance(FinancialStatementInstance instance)
        {
            FinancialStatement financialStatement = new FinancialStatement();
            financialStatement.PeriodStart = DateTime.FromFileTimeUtc(instance.PeriodStart);
            financialStatement.PeriodEnd = DateTime.FromFileTimeUtc(instance.PeriodEnd);
            financialStatement.Assets = instance.Assets;
            financialStatement.CurrentAssets = instance.CurrentAssets;
            financialStatement.CurrentLiabilities = instance.CurrentLiabilities;
            financialStatement.Liabilities = instance.Liabilities;
            financialStatement.Revenue = instance.Revenue;
            financialStatement.SellingGeneralAndAdministrativeExpense= instance.SellingGeneralAndAdministrativeExpense;
            financialStatement.ResearchAndDevelopmentExpense= instance.ResearchAndDevelopmentExpense;
            financialStatement.OperatingIncome = instance.OperatingIncome;
            financialStatement.NetIncome = instance.NetIncome;
            financialStatement.Equity = instance.Equity;
            financialStatement.Cash = instance.Cash;
            financialStatement.RetainedEarnings = instance.RetainedEarnings;
            financialStatement.CommonStockSharesOutstanding = instance.CommonStockSharesOutstanding;
            financialStatement.OperatingCashFlows = instance.OperatingCashFlows;
            financialStatement.InvestingCashFlows = instance.InvestingCashFlows;
            financialStatement.FinancingCashFlows = instance.FinancingCashFlows;
            financialStatement.ProceedsFromIssuanceOfDebt = instance.ProceedsFromIssuanceOfDebt;
            financialStatement.PaymentsOfDebt = instance.PaymentsOfDebt;
            financialStatement.DividendsPaid = instance.DividendsPaid;
            
            return financialStatement;
            }
        private static FinancialStatementInstance ConvertToInstance(FinancialStatement instance, string Ticker, DateTime filingDate, string xml)
        {
            FinancialStatementInstance financialStatement = new FinancialStatementInstance();
            financialStatement.Ticker = Ticker;
            financialStatement.PullTime = filingDate.ToFileTimeUtc();
            financialStatement.PeriodStart = instance.PeriodStart==null? 0: instance.PeriodStart.Value.ToFileTimeUtc();
            financialStatement.PeriodEnd = instance.PeriodEnd== null ? 0 : instance.PeriodEnd.Value.ToFileTimeUtc();
            financialStatement.Assets = instance.Assets;
            financialStatement.CurrentAssets = instance.CurrentAssets;
            financialStatement.CurrentLiabilities = instance.CurrentLiabilities;
            financialStatement.Liabilities = instance.Liabilities;
            financialStatement.Revenue = instance.Revenue;
            financialStatement.SellingGeneralAndAdministrativeExpense = instance.SellingGeneralAndAdministrativeExpense;
            financialStatement.ResearchAndDevelopmentExpense = instance.ResearchAndDevelopmentExpense;
            financialStatement.OperatingIncome = instance.OperatingIncome;
            financialStatement.NetIncome = instance.NetIncome;
            financialStatement.Equity = instance.Equity;
            financialStatement.Cash = instance.Cash;
            financialStatement.RetainedEarnings = instance.RetainedEarnings;
            financialStatement.CommonStockSharesOutstanding = instance.CommonStockSharesOutstanding;
            financialStatement.OperatingCashFlows = instance.OperatingCashFlows;
            financialStatement.InvestingCashFlows = instance.InvestingCashFlows;
            financialStatement.FinancingCashFlows = instance.FinancingCashFlows;
            financialStatement.ProceedsFromIssuanceOfDebt = instance.ProceedsFromIssuanceOfDebt;
            financialStatement.PaymentsOfDebt = instance.PaymentsOfDebt;
            financialStatement.DividendsPaid = instance.DividendsPaid;
            financialStatement.xml = xml;
            return financialStatement;
        }
        public static HistoricalDataRecord ConvertFromInstance(StockPriceInstance instance)
        {
            HistoricalDataRecord hdr = new HistoricalDataRecord();
            hdr.Date= DateTime.FromFileTimeUtc(instance.Date);
            hdr.Open = instance.Open;
            hdr.Volume = instance.Volume;
            hdr.AdjustedClose = instance.AdjustedClose;
            hdr.Close = instance.Close;
            return hdr;
        }
        public static HistoricalDataRecord[] ConvertFromInstances(StockPriceInstance[] instances)
        {
            HistoricalDataRecord[] hdrs = new HistoricalDataRecord[instances.Length];
            for (int i = 0; i < instances.Length; i++) 
            {
                hdrs[i] = new HistoricalDataRecord();
                hdrs[i].Date = DateTime.FromFileTimeUtc(instances[i].Date);
                hdrs[i].Open = instances[i].Open;
                hdrs[i].Volume = instances[i].Volume;
                hdrs[i].AdjustedClose = instances[i].AdjustedClose;
                hdrs[i].Close = instances[i].Close; 
            }

            return hdrs;
        }
       
        public static StockPriceInstance ConvertToInstance(HistoricalDataRecord instance,string Ticker)
        {
            StockPriceInstance spi = new StockPriceInstance();
            spi.Ticker = Ticker;
            spi.Open = instance.Open;
            spi.Volume = instance.Volume;
            spi.AdjustedClose = instance.AdjustedClose;
            spi.Date = instance.Date.ToFileTimeUtc();
            spi.Close = instance.Close;
            spi.DownloadDate = DateTime.Now.ToFileTimeUtc();
            return spi;
        }

    }
}
