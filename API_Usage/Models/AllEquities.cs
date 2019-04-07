using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Usage.Models
{
    public class AllEquities
    {
        public List<Company> Companies { get; set; }
        public Equity yearlyequity { get; set; }
        public DailyEquity dailyequity { get; set; }

    }
}
