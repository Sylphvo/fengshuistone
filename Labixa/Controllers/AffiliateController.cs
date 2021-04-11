using Labixa.Common;
using log4net;
using Newtonsoft.Json;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Labixa.Controllers
{
    [Authorize]
    public class AffiliateController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        private string DOMAIN_API = System.Configuration.ConfigurationManager.AppSettings["domain_api"];

        private readonly ISDKApiFundist _sdkApiFundist;
        private readonly ISDKApiAdmin _sdkApiAdmin;
        private IUserService _userServices;
        public AffiliateController(ISDKApiFundist sDKApiFundist, ISDKApiAdmin sDKApiAdmin, IUserService userServices)
        {
            this._userServices = userServices;
            this._sdkApiFundist = sDKApiFundist;
            this._sdkApiAdmin = sDKApiAdmin;
        }
        //
        // GET: /Affiliate/
        public ActionResult Index()
        {
            log.Info("hello");
            var listUser = _userServices.GetUsers();
            return View(listUser);
        }
        //[HttpPost]
        //public async System.Threading.Tasks.Task<string> transactionHistory(string type)
        //{
        //    var settings = new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //        Formatting = Formatting.Indented
        //    };
        //    try
        //    {
        //        if (type.Equals("deposit"))
        //        {
        //            var currency = await _sdkApiAdmin.HistoryCurrency(User.Identity.Name, "deposit", 1);
        //            var New = await _sdkApiAdmin.HistoryNew(User.Identity.Name, "deposit", 1);
        //            Console.WriteLine(currency);
        //            Console.WriteLine(New);
        //            List<HistoryCurrency> listHistoryCurrency = JsonConvert.DeserializeObject<List<HistoryCurrency>>(currency);
        //            List<HistoryNew> listHistoryNew = JsonConvert.DeserializeObject<List<HistoryNew>>(New);
        //            ModelHistory obj = new ModelHistory()
        //            {
        //                historycurrency = listHistoryCurrency,
        //                historynew = listHistoryNew
        //            };
        //            return JsonConvert.SerializeObject(obj, settings);
        //        }
        //        else if (type.Equals("withdraw"))
        //        {
        //            var currency = await _sdkApiAdmin.HistoryCurrency(User.Identity.Name, "withdraw", 1);
        //            var New = await _sdkApiAdmin.HistoryNew(User.Identity.Name, "withdraw", 1);
        //            Console.WriteLine(currency);
        //            Console.WriteLine(New);
        //            List<HistoryCurrency> listHistoryCurrency = JsonConvert.DeserializeObject<List<HistoryCurrency>>(currency);
        //            List<HistoryNew> listHistoryNew = JsonConvert.DeserializeObject<List<HistoryNew>>(New);
        //            ModelHistory obj = new ModelHistory()
        //            {
        //                historycurrency = listHistoryCurrency,
        //                historynew = listHistoryNew
        //            };
        //            return JsonConvert.SerializeObject(obj, settings);
        //        }
        //        else if(type.Equals("exchangeETH"))
        //        {
        //            var currency = await _sdkApiAdmin.HistoryCurrency(User.Identity.Name, "exchance", 1);
        //            var New = await _sdkApiAdmin.HistoryNew(User.Identity.Name, "exchance", 1);
        //            Console.WriteLine(currency);
        //            Console.WriteLine(New);
        //            List<HistoryCurrency> listHistoryCurrency = JsonConvert.DeserializeObject<List<HistoryCurrency>>(currency);
        //            List<HistoryNew> listHistoryNew = JsonConvert.DeserializeObject<List<HistoryNew>>(New);
        //            ModelHistory obj = new ModelHistory()
        //            {
        //                historycurrency = listHistoryCurrency,
        //                historynew = listHistoryNew
        //            };
        //            return JsonConvert.SerializeObject(obj, settings);
        //        }
        //        else if(type.Equals("exchangeVIP"))
        //        {
        //            var currency = await _sdkApiAdmin.HistoryCurrency(User.Identity.Name, "exchance", 2);
        //            var New = await _sdkApiAdmin.HistoryNew(User.Identity.Name, "exchance", 2);
        //            Console.WriteLine(currency);
        //            Console.WriteLine(New);
        //            List<HistoryCurrency> listHistoryCurrency = JsonConvert.DeserializeObject<List<HistoryCurrency>>(currency);
        //            List<HistoryNew> listHistoryNew = JsonConvert.DeserializeObject<List<HistoryNew>>(New);
        //            ModelHistory obj = new ModelHistory()
        //            {
        //                historycurrency = listHistoryCurrency,
        //                historynew = listHistoryNew
        //            };
        //            return JsonConvert.SerializeObject(obj, settings);
        //        }
        //        else if(type.Equals("depositVIP"))
        //        {
        //            var currency = await _sdkApiAdmin.HistoryCurrency(User.Identity.Name, "deposit", 2);
        //            var New = await _sdkApiAdmin.HistoryNew(User.Identity.Name, "deposit", 2);
        //            Console.WriteLine(currency);
        //            Console.WriteLine(New);
        //            List<HistoryCurrency> listHistoryCurrency = JsonConvert.DeserializeObject<List<HistoryCurrency>>(currency);
        //            List<HistoryNew> listHistoryNew = JsonConvert.DeserializeObject<List<HistoryNew>>(New);
        //            ModelHistory obj = new ModelHistory()
        //            {
        //                historycurrency = listHistoryCurrency,
        //                historynew = listHistoryNew
        //            };
        //            return JsonConvert.SerializeObject(obj, settings);
        //        }
        //        else if (type.Equals("withdrawVIP"))
        //        {
        //            var currency = await _sdkApiAdmin.HistoryCurrency(User.Identity.Name, "withdraw", 2);
        //            var New = await _sdkApiAdmin.HistoryNew(User.Identity.Name, "withdraw", 2);
        //            Console.WriteLine(currency);
        //            Console.WriteLine(New);
        //            List<HistoryCurrency> listHistoryCurrency = JsonConvert.DeserializeObject<List<HistoryCurrency>>(currency);
        //            List<HistoryNew> listHistoryNew = JsonConvert.DeserializeObject<List<HistoryNew>>(New);
        //            ModelHistory obj = new ModelHistory()
        //            {
        //                historycurrency = listHistoryCurrency,
        //                historynew = listHistoryNew
        //            };
        //            return JsonConvert.SerializeObject(obj, settings);
        //        }
        //        else if (type.Equals("commission"))
        //        {
        //            var currency = await _sdkApiAdmin.HistoryCurrency(User.Identity.Name, "commission", 0);
        //            var New = await _sdkApiAdmin.HistoryNew(User.Identity.Name, "commission", 0);
        //            Console.WriteLine(currency);
        //            Console.WriteLine(New);
        //            List<HistoryCurrency> listHistoryCurrency = JsonConvert.DeserializeObject<List<HistoryCurrency>>(currency);
        //            List<HistoryNew> listHistoryNew = JsonConvert.DeserializeObject<List<HistoryNew>>(New);
        //            ModelHistory obj = new ModelHistory()
        //            {
        //                historycurrency = listHistoryCurrency,
        //                historynew = listHistoryNew
        //            };
        //            return JsonConvert.SerializeObject(obj, settings);
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info(ex.Message);
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }
        //}
        [HttpPost]
        public async System.Threading.Tasks.Task<string> CalcTotalBet()
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            try
            {
                var result = await _sdkApiAdmin.GetTotalBet(DOMAIN_API+"api/Dashboard?", User.Identity.Name);
              //  var resultSystemBet = await _sdkApiAdmin.GetSytemBet("Http://api.avataclub.com/api/Affiliate?", User.Identity.Name);
                //IFirebaseConfig config = new FirebaseConfig
                //{
                //    AuthSecret = "KYAJfgHZE6AF9DvLmf9NJrjjeznaqsJ7topo7fOf",
                //    BasePath = "https://projectbets-9a7aa.firebaseio.com/"
                //};
                //IFirebaseClient client = new FirebaseClient(config);
                //var Url = "thanbailongtu4/" + DateTime.Now.Date.ToString("MM-dd-yyyy") + "/debit/";
                //FirebaseResponse response = await client.GetAsync(Url);
                ////List<Dictionary<string, debitModel>> objList = new List<Dictionary<string, debitModel>>();
                //var D = response.ResultAs<Dictionary<string, debitModel>>(); //The response will contain the data being retreived
                ////var totalBet;
                //foreach (var item in D.Values.ToList())
                //{

                //}
                Console.WriteLine(result);
                var bet = new BetModel();
                bet.TotalBet = result;
              //  bet.SystemBet = resultSystemBet;
                return JsonConvert.SerializeObject(bet, settings);
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                Console.WriteLine(ex.Message);
                return JsonConvert.SerializeObject(new BetModel(), settings);
            }
        }
    }
    public class debitModel
    {
        public debitModel()
        {
            amount = 0.00;
        }
        public string type { get; set; }
        public double amount { get; set; }
        public string tid { get; set; }
    }
    public class BetModel
    {
        public BetModel()
        {
            TotalBet = 0.00;
            SystemBet = 0.00;
        }
        public double TotalBet { get; set; }
        public double SystemBet { get; set; }
    }

    public class HistoryCurrency
    {
        public HistoryCurrency()
        {
            Time = DateTime.Now.Date;
        }
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public string Fee { get; set; }
        public string ExchandeRate { get; set; }
        public string Amount { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TxHash { get; set; }
        public string TxnHash { get; set; }
        public string Status { get; set; }
    }
    public class HistoryNew
    {
        public HistoryNew()
        {
            Time = DateTime.Now.Date;
        }
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public string Fee { get; set; }
        public string ExchandeRate { get; set; }
        public string Amount { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TxHash { get; set; }
        public string TxnHash { get; set; }
        public string Status { get; set; }
    }
    public class ModelHistory
    {
        public List<HistoryNew> historynew { get; set; }
        public List<HistoryCurrency> historycurrency { get; set; }
    }
}