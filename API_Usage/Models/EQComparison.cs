using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Usage.Models
{
    public class EQComparison
    {
        public List<Equity> FirstEQ { set; get; }
        public List<Equity> SecondEQ { set; get; }
        public List<DailyEquity> FirstDailyEQ { set; get; }
        public List<DailyEquity> SecondDailyEQ { set; get; }
        public List<Financial> FirstFinancial { set; get; }
        public List<Financial> SecondFinancial { set; get; }
        public List<Quote> FirstQuote { set; get; }
        public List<Quote> SecondQuote { set; get; }
    }
}
