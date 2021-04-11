using Labixa.Common;
using Labixa.Models;
using log4net;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace Labixa.Controllers
{

    public class HomeController : Controller
    {
        ILog Log = log4net.LogManager.GetLogger(typeof(HomeController));
        private UserManager<User> _userManager;
        private readonly ISDKApiFundist _sdkApiFundist;
        private readonly ISDKApiAdmin _sdkApiAdmin;
        private readonly IConfirmEmail _confirmEmail;
        private readonly IUserService _userService;
        private readonly IWebsiteAttributeService _websiteAttributeService;
        private readonly IValidateCapcha _captcha;
        private string SERVER_IP = System.Configuration.ConfigurationManager.AppSettings["server_ip"];
        private string DOMAIN_API = System.Configuration.ConfigurationManager.AppSettings["domain_api"];
        private string DOMAIN_SERVER = System.Configuration.ConfigurationManager.AppSettings["domain"];

        public HomeController(ISDKApiFundist sDKApiFundist, ISDKApiAdmin sDKApiAdmin, UserManager<User> userManager, IConfirmEmail confirmEmail, IUserService userSerice, IWebsiteAttributeService websiteAttributeService, IValidateCapcha captcha)
        {
            _websiteAttributeService = websiteAttributeService;
            _userManager = userManager;
            this._sdkApiFundist = sDKApiFundist;
            this._sdkApiAdmin = sDKApiAdmin;
            _confirmEmail = confirmEmail;
            _userService = userSerice;
            _captcha = captcha;
        }
        #region Home
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            Request.Cookies.Remove("mimosa");//QRIMage
            Request.Cookies.Remove("mamosi");//QRText
            Log.Debug("Debug message");
            var aat = CommonCalculate.GetTimeStamp();
            var obj = await _sdkApiFundist.ApiGetFullList(CommonCalculate.GetTimeStamp());
            try
            {
                JObject json = JObject.Parse(obj);
                var aa = json["games"];
                //JObject jsonaa = JObject.Parse(aa);
                var res = JsonConvert.SerializeObject(aa);
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(obj);
                // var bbb = JsonConvert.DeserializeObject<List<RootGame>>(res);
                return View(myDeserializedClass);
            }
            catch (Exception ex)
            {
                Log.Info("Error exception action index function load list game fundist _ " + DateTime.Now + ": " + ex);
                return RedirectToAction("Index", "ErrorMessage");
            }
        }
        #endregion
        public async System.Threading.Tasks.Task<ActionResult> CreateAccount()
        {
            try
            {
                var _user = _userService.GetUsers().Where(p => p.UserName != "bigplayerclub" && p.UserName != "master").ToList();
                var ip = IPClient.getIP();//chỉ dùng cho web golive
                bool flag = false;
                string str = "";
                int count = 1;
                if (ip.Equals("::1"))
                {
                    ip = SERVER_IP;
                }
                //var callFundist = await _sdkApiFundist.ApiCreateUser("thuyling", "281198", CommonCalculate.GetTimeStamp(), ip, "thuyling");

                foreach (var item in _user)
                {
                    var callFundist = await _sdkApiFundist.ApiCreateUser(item.UserName.ToLower(), item.PasswordNotHash, CommonCalculate.GetTimeStamp(), ip, item.UserName.ToLower());
                    System.Threading.Thread.Sleep(1500);
                    if (callFundist.Equals("1"))
                    {
                        flag = true;
                        count++;
                    }
                }
                if (flag)
                {
                    str = count + "_Success all create user Fundist";
                }
                else
                {
                    str = count + "_Create failed user Fundist";
                }
                return View(model: count + "_" + str);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> ListWareGame()
        {
            Log.Debug("Debug message");
            var aat = CommonCalculate.GetTimeStamp();
            var obj = await _sdkApiFundist.ApiGetFullList(CommonCalculate.GetTimeStamp());

            JObject json = JObject.Parse(obj);
            var aa = json["games"].ToString();
            JObject jsonaa = JObject.Parse(aa);
            //var res = JsonConvert.DeserializeObject<List<GameFullList>>(aa);
            return Json(jsonaa);
        }
        public ActionResult About()
        {
            try
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }
            catch (Exception ex)
            {
                Log.Info("Error exception action about _" + DateTime.Now + ": " + ex);
                return RedirectToAction("Index", "ErrorMessage");
            }
        }
        #region Support Customer
        public ActionResult Contact(string str)
        {
            //CaptchaResponse response = _captcha.ValidateCaptcha(Request["g-recaptcha-response"]);
            try
            {

                ViewBag.Message = "Thank you,";
                if (str != null)
                {
                    ViewBag.ScriptsMessage = "<script>alert('" + str + "')</script>";
                }

                return View();
            }
            catch (Exception ex)
            {
                Log.Info("Error exception action Contact _" + DateTime.Now + ": " + ex);
                return RedirectToAction("Index", "ErrorMessage");
            }

        }
        [HttpPost]
        public ActionResult Contactrespon(string username, string email, string message, string str)
        {
            WebsiteAttribute websiteAttribute = new WebsiteAttribute()
            {
                Name = User.Identity.Name,//UserName
                Description = email,//UserName
                Noted_1 = message,//UserName
                Noted_2 = DateTime.Now.ToString("dd/MM/yyyy/ HH:mm:ss"),
                //Noted_3 = InputFileName,
                IsPublic = true,
                Deleted = false
            };
            _websiteAttributeService.CreateWebsiteAttribute(websiteAttribute);
            ViewBag.Message = "Thank you for your comments, we will assist you as quickly as possible";
            return RedirectToAction("Contact", "Home", new { str = ViewBag.Message });
        }
        //[HttpPost]
        //public ActionResult Contactrespon( string email, string message, string str, HttpPostedFileBase[] files)
        //{
        //    //Ensure model state is valid  
        //    if (ModelState.IsValid)
        //    {   //iterating through multiple file collection   
        //        foreach (HttpPostedFileBase file in files)
        //        {
        //            //Checking file is available to save.  
        //            if (file != null)
        //            {
        //                var InputFileName = Path.GetFileName(file.FileName);
        //                var ServerSavePath = Path.Combine(Server.MapPath("~/SaveImage/") + InputFileName);
        //                //Save file to server folder  
        //                file.SaveAs(ServerSavePath);
        //                WebsiteAttribute websiteAttribute = new WebsiteAttribute()
        //                {
        //                    Name = User.Identity.Name,//UserName
        //                    Description = email,//UserName
        //                    Noted_1 = message,//UserName
        //                    Noted_2 = DateTime.Now.ToString(),
        //                    Noted_3 = InputFileName,
        //                    IsPublic = true,
        //                    Deleted = false
        //                };
        //                _websiteAttributeService.CreateWebsiteAttribute(websiteAttribute);
        //                ViewBag.Message = "Send Support success!";
        //                //assigning file uploaded status to ViewBag for showing message to user.  
        //                ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
        //            }
        //        }
        //    }
        //    return RedirectToAction("Contact", "Home", new { str = ViewBag.Message });
        //}
        #endregion
        public ActionResult AboutUs()
        {
            return View();
        }
        #region List Game
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> WareGame()
        {
            Log.Debug("Debug message");
            var obj = await _sdkApiFundist.ApiGetFullList(CommonCalculate.GetTimeStamp());
            try
            {
                JObject json = JObject.Parse(obj);
                var aa = json["games"];
                //JObject jsonaa = JObject.Parse(aa);
                var res = JsonConvert.SerializeObject(aa);
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(obj);
                // var bbb = JsonConvert.DeserializeObject<List<RootGame>>(res);
                return View(myDeserializedClass);
            }
            catch (Exception ex)
            {
                Log.Info("Error exception action Waregame function call list game _" + DateTime.Now + ": " + ex);
                return RedirectToAction("Index", "ErrorMessage");
            }

            //return View(new List<GameFullList>());
        }
        #endregion
        #region Frame Play Game
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> GameDetail(string ID, string System)
        {
            //return RedirectToAction("AccountDashboard","Home");
            ViewBag.scriptsViewPort = "<meta name='viewport' content='width=device-width, initial-scale=0.5'>";
            try
            {
                var ip = IPClient.getIP();//chỉ dùng cho web golive
                                          //if (ip == "::1")
                                          //{
                                          //    ip = "115.73.214.11";
                                          //}

                //if (ip.Equals("115.73.214.11"))
                //{

                //}
                //else
                //{
                //    return RedirectToAction("Index", "ErrorMessage");
                //}
                var userId = User.Identity.GetUserId();
                var _user = _userManager.FindById(userId);
                var result = await _sdkApiFundist.ApiDirect_AuthorizationAsync(User.Identity.Name, CommonCalculate.GetTimeStamp(), System, _user.PasswordNotHash, ID, ip);
                ViewBag.DirectUrl = result.Split(',')[1];
                var result2 = _sdkApiFundist.ApiAuthorization_with_HTML(User.Identity.Name, CommonCalculate.GetTimeStamp(), _user.PasswordNotHash, ID, ip, System);
                if (result.Split(',')[0].Equals("1"))
                {
                    ViewBag.DirectUrl = result.Split(',')[1];
                }
                else if (result2.Split(',')[0].Equals("1"))
                {
                    ViewBag.HTMLUrl = result2.Split(',')[1].ToString();
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "ErrorMessage");
            }

        }
        #endregion
        public ActionResult BLog()
        {
            return View();
        }
        public ActionResult BLogDetail()
        {
            return View();
        }
        public ActionResult Promotion()
        {
            return View();
        }
        public ActionResult AffiliateProgramming()
        {
            return View();
        }
        public ActionResult FAQ()
        {
            return View();
        }
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> Affilites()
        {
            try
            {
                var Result_JsonAffiliate = await _sdkApiAdmin.SDKApiAffiliateGet(DOMAIN_API + "api/affiliate?username=", User.Identity.Name);
                List<ObjTransaction> modelParseTransaction = new List<ObjTransaction>();
                ViewBag.linkURL = DOMAIN_SERVER + "register-account/" + User.Identity.Name;
                return View("Affilites", model: Result_JsonAffiliate);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "ErrorMessage");
            }

        }
        [Authorize]
        public ActionResult AccountDashboard()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var _user = _userManager.FindById(userId);
                //ViewBag.findUserParent = _userService.GetUsers().Where(p => p.Id == _user.ParentId).FirstOrDefault();
                ViewBag.timeStamp = CommonCalculate.GetTimeStamp();
                return View(_user);
            }
            catch (Exception ex)
            {
                Log.Info("Error exception action account dashboard _" + DateTime.Now + ": " + ex);
                return RedirectToAction("Index", "ErrorMessage");
            }

        }
        public async System.Threading.Tasks.Task<ActionResult> KillALL()
        {
            var _user = _userService.GetUsers().ToList();
            foreach (var item in _user)
            {
                var result_kill = await _sdkApiFundist.ApiKill_Authorization(item.UserName, CommonCalculate.GetTimeStamp());
                Thread.Sleep(1500);
            }
            return View();
        }
        public async System.Threading.Tasks.Task<ActionResult> History()
        {
            #region call api history deposit
            var currencydepositeth = await _sdkApiAdmin.History_Deposit_Currency_ETH(User.Identity.Name);
            var currencydepositbit = await _sdkApiAdmin.History_Deposit_Currency_BIT(User.Identity.Name);
            var Newdepositeth = await _sdkApiAdmin.History_Deposit_New_ETH(User.Identity.Name);
            var Newdepositbit = await _sdkApiAdmin.History_Deposit_New_BIT(User.Identity.Name);

            var depositcurrencyeth = JsonConvert.DeserializeObject<List<DepositETHCurrency>>(currencydepositeth.ToString());
            var depositcurrencybit = JsonConvert.DeserializeObject<List<DepositBITCurrency>>(currencydepositbit.ToString());
            var depositneweth = JsonConvert.DeserializeObject<List<DepositETHNew>>(Newdepositeth.ToString());
            var depositnewbit = JsonConvert.DeserializeObject<List<DepositBITNew>>(Newdepositbit.ToString());
            #endregion
            #region call api history exchange
            var currencyexchangeeth = await _sdkApiAdmin.History_Exchange_Currency_ETH(User.Identity.Name);
            var currencyexchangebit = await _sdkApiAdmin.History_Exchange_Currency_BIT(User.Identity.Name);
            var Newexchangeeth = await _sdkApiAdmin.History_Exchange_New_ETH(User.Identity.Name);
            var Newexchangebit = await _sdkApiAdmin.History_Exchange_New_BIT(User.Identity.Name);

            var exchangecurrencyeth = JsonConvert.DeserializeObject<List<ExchangeETHCurrency>>(currencyexchangeeth.ToString());
            var exchangecurrencybit = JsonConvert.DeserializeObject<List<ExchangeBITCurrency>>(currencyexchangebit.ToString());
            var exchangeneweth = JsonConvert.DeserializeObject<List<ExchangeETHNew>>(Newexchangeeth.ToString());
            var exchangenewbit = JsonConvert.DeserializeObject<List<ExchangeBITNew>>(Newexchangebit.ToString());
            #endregion
            #region call api history withdraw
            var currencywithdraweth = await _sdkApiAdmin.History_Withdraw_Currency_ETH(User.Identity.Name);
            var currencywithdrawbit = await _sdkApiAdmin.History_Withdraw_Currency_BIT(User.Identity.Name);
            var Newwithdraweth = await _sdkApiAdmin.History_Withdraw_New_ETH(User.Identity.Name);
            var Newwithdrawbit = await _sdkApiAdmin.History_Withdraw_New_BIT(User.Identity.Name);

            var withdrawcurrencyeth = JsonConvert.DeserializeObject<List<WithdrawETHCurrency>>(currencywithdraweth.ToString());
            var withdrawcurrencybit = JsonConvert.DeserializeObject<List<WithdrawBITCurrency>>(currencywithdrawbit.ToString());
            var withdrawneweth = JsonConvert.DeserializeObject<List<WithdrawETHNew>>(Newwithdraweth.ToString());
            var withdrawnewbit = JsonConvert.DeserializeObject<List<WithdrawBITNew>>(Newwithdrawbit.ToString());
            #endregion
            History_Modal obj = new History_Modal()
            {
                #region deposit
                depositethcurrency = depositcurrencyeth,
                depositethnew = depositneweth,
                depositbitcurrency = depositcurrencybit,
                depositbitnew = depositnewbit,
                #endregion
                #region exchange
                exchangeethcurrency = exchangecurrencyeth,
                exchangeethnew = exchangeneweth,
                exchangebitcurrency = exchangecurrencybit,
                exchangebitnew = exchangenewbit,
                #endregion
                #region withdraw
                withdrawethcurrency = withdrawcurrencyeth,
                withdrawethnew = withdrawneweth,
                withdrawbitcurrency = withdrawcurrencybit,
                withdrawbitnew = withdrawnewbit
                #endregion
            };
            ViewBag.scriptsViewPort = "<meta name='viewport' content='width=device-width, initial-scale=0.5'>";
            return View(obj);
        }
        public async System.Threading.Tasks.Task<ActionResult> RunGame()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://apitest.fundist.org");
                    client.DefaultRequestHeaders.Accept.Clear();
                    HttpResponseMessage response = await client.PostAsync("/System/Api/96778c3a2b606891cf5f9da390cb9d71/User/DirectAuth/?Login=longt&Password=123456789&System=998&TID=a65a5&Hash=94cc7eb7f8e3f06b2dba761f1d395029&Page=3Aorj256pdcg2hvhs&UserIP=14.161.36.53&Language=en&Currency=USD", null);
                    if (response.IsSuccessStatusCode)
                    {

                        var sa = response.Content.ReadAsStringAsync().Result.Split(',')[1];
                        return View("RunGame", model: sa);

                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "ErrorMessage");
            }

        }
        #region header and footer
        public ActionResult _getHeader()
        {
            return PartialView("_header");
        }
        public ActionResult TestAPI()
        {
            //var aa = new PaymentCryptoApi();
            //var model = aa.CreateAccountPassword("addfsfdss");
            return View();
        }
        public ActionResult _getFooter()
        {
            return PartialView("_footer");
        }
        public ActionResult getBannerPlaynow()
        {
            return PartialView("_bannerPlayNow");
        }
        #endregion
        public void SetSession()
        {
            var userId = User.Identity.GetUserId();
            Session["User"] = _userManager.FindById(userId);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> Getsystem(string userName)
        {
            var CalcApiTransaction = await _sdkApiAdmin.SDKApiSystemBetGetAsync(DOMAIN_API + "api/dashboard/", userName);
            SystemBetDashboardModel modelParseTransaction = new SystemBetDashboardModel();


            if (!string.IsNullOrEmpty(CalcApiTransaction.ToString()))
            {
                modelParseTransaction = JsonConvert.DeserializeObject<SystemBetDashboardModel>(CalcApiTransaction);//parse object transaction to model
                return Json(modelParseTransaction, JsonRequestBehavior.AllowGet);
            }
            return Json(modelParseTransaction, JsonRequestBehavior.AllowGet);
        }
        #region call ajax
        //Call api dashboard lấy banlance ETH, VIP, TOTAL BET
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> GetAccountDashboardAsync(string userName)//type 1
        {
            var CalcApiTransaction = await _sdkApiAdmin.SDKApiTransactionGetAsync(DOMAIN_API + "api/dashboard/", userName);
            DashboardValue modelParseTransaction = new DashboardValue();


            if (!string.IsNullOrEmpty(CalcApiTransaction.ToString()))
            {
                //Log.Info("Call api admin success !");
                //JObject ApiAdminCall = JObject.Parse(CalcApiTransaction.ToString());
                //var objTransaction = ApiAdminCall["Transactions"].ToString();
                //BetDashboard dashBet = new BetDashboard()
                //{
                //    Eth_balance = ApiAdminCall["Balance"].ToString() == null ? "0" : ApiAdminCall["Balance"].ToString(),
                //    Vip_balance = ApiAdminCall["BalanceVip"].ToString() == null ? "0" : ApiAdminCall["BalanceVip"].ToString(),
                //    Totalbet = ApiAdminCall["TotalBet"].ToString() == null ? "0" : ApiAdminCall["TotalBet"].ToString(),
                //    Systembet = ApiAdminCall["SystemBet"].ToString() == null ? "0" : ApiAdminCall["SystemBet"].ToString(),
                //    AdressKey = ApiAdminCall["AdressKey"].ToString() == null ? "0" : ApiAdminCall["AdressKey"].ToString(),
                //    UserName = ApiAdminCall["UserName"].ToString() == null ? "0" : ApiAdminCall["UserName"].ToString(),
                //    MinETH = ApiAdminCall["PrivateKey"].ToString() == null ? "0" : ApiAdminCall["PrivateKey"].ToString(),
                //    MinVIP = ApiAdminCall["Temp5"].ToString() == null ? "0" : ApiAdminCall["Temp5"].ToString(),
                //    RateETH = ApiAdminCall["Temp6"].ToString() == null ? "0" : ApiAdminCall["Temp6"].ToString(),
                //    WidthdrawETHFee = ApiAdminCall["Password"].ToString() == null ? "0" : ApiAdminCall["Password"].ToString(),
                //    WidthdrawVIPFee = ApiAdminCall["PublicKey"].ToString() == null ? "0" : ApiAdminCall["PublicKey"].ToString()
                //};
                //ViewBag.lstBet = dashBet;
                //ViewBag.AddressKey = ApiAdminCall["AdressKey"].ToString();
                //ViewBag.UserName = ApiAdminCall["UserName"].ToString();
                //ViewBag.MinETH = ApiAdminCall["PrivateKey"].ToString();
                //ViewBag.MinETH_1 = ApiAdminCall["Temp5"].ToString();
                //ViewBag.RateETH = ApiAdminCall["Temp6"].ToString();
                //ViewBag.WidthdrawETHFee = ApiAdminCall["Password"].ToString();
                //ViewBag.WidthdrawVIPFee = ApiAdminCall["PublicKey"].ToString();
                //List<ObjTransaction> lstObject = new List<ObjTransaction>();
                modelParseTransaction = JsonConvert.DeserializeObject<DashboardValue>(CalcApiTransaction);//parse object transaction to model
                                                                                                          //for (int i = 0; i < modelParseTransaction.Count; i++)
                                                                                                          //{
                                                                                                          //    if (modelParseTransaction[i].Type == 4 || modelParseTransaction[i].Type == 1 || modelParseTransaction[i].Type == 5)
                                                                                                          //    {
                                                                                                          //        modelParseTransaction[i].ValueAtPlayer = modelParseTransaction[i].ValueAtPlayer == null ? "0" : modelParseTransaction[i].ValueAtPlayer.ToString();
                                                                                                          //        var dateToString = modelParseTransaction[i].DateCreate.ToString();
                                                                                                          //        modelParseTransaction[i].DateCreate = dateToString;

                //        lstObject.Add(modelParseTransaction[i]);
                //    }
                //}
                //var modelParseApiTransaction = JsonConvert.DeserializeObject<List<TransactionApi>>(ApiAdminCall.ToString());//parse object wallet to model
                //List<BetDashboard> lstBet = new List<BetDashboard>();
                //lstBet.Add(dashBet);
                //var model = new { listObj = lstObject, listBet = lstBet };

                return Json(modelParseTransaction, JsonRequestBehavior.AllowGet);
            }
            return Json(modelParseTransaction, JsonRequestBehavior.AllowGet);
        }
        //ajax gửi mail transaction
        //[HttpPost]
        //public JsonResult MailTransaction(string id, string email, string timestamp, string Amount, string AddressTo, string FeeTransaction, string username)
        //{
        //    //gọi đến class confirmemail -> hàm TransactionEmailAccount
        //    var resMailTransaction = _confirmEmail.TransactionEmailAccount(id, email, timestamp, Server, Amount, AddressTo, FeeTransaction, username);//gọi tới controller confirmEmail và action Transaction mail để gửi mail xác nhận
        //    return Json(resMailTransaction);
        //}
        //[HttpPost]
        //public JsonResult MailTransactionVIP(string id, string email, string timestamp, string Amount, string AddressTo, string FeeTransaction, string username)
        //{
        //    //gọi đến class confirmemail -> hàm TransactionEmailAccount
        //    var resMailTransaction = _confirmEmail.TransactionEmailAccountVIP(id, email, timestamp, Server, Amount, AddressTo, FeeTransaction, username);//gọi tới controller confirmEmail và action Transaction mail để gửi mail xác nhận
        //    return Json(resMailTransaction);
        //}
        #endregion
        [HttpPost]
        public JsonResult SearchName(string userName)
        {
            var name = _userManager.FindByName(userName).Email;
            return Json(name, JsonRequestBehavior.AllowGet);
        }
    }
    #region parse JsonObject to Model
    //public class Trans
    //{
    //    public string en { get; set; }
    //    [JsonProperty("zh-cn")]
    //    public string ZhCn { get; set; }
    //    public string de { get; set; }
    //    public string pt { get; set; }
    //    [JsonProperty("pt-pt")]
    //    public string PtPt { get; set; }
    //    public string sv { get; set; }
    //    public string b5 { get; set; }
    //    public string ru { get; set; }
    //    public string uk { get; set; }
    //    public string zh { get; set; }
    //    public string cn { get; set; }
    //    public string ja { get; set; }
    //    [JsonProperty("zh-hant")]
    //    public string ZhHant { get; set; }
    //    [JsonProperty("zh-hans")]
    //    public string ZhHans { get; set; }
    //    public string ko { get; set; }
    //    public string da { get; set; }
    //    public string no { get; set; }
    //    public string th { get; set; }
    //    public string tr { get; set; }
    //}
    //public class Name
    //{
    //    public string en { get; set; }
    //    [JsonProperty("zh-cn")]
    //    public string ZhCn { get; set; }
    //    public string de { get; set; }
    //    public string pt { get; set; }
    //    [JsonProperty("pt-pt")]
    //    public string PtPt { get; set; }
    //    public string sv { get; set; }
    //    public string b5 { get; set; }
    //    public string ru { get; set; }
    //    public string uk { get; set; }
    //    public string zh { get; set; }
    //    public string cn { get; set; }
    //    public string ja { get; set; }
    //    [JsonProperty("zh-hant")]
    //    public string ZhHant { get; set; }
    //    [JsonProperty("zh-hans")]
    //    public string ZhHans { get; set; }
    //    public string ko { get; set; }
    //    public string da { get; set; }
    //    public string no { get; set; }
    //    public string th { get; set; }
    //    public string tr { get; set; }
    //}
    //public class Category
    //{
    //    public string ID { get; set; }
    //    public Trans Trans { get; set; }
    //    public List<string> Tags { get; set; }
    //    public Name Name { get; set; }
    //    public string CSort { get; set; }
    //    public string CSubSort { get; set; }
    //    public string Slug { get; set; }
    //}
    //public class Name2
    //{
    //    public string en { get; set; }
    //    public string ru { get; set; }
    //}
    //public class Description
    //{
    //}
    //public class SortPerCategory
    //{
    //    //public int _33 { get; set; }
    //    //public int _37 { get; set; }
    //    //public int? _7 { get; set; }
    //    //public int? _41 { get; set; }
    //}
    //public class GameFullList
    //{
    //    public string ID { get; set; }
    //    public string Image { get; set; }
    //    public string Url { get; set; }
    //    public Name2 Name { get; set; }
    //    public Description Description { get; set; }
    //    public string MobileUrl { get; set; }
    //    public int Branded { get; set; }
    //    public int SuperBranded { get; set; }
    //    public int hasDemo { get; set; }
    //    public List<string> CategoryID { get; set; }
    //    public SortPerCategory SortPerCategory { get; set; }
    //    public string MerchantID { get; set; }
    //    public string SubMerchantID { get; set; }
    //    public string AR { get; set; }
    //    public string IDCountryRestriction { get; set; }
    //    public string Sort { get; set; }
    //    public string PageCode { get; set; }
    //    public string MobilePageCode { get; set; }
    //    public object MobileAndroidPageCode { get; set; }
    //    public object MobileWindowsPageCode { get; set; }
    //    public object ExternalCode { get; set; }
    //    public object MobileExternalCode { get; set; }
    //    public string ImageFullPath { get; set; }
    //    public object WorkingHours { get; set; }
    //    public string IsVirtual { get; set; }
    //    public string TableID { get; set; }
    //    public string Freeround { get; set; }

    //}
    //public class _983
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public object IDParent { get; set; }
    //    public string Alias { get; set; }
    //    public string Image { get; set; }
    //}

    //public class _998
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public object IDParent { get; set; }
    //    public string Alias { get; set; }
    //    public string Image { get; set; }
    //}

    //public class Merchants
    //{
    //    public _983 __983 { get; set; }
    //    public _998 __998 { get; set; }
    //}

    //public class _15
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string IDMerchant { get; set; }
    //    public List<string> Countries { get; set; }
    //    public string IsDefault { get; set; }
    //    public string IDParent { get; set; }
    //    public string IDApiTemplate { get; set; }
    //}

    //public class _20
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string IDMerchant { get; set; }
    //    public List<string> Countries { get; set; }
    //    public string IsDefault { get; set; }
    //    public string IDParent { get; set; }
    //    public string IDApiTemplate { get; set; }
    //}

    //public class _22
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string IDMerchant { get; set; }
    //    public List<string> Countries { get; set; }
    //    public string IsDefault { get; set; }
    //    public string IDParent { get; set; }
    //    public string IDApiTemplate { get; set; }
    //}

    //public class _37
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string IDMerchant { get; set; }
    //    public List<string> Countries { get; set; }
    //    public string IsDefault { get; set; }
    //    public string IDParent { get; set; }
    //    public string IDApiTemplate { get; set; }
    //}

    //public class _100
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string IDMerchant { get; set; }
    //    public List<string> Countries { get; set; }
    //    public string IsDefault { get; set; }
    //    public string IDParent { get; set; }
    //    public string IDApiTemplate { get; set; }
    //}

    //public class _124
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string IDMerchant { get; set; }
    //    public List<string> Countries { get; set; }
    //    public string IsDefault { get; set; }
    //    public string IDParent { get; set; }
    //    public string IDApiTemplate { get; set; }
    //}

    //public class _125
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string IDMerchant { get; set; }
    //    public List<string> Countries { get; set; }
    //    public string IsDefault { get; set; }
    //    public string IDParent { get; set; }
    //    public string IDApiTemplate { get; set; }
    //}

    //public class CountriesRestrictions
    //{
    //    public _15 __15 { get; set; }
    //    public _20 __20 { get; set; }
    //    public _22 __22 { get; set; }
    //    public _37 __37 { get; set; }
    //    public _100 __100 { get; set; }
    //    public _124 __124 { get; set; }
    //    public _125 __125 { get; set; }
    //}

    //public class MerchantsCurrency
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string IDMerchant { get; set; }
    //    public List<string> Currencies { get; set; }
    //    public string DefaultCurrency { get; set; }
    //    public string IsDefault { get; set; }
    //}

    //public class Root
    //{
    //    public List<Category> categories { get; set; }
    //    public List<GameFullList> games { get; set; }
    //    public Merchants merchants { get; set; }
    //    public CountriesRestrictions countriesRestrictions { get; set; }
    //    public List<MerchantsCurrency> merchantsCurrencies { get; set; }
    //}
    #endregion
    #region JsonObject Transaction to model
    public class TransactionApi
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public object Password { get; set; }
        public string AdressKey { get; set; }
        public object PrivateKey { get; set; }
        public object PublicKey { get; set; }
        public string KeyStore { get; set; }
        public object RestoreWord { get; set; }
        public DateTime DateCreate { get; set; }
        public string Balance { get; set; }
        public string RawData { get; set; }
        public string BalanceVip { get; set; }
        public string Temp3 { get; set; }
        public string Temp4 { get; set; }
        public string Temp5 { get; set; }
        public string Temp6 { get; set; }
        public bool IsActived { get; set; }
        public string TotalBet { get; set; }
        public int ParentId { get; set; }
        public string SystemBet { get; set; }
        public string LevelId { get; set; }
        public string Parent { get; set; }
        public object HookUp { get; set; }
        public object InverseParent { get; set; }
        public object LogBet { get; set; }
        public ObjTransaction Transactions { get; set; }
    }
    public class ObjTransaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public string ToAdressPlayer { get; set; }
        public string ToAddressOuter { get; set; }
        public string HashPlayer { get; set; }
        public string HashOuter { get; set; }
        public string FromAdressPlayer { get; set; }
        public string ValueAtPlayer { get; set; }
        public string FeeAt1Percent { get; set; }
        public string StatusAtPlayer { get; set; }
        public string DateCreate { get; set; }
        public string PriceGasPlayer { get; set; }
        public string PriceGasOuter { get; set; }
        public string GasLimitPlayer { get; set; }
        public string GasLimitOuter { get; set; }
        public int? ConfirmationsOuter { get; set; }
        public int? ConfirmationsPlayer { get; set; }
        public string BalanceAfterTx { get; set; }
        public string TimestampPlayer { get; set; }
        public string TimestampOuter { get; set; }
        public string GasUsedPlayer { get; set; }
        public string GasUsedOuter { get; set; }
        public string RawDataPlayer { get; set; }
        public string RawDataOuter { get; set; }
        public string Temp1 { get; set; }
        public string Temp2 { get; set; }
        public string Temp3 { get; set; }
        public string Temp4 { get; set; }
        public string Temp5 { get; set; }
        public bool IsWithdraw { get; set; }
        public int Type { get; set; }
        public string FeeTransaction { get; set; }
        public string EthAmount { get; set; }
        public string VipAmount { get; set; }
        public string EthToVipAmount { get; set; }
        public string VipToEthAmount { get; set; }
        public string BalanceVipAfterTx { get; set; }
        public string EthToVipPercent { get; set; }
        public string VipToEthPercent { get; set; }
        public string EthRating { get; set; }
        public string VipRating { get; set; }
    }
    public class BetDashboard
    {
        public string Eth_balance { get; set; }
        public string Vip_balance { get; set; }
        public string Totalbet { get; set; }
        public string Systembet { get; set; }
        public string AdressKey { get; set; }
        public string UserName { get; set; }
        public string MinETH { get; set; }
        public string MinVIP { get; set; }
        public string RateETH { get; set; }
        public string WidthdrawETHFee { get; set; }
        public string WidthdrawVIPFee { get; set; }
    }
    #endregion
    public class DashboardValue
    {
        public double bonus { get; set; }
        public double eth { get; set; }
        public double vip { get; set; }
        public double totalbet { get; set; }
        public string systembet { get; set; }
        public int level { get; set; }
        public int refferal { get; set; }
        public double minEth { get; set; }
        public double maxEth { get; set; }
        public double minVip { get; set; }
        public double maxVip { get; set; }
        public double minExchange { get; set; }
        public double withdrawfee { get; set; }
        public double withdrawvipfee { get; set; }
        public double EthExchangeVipFee { get; set; }
        public double VipExchangeEthFee { get; set; }
    }

    public class SystemBetDashboardModel
    {
        public string systembet { get; set; }
        public string totalreff { get; set; }
    }
    #region parse again fulist game
    public class Trans
    {
        public string en { get; set; }
        [JsonProperty("zh-cn")]
        public string ZhCn { get; set; }
        public string de { get; set; }
        public string pt { get; set; }
        [JsonProperty("pt-pt")]
        public string PtPt { get; set; }
        public string sv { get; set; }
        public string b5 { get; set; }
        public string ru { get; set; }
        public string uk { get; set; }
        public string zh { get; set; }
        public string cn { get; set; }
        public string ja { get; set; }
        [JsonProperty("zh-hant")]
        public string ZhHant { get; set; }
        [JsonProperty("zh-hans")]
        public string ZhHans { get; set; }
        public string ko { get; set; }
        public string da { get; set; }
        public string no { get; set; }
        public string th { get; set; }
        public string tr { get; set; }
    }

    public class Name
    {
        public string en { get; set; }
        [JsonProperty("zh-cn")]
        public string ZhCn { get; set; }
        public string de { get; set; }
        public string pt { get; set; }
        [JsonProperty("pt-pt")]
        public string PtPt { get; set; }
        public string sv { get; set; }
        public string b5 { get; set; }
        public string ru { get; set; }
        public string uk { get; set; }
        public string zh { get; set; }
        public string cn { get; set; }
        public string ja { get; set; }
        [JsonProperty("zh-hant")]
        public string ZhHant { get; set; }
        [JsonProperty("zh-hans")]
        public string ZhHans { get; set; }
        public string ko { get; set; }
        public string da { get; set; }
        public string no { get; set; }
        public string th { get; set; }
        public string tr { get; set; }
    }

    public class Category
    {
        public string ID { get; set; }
        public Trans Trans { get; set; }
        public List<string> Tags { get; set; }
        public Name Name { get; set; }
        public string CSort { get; set; }
        public string CSubSort { get; set; }
        public string Slug { get; set; }
    }

    public class Description
    {
    }

    public class Game
    {
        public string ID { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public Name Name { get; set; }
        public Description Description { get; set; }
        public string MobileUrl { get; set; }
        public int Branded { get; set; }
        public int SuperBranded { get; set; }
        public int hasDemo { get; set; }
        public List<string> CategoryID { get; set; }
        public object SortPerCategory { get; set; }
        public string MerchantID { get; set; }
        public string SubMerchantID { get; set; }
        public string AR { get; set; }
        public string IDCountryRestriction { get; set; }
        public string Sort { get; set; }
        public string PageCode { get; set; }
        public string MobilePageCode { get; set; }
        public object MobileAndroidPageCode { get; set; }
        public object MobileWindowsPageCode { get; set; }
        public object ExternalCode { get; set; }
        public object MobileExternalCode { get; set; }
        public string ImageFullPath { get; set; }
        public object WorkingHours { get; set; }
        public string IsVirtual { get; set; }
        public string TableID { get; set; }
        public string Freeround { get; set; }
    }

    public class _998
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public object IDParent { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
    }

    public class _980
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public object IDParent { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
    }

    public class Merchants
    {
        public _998 _998 { get; set; }
        public _980 _980 { get; set; }
    }

    public class _15
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _20
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _22
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _61
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _124
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class _125
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Countries { get; set; }
        public string IsDefault { get; set; }
        public string IDParent { get; set; }
        public string IDApiTemplate { get; set; }
    }

    public class CountriesRestrictions
    {
        public _15 _15 { get; set; }
        public _20 _20 { get; set; }
        public _22 _22 { get; set; }
        public _61 _61 { get; set; }
        public _124 _124 { get; set; }
        public _125 _125 { get; set; }
    }

    public class MerchantsCurrency
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDMerchant { get; set; }
        public List<string> Currencies { get; set; }
        public string DefaultCurrency { get; set; }
        public string IsDefault { get; set; }
    }

    public class Root
    {
        public List<Category> categories { get; set; }
        public List<Game> games { get; set; }
        public Merchants merchants { get; set; }
        public CountriesRestrictions countriesRestrictions { get; set; }
        public List<MerchantsCurrency> merchantsCurrencies { get; set; }
    }

    #endregion
    public class History_Modal
    {
        #region Deposit
        public List<DepositETHCurrency> depositethcurrency { get; set; }
        public List<DepositETHNew> depositethnew { get; set; }
        public List<DepositBITCurrency> depositbitcurrency { get; set; }
        public List<DepositBITNew> depositbitnew { get; set; }
        #endregion
        #region Exchange
        public List<ExchangeETHCurrency> exchangeethcurrency { get; set; }
        public List<ExchangeETHNew> exchangeethnew { get; set; }
        public List<ExchangeBITCurrency> exchangebitcurrency { get; set; }
        public List<ExchangeBITNew> exchangebitnew { get; set; }
        #endregion
        #region Withdraw
        public List<WithdrawETHCurrency> withdrawethcurrency { get; set; }
        public List<WithdrawETHNew> withdrawethnew { get; set; }
        public List<WithdrawBITCurrency> withdrawbitcurrency { get; set; }
        public List<WithdrawBITNew> withdrawbitnew { get; set; }
        #endregion
    }
}