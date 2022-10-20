using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yahoo.Finance;
//using System.ComponentModel.DataAnnotations;
using SQLite;

namespace EdgarCacheFramework.Models
{
    public class StockPriceInstance
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        [MaxLength(7)]
        public string Ticker { get; set; }
        public long Date { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public float AdjustedClose { get; set; }
        public int Volume { get; set; }

        public long DownloadDate { get; set; }
    }
}
