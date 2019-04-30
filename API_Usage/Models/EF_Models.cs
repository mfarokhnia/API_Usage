using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API_Usage.Models
{
  public class Company
  {
    [Key]
    public string symbol { get; set; }
    public string name { get; set; }
    public string date { get; set; }
    public bool isEnabled { get; set; }
    public string type { get; set; }
    public string iexId { get; set; }
    public List<Equity> Equities { get; set; }
    public string industry { get; set; }
    public string Sector { get; set; }
    }

  public class Equity
  {
    public int EquityId { get; set; }
    public string date { get; set; }
    public float open { get; set; }
    public float high { get; set; }
    public float low { get; set; }
    public float close { get; set; }
    public int volume { get; set; }
    public int unadjustedVolume { get; set; }
    public float change { get; set; }
    public float changePercent { get; set; }
    public float vwap { get; set; }
    public string label { get; set; }
    public float changeOverTime { get; set; }
    public string symbol { get; set; }
  }

    public class DailyEquity
    {
        public int DailyEquityId { get; set; }
        public string minute { get; set; }
        public float? marketaverage { get; set; }
        public float? marketnotional { get; set; }
        public float? marketnumberoftrades { get; set; }
        public float? marketopen { get; set; }
        public float? marketclose { get; set; }
        public float? markethigh { get; set; }
        public float? marketlow { get; set; }
        public float? marketvolume { get; set; }
        public float? average { get; set; }
        public string symbol { get; set; }
    }

    public class Dividend

    {

        public int DividendId { get; set; }

        public string exDate { get; set; }

        public string paymentDate { get; set; }

        public string recordDate { get; set; }

        public string declaredDate { get; set; }

        public decimal amount { get; set; }

        public string flag { get; set; }

        public string type { get; set; }

        public string qualified { get; set; }

        public string indicated { get; set; }

        public string symbol { get; set; }

        public virtual Company Company { get; set; }

    }
    public class ChartRoot
  {
    public Equity[] chart { get; set; }
  }

  public class DailyChartRoot
   {
     public List <DailyEquity> dailychart { get; set; }
   }

    public class DividendVM

    {

        public List<Company> Companies { get; set; }

        public Dividend Current { get; set; }

        public DividendVM(List<Company> companies, Dividend current)

        {

            Companies = companies;

            Current = current;

        }

    }

}