using Microsoft.AspNetCore.Mvc;
using API_Usage.DataAccess;
using API_Usage.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;


/*
 * Acknowledgments
 *  v1 of the project was created for the Fall 2018 class by Dhruv Dhiman, MS BAIS '18
 *    This example showed how to use v1 of the IEXTrading API
 *    
 *  Kartikay Bali (MS BAIS '19) extended the project for Spring 2019 by demonstrating 
 *    how to use similar methods to access Azure ML models
*/

namespace API_Usage.Controllers
{
    public class HomeController : Controller
    {

        public ApplicationDbContext dbContext;

        //Base URL for the IEXTrading API. Method specific URLs are appended to this base URL.
        string BASE_URL = "https://api.iextrading.com/1.0/";
        HttpClient httpClient;
        HttpClient httpClient2;

        /// <summary>
        /// Initialize the database connection and HttpClient object
        /// </summary>
        /// <param name="context"></param>
        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient2 = new HttpClient();
            httpClient2.DefaultRequestHeaders.Accept.Clear();
            httpClient2.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        /****
         * The Symbols action calls the GetSymbols method that returns a list of Companies.
         * This list of Companies is passed to the Symbols View.
        ****/
        public IActionResult Symbols()
        {
            //Set ViewBag variable first
            ViewBag.dbSucessComp = 0;
            List<Company> companies = GetSymbols();

            //Save companies in TempData, so they do not have to be retrieved again
            TempData["Companies"] = JsonConvert.SerializeObject(companies);
            //TempData["Companies"] = companies;

            return View(companies);
        }

        public IActionResult ComparisonByEquity(string symbol1, string symbol2) //it allows users to compare two selected symbols based on their yearly equity 
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessChart = 0;

            List<Equity> equity1 = new List<Equity>();
            List<Equity> equity2 = new List<Equity>();

            if (symbol1 != null && symbol2 != null)
            {
                equity1 = GetChart(symbol1); //it calls getchart method which returns the annual equity report
                equity2 = GetChart2(symbol2);

                equity1 = equity1.OrderByDescending(c => c.date).ToList(); //Make sure the data is in ascending order of date.
                equity2 = equity2.OrderByDescending(c => c.date).ToList();
            }
            var VM = new EQComparison(); // to return to list through the view to the related view
            VM.FirstEQ = equity1;
            VM.SecondEQ = equity2;

            return View(VM);
        }

        public IActionResult ComparisonByDailyEquity(string symbol1, string symbol2) //it allows users to compare two selected symbols based on their daily equity
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessChart = 0;

            List<DailyEquity> dailyequity1 = new List<DailyEquity>();
            List<DailyEquity> dailyequity2 = new List<DailyEquity>();


            if (symbol1 != null && symbol2 != null)
            {
                dailyequity1 = GetDailyChart(symbol1);  //it calls getdailychart method which returns the daily equity report
                dailyequity2 = GetDailyChart2(symbol2);

                dailyequity1 = dailyequity1.OrderByDescending(c => c.minute).ToList(); //Make sure the data is in ascending order of date.
                dailyequity2 = dailyequity2.OrderByDescending(c => c.minute).ToList();
            }
            var DailyVM = new EQComparison(); // to return to list through the view to the related view
            DailyVM.FirstDailyEQ = dailyequity1;
            DailyVM.SecondDailyEQ = dailyequity2;

            return View(DailyVM);
        }



        /****
         * The Chart action calls the GetChart method that returns 1 year's equities for the passed symbol.
         * A ViewModel CompaniesEquities containing the list of companies, prices, volumes, avg price and volume.
         * This ViewModel is passed to the Chart view.
        ****/
        /// <summary>
        /// The Chart action calls the GetChart method that returns 1 year's equities for the passed symbol.
        /// A ViewModel CompaniesEquities containing the list of companies, prices, volumes, avg price and volume.
        /// This ViewModel is passed to the Chart view.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IActionResult Chart(string symbol)
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessChart = 0;
            List<Equity> equities = new List<Equity>();

            if (symbol != null)
            {
                equities = GetChart(symbol);
                equities = equities.OrderBy(c => c.date).ToList(); //Make sure the data is in ascending order of date.
            }

            CompaniesEquities companiesEquities = getCompaniesEquitiesModel(equities);

            return View(companiesEquities);
        }

        public IActionResult Trade(string symbol1) //it allows users to compare two selected symbols based on their daily equity
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessChart = 0;

            List<LargestTrade> largesttrade1 = new List<LargestTrade>();
           //List<LargestTrade> largesttrade2 = new List<LargestTrade>();


            if (symbol1 != null )
            {
                largesttrade1 = GetTrade(symbol1);  //it calls getdailychart method which returns the daily equity report
                //dailyequity2 = GetDailyChart2(symbol2);

                largesttrade1 = largesttrade1.OrderByDescending(c => c.time).ToList(); //Make sure the data is in ascending order of date.
               // dailyequity2 = dailyequity2.OrderByDescending(c => c.minute).ToList();
            }
            var DailyTR = new EQComparison(); // to return to list through the view to the related view
            DailyTR.FirstTrade = largesttrade1;
            //DailyTR.SecondDailyEQ = dailyequity2;

            return View(DailyTR);
        }

        public IActionResult Volume(string symbol1) //it allows users to compare two selected symbols based on their daily equity
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessChart = 0;

            List<VolumeByVenue> DailyVolume1 = new List<VolumeByVenue>();
            //List<LargestTrade> largesttrade2 = new List<LargestTrade>();


            if (symbol1 != null)
            {
                DailyVolume1 = GetVolume(symbol1);  //it calls getdailychart method which returns the daily equity report
                //dailyequity2 = GetDailyChart2(symbol2);

                DailyVolume1 = DailyVolume1.OrderByDescending(c => c.Date).ToList(); //Make sure the data is in ascending order of date.
                                                                                       // dailyequity2 = dailyequity2.OrderByDescending(c => c.minute).ToList();
            }
            var DailyVL = new EQComparison(); // to return to list through the view to the related view
            DailyVL.FirstVolume = DailyVolume1;
            //DailyTR.SecondDailyEQ = dailyequity2;

            return View(DailyVL);
        }



        //public IActionResult Trades()
        //{
        //    //Set ViewBag variable first
        //    ViewBag.dbSucessComp = 0;
        //    List<LargestTrade> tra = GetTrade();

        //    //Save companies in TempData, so they do not have to be retrieved again
        //    TempData["Trades"] = JsonConvert.SerializeObject(tra);
        //    //TempData["Companies"] = companies;

        //    return View(tra);
        //}
        /// <summary>
        /// Calls the IEX reference API to get the list of symbols
        /// </summary>
        /// <returns>A list of the companies whose information is available</returns>
        public List<Company> GetSymbols()
        {
            string IEXTrading_API_PATH = BASE_URL + "ref-data/symbols";
            string companyList = "";
            List<Company> companies = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                companyList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!companyList.Equals(""))
            {
                // https://stackoverflow.com/a/46280739
                //JObject result = JsonConvert.DeserializeObject<JObject>(companyList);
                companies = JsonConvert.DeserializeObject<List<Company>>(companyList);
                companies = companies.GetRange(0,50);
            }

            return companies;
        }

        public List<VolumeByVenue> GetVolume(string symbol1)  //this action method returns list of financial propperties of the financial API end point
        {
            // string to specify information to be retrieved from the API
            string IEXTrading_API_PATH = BASE_URL + "stock/" + symbol1 + "/batch?types=delayed-quote";

            string Vloume = "";
            List<VolumeByVenue> Ivolume = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                Vloume = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!Vloume.Equals(""))
            {
                RootObject result = JsonConvert.DeserializeObject<RootObject>(Vloume);
            }

            return Ivolume;
        }

        /// <summary>
        /// Calls the IEX stock API to get 1 year's chart for the supplied symbol
        /// </summary>
        /// <param name="symbol">Stock symbol of the company whose quotes are to be retrieved</param>
        /// <returns></returns>
        public List<Equity> GetChart(string symbol)
        {
            // string to specify information to be retrieved from the API
            string IEXTrading_API_PATH = BASE_URL + "stock/" + symbol + "/batch?types=chart&range=3m";

            // initialize objects needed to gather data
            string charts = "";
            List<Equity> Equities = new List<Equity>();
            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);

            // connect to the API and obtain the response
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // now, obtain the Json objects in the response as a string
            if (response.IsSuccessStatusCode)
            {
                charts = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // parse the string into appropriate objects
            if (!charts.Equals(""))
            {
                ChartRoot root = JsonConvert.DeserializeObject<ChartRoot>(charts,
                  new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Equities = root.chart.ToList();
            }

            // fix the relations. By default the quotes do not have the company symbol
            //  this symbol serves as the foreign key in the database and connects the quote to the company
            foreach (Equity Equity in Equities)
            {
                Equity.symbol = symbol;
            }

            return Equities;
        }

        public List<Equity> GetChart2(string symbol) //this method is used to handle two simultaneous requests for a same API End Point
        {
            // string to specify information to be retrieved from the API
            string IEXTrading_API_PATH2 = BASE_URL + "stock/" + symbol + "/batch?types=chart&range=3m";

            // initialize objects needed to gather data
            string charts = "";
            List<Equity> Equities = new List<Equity>();
            httpClient2.BaseAddress = new Uri(IEXTrading_API_PATH2);

            // connect to the API and obtain the response
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH2).GetAwaiter().GetResult();

            // now, obtain the Json objects in the response as a string
            if (response.IsSuccessStatusCode)
            {
                charts = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                //charts.GetType();

            }

            // parse the string into appropriate objects
            if (!charts.Equals(""))
            {
                ChartRoot root = JsonConvert.DeserializeObject<ChartRoot>(charts,
                  new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Equities = root.chart.ToList();
            }

            // fix the relations. By default the quotes do not have the company symbol
            //  this symbol serves as the foreign key in the database and connects the quote to the company
            foreach (Equity Equity in Equities)
            {
                Equity.symbol = symbol;
            }

            return Equities;
        }


        public List<DailyEquity> GetDailyChart(string symbol)  //this method returns the list of daily report using the specified API end point
        {
            // string to specify information to be retrieved from the API
            string IEXTrading_API_PATH = BASE_URL + "stock/" + symbol + "/chart/1d";

            // initialize objects needed to gather data
            string Dailycharts = "";
            List<DailyEquity> DailyEquities = new List<DailyEquity>();
            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);

            // connect to the API and obtain the response
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // now, obtain the Json objects in the response as a string
            if (response.IsSuccessStatusCode)
            {
                Dailycharts = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var aa = Dailycharts.GetType();
            }

                if (!Dailycharts.Equals(""))
                {

                 DailyEquity[] dailyEquities = JsonConvert.DeserializeObject<DailyEquity[]>(Dailycharts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                DailyEquities = dailyEquities.ToList();

            }



            // fix the relations. By default the quotes do not have the company symbol
            //  this symbol serves as the foreign key in the database and connects the quote to the company
            foreach (DailyEquity Equity in DailyEquities)
            {
                Equity.symbol = symbol;
            }

            return DailyEquities;
        }

        public List<DailyEquity> GetDailyChart2(string symbol2) //This Action Method is used for handling two simultaneous requests of the same API end Point
        {
            // string to specify information to be retrieved from the API
            string IEXTrading_API_PATH2 = BASE_URL + "stock/" + symbol2 + "/chart/1d";

            // initialize objects needed to gather data
            string Dailycharts = "";
            List<DailyEquity> DailyEquities = new List<DailyEquity>();
            httpClient2.BaseAddress = new Uri(IEXTrading_API_PATH2);

            // connect to the API and obtain the response
            HttpResponseMessage response = httpClient2.GetAsync(IEXTrading_API_PATH2).GetAwaiter().GetResult();

            // now, obtain the Json objects in the response as a string
            if (response.IsSuccessStatusCode)
            {
                Dailycharts = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var aa = Dailycharts.GetType();
            }

            // parse the string into appropriate objects
            if (!Dailycharts.Equals(""))
            {

                DailyEquity[] dailyEquities = JsonConvert.DeserializeObject<DailyEquity[]>(Dailycharts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                DailyEquities = dailyEquities.ToList();

            }

            // fix the relations. By default the quotes do not have the company symbol
            //  this symbol serves as the foreign key in the database and connects the quote to the company
            foreach (DailyEquity Equity in DailyEquities)
            {
                Equity.symbol = symbol2;
            }

            return DailyEquities;
        }

        
        public List<LargestTrade> GetTrade(string symbol1)  //this action method returns list of financial propperties of the financial API end point
        {
                // string to specify information to be retrieved from the API
                string IEXTrading_API_PATH = BASE_URL + "stock/" + symbol1 + "/largest-trades";

                // initialize objects needed to gather data
                string FixTrade = "";
                List<LargestTrade> Tradings = new List<LargestTrade>();
                httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);

                // connect to the API and obtain the response
                HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

                // now, obtain the Json objects in the response as a string
                if (response.IsSuccessStatusCode)
                {
                    FixTrade = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var aa = FixTrade.GetType();
                }

                // parse the string into appropriate objects
                if (!FixTrade.Equals(""))
                {
                LargestTradeRoot LargestTradeRoot = JsonConvert.DeserializeObject<LargestTradeRoot>(FixTrade,
                     new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //     Financial[] dailyfinancials = JsonConvert.DeserializeObject<Financial[]>(FixFinancial, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                Tradings = LargestTradeRoot.Trade.ToList();
                }

    

                return Tradings;
            
        }









        /// <summary>
        /// Call the ClearTables method to delete records from a table or all tables.
        ///  Count of current records for each table is passed to the Refresh View
        /// </summary>
        /// <param name="tableToDel">Table to clear</param>
        /// <returns>Refresh view</returns>
        public IActionResult Refresh(string tableToDel)
        {
            ClearTables(tableToDel);
            Dictionary<string, int> tableCount = new Dictionary<string, int>();
            tableCount.Add("Companies", dbContext.Companies.Count());
            tableCount.Add("Charts", dbContext.Equities.Count());
            return View(tableCount);
        }

        /// <summary>
        /// save the quotes (equities) in the database
        /// </summary>
        /// <param name="symbol">Company whose quotes are to be saved</param>
        /// <returns>Chart view for the company</returns>
        public IActionResult SaveCharts(string symbol)
        {
            List<Equity> equities = GetChart(symbol);

            // save the quote if the quote has not already been saved in the database
            foreach (Equity equity in equities)
            {
                if (dbContext.Equities.Where(c => c.date.Equals(equity.date)).Count() == 0)
                {
                    dbContext.Equities.Add(equity);
                }
            }

            // persist the data
            dbContext.SaveChanges();

            // populate the models to render in the view
            ViewBag.dbSuccessChart = 1;
            CompaniesEquities companiesEquities = getCompaniesEquitiesModel(equities);
            return View("Chart", companiesEquities);
        }

        /// <summary>
        /// Use the data provided to assemble the ViewModel
        /// </summary>
        /// <param name="equities">Quotes to dsiplay</param>
        /// <returns>The view model to include </returns>
        public CompaniesEquities getCompaniesEquitiesModel(List<Equity> equities)
        {
            List<Company> companies = dbContext.Companies.ToList();

            if (equities.Count == 0)
            {
                return new CompaniesEquities(companies, null, "", "", "", 0, 0);
            }

            Equity current = equities.Last();

            // create appropriately formatted strings for use by chart.js
            string dates = string.Join(",", equities.Select(e => e.date));
            string prices = string.Join(",", equities.Select(e => e.high));
            float avgprice = equities.Average(e => e.high);

            //Divide volumes by million to scale appropriately
            string volumes = string.Join(",", equities.Select(e => e.volume / 1000000));
            double avgvol = equities.Average(e => e.volume) / 1000000;

            return new CompaniesEquities(companies, equities.Last(), dates, prices, volumes, avgprice, avgvol);
        }

        /// <summary>
        /// Save the available symbols in the database
        /// </summary>
        /// <returns></returns>
        public IActionResult PopulateSymbols()
        {
            // retrieve the companies that were saved in the symbols method
            // saving in TempData is extremely inefficient - the data circles back from the browser
            // better methods would be to serialize to the hard disk, or save directly into the database
            //  in the symbols method. This example has been structured to demonstrate one way to save object data
            //  and retrieve it later
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(TempData["Companies"].ToString());

            foreach (Company company in companies)
            {
                //Database will give PK constraint violation error when trying to insert record with existing PK.
                //So add company only if it doesnt exist, check existence using symbol (PK)
                if (dbContext.Companies.Where(c => c.symbol.Equals(company.symbol)).Count() == 0)
                {
                    dbContext.Companies.Add(company);
                }
            }

            dbContext.SaveChanges();
            ViewBag.dbSuccessComp = 1;
            return View("Symbols", companies);
        }

        /// <summary>
        /// Delete all records from tables
        /// </summary>
        /// <param name="tableToDel">Table to clear</param>
        public void ClearTables(string tableToDel)
        {
            if ("all".Equals(tableToDel))
            {
                //First remove equities and then the companies
                dbContext.Equities.RemoveRange(dbContext.Equities);
                dbContext.Companies.RemoveRange(dbContext.Companies);
            }
            else if ("Companies".Equals(tableToDel))
            {
                //Remove only those companies that don't have related quotes stored in the Equities table
                dbContext.Companies.RemoveRange(dbContext.Companies
                                                         .Where(c => c.Equities.Count == 0)
                                                                      );
            }
            else if ("Charts".Equals(tableToDel))
            {
                dbContext.Equities.RemoveRange(dbContext.Equities);
            }
            dbContext.SaveChanges();
        }
    }
}