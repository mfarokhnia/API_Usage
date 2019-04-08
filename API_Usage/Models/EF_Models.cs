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

    public class Financial
    {
        public int  FinancialId { get; set; }
        public float? grossprofit { get; set; }
        public float? totalrevenue { get; set; }
        public float? netincome { get; set; }
        public float? currentassets { get; set; }
        public float? totalliabilities { get; set; }
        public float? totalcash { get; set; }
        public float? totaldebt { get; set; }
        public float? cashflow { get; set; }
        public string symbol { get; set; }
    }
    public class Quote
    {
        public int QuoteId { get; set; }
        public string companysymbol { get; set; }
        public string companyname { get; set; }
        public string sector { get; set; }
        public float iexRealtimePrice { get; set; }
        public float iexRealtimeSize { get; set; }
        public float YTDchange { get; set; }
    }

  public class ChartRoot
  {
    public Equity[] chart { get; set; }
  }

  public class DailyChartRoot
   {
     public List <DailyEquity> dailychart { get; set; }
   }
    public class FinancialRoot
    {
        public List <Financial> financial { get; set; }
    }
    public class QuoteRoot
    {
        public List<Quote> quote { get; set; }
    }
}