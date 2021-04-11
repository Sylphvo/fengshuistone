using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using Labixa.HashMD;
using log4net;
using log4net.Config;
using Outsourcing.Data.Infrastructure;

namespace Labixa.Common
{
    public interface ISDKApiFundist
    {
        System.Threading.Tasks.Task<string> ApiCreateUser(string login, string pass, string TID, string regIP, string nickName);

        //string ApiCreateUser(string login, string pass, string TID, string regIP, string gender, string country, DateTime dateOfBirth, string nick, string timezone, string name, string lastName, string phone, string albPhone, string city, string address, string email, string AffiliateID);
        string ApiUpdateUserInfo(string login, string pass, string TID, string regIP, string gender, string country, DateTime dateOfBirth, string nick, string timezone, string name, string lastName, string phone, string albPhone, string city, string address, string email, string AffiliateID);
        string ApiEnableUser(string login, string TID);
        string ApiDisableUser(string login, string TID);
        string ApiSetBalance_Transfer(string login, string amount, string TID, string type, string system);
        string ApiGetUser_BalanceSpecified(string login, string TID, string system);
        System.Threading.Tasks.Task<string> ApiKill_Authorization(string login, string TID);
        string ApiAuthorization_with_HTML(string login, string TID, string pass, string page, string userIP, string system);
        System.Threading.Tasks.Task<string> ApiGetFullList(string TID);
        System.Threading.Tasks.Task<string> ApiDirect_AuthorizationAsync(string login, string TID, string system, string pass, string page, string UserIP);
    };
    public class SDKApiFundist : ISDKApiFundist
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string SERVER_IP = System.Configuration.ConfigurationManager.AppSettings["server_ip"];
        private string API_KEY = System.Configuration.ConfigurationManager.AppSettings["api_key"];
        private string API_PWD = System.Configuration.ConfigurationManager.AppSettings["api_password"];
        private string SYSTEM_EVO = System.Configuration.ConfigurationManager.AppSettings["system_evo"];
        private string SYSTEM_EZUGI = System.Configuration.ConfigurationManager.AppSettings["system_ezugi"];
        private string CURRENCY = System.Configuration.ConfigurationManager.AppSettings["currency"];
        private string LANGUAGE = System.Configuration.ConfigurationManager.AppSettings["language"];
        private string URL_API_FUNDIST = System.Configuration.ConfigurationManager.AppSettings["url_api_fundist"];

        //public string URL_API_FUNDIST = URL_API_FUNDIST;
        private HashMD5 hashMD5 = new HashMD5();
        private readonly IUnitOfWork _unitOfWork;
        public SDKApiFundist(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        /// <summary>
        /// New user creation 
        /// Hash MD5: User/Add/[CASINO_SERVER_IP]/[TID]/[KEY]/[LOGIN]/[PASSWORD]/[CURRENCY]/[PWD]
        /// url: /System/Api/[KEY]/User/Add/?
        /// Amount Param: 20 params
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <param name="TID"></param>
        /// <param name="regIP">kiểm tra IP của quốc gia đó có trong danh sách bị cấm của fundits</param>
        /// <param name="gender"></param>
        /// <param name="country"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="nick"></param>
        /// <param name="timezone">muối giờ</param>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="phone">sdt để đăng ký tài khoản</param>
        /// <param name="albPhone">sô điện thoại đẻ reset tài khoản</param>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <param name="email"></param>
        /// <param name="AffiliateID">ID liên kết bên thứ 3</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiCreateUser(string login, string pass, string TID, string regIP, string nickName)
        {
            string strMdD5 = hashMD5.Fundist_Hash(login, pass, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/User/Add/?&Login=" + login + "&Password=" + pass + "&TID=" + TID + "&Currency=" + CURRENCY + "&Hash=" + strMdD5 + "&Language=" + LANGUAGE + "&Nick=" + nickName + "&RegistrationIP=" + regIP, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        //public string ApiCreateUser(string login, string pass, string TID, string regIP, string gender, string country, DateTime dateOfBirth, string nick, string timezone, string name, string lastName, string phone, string albPhone, string city, string address, string email, string AffiliateID)
        //{
        //    string strMdD5 = hashMD5.Fundist_Hash(login, pass, TID);
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(URL_API_FUNDIST);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        //        HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/User/Add/?Login=" + login + "&Password=" + pass + "&Currency=" + CURRENCY + "&Hash=" + strMdD5 + "&Language=" + LANGUAGE + "&RegistrationIP=" + regIP + "&Gender=" + gender + "&Country=" + country + "&DateOfBirth=" + dateOfBirth + "&Nick=" + nick + "&Timezone=" + timezone + "&Name=" + name + "&LastName=" + lastName + "&Phone=" + phone + "&AlternativePhone=" + albPhone + "&City=" + city + "&Address=" + address + "&Email=" + email + "&AffiliateID=" + AffiliateID).Result;
        //        if (response.IsSuccessStatusCode)
        //        {

        //            return response.Content.ReadAsStringAsync().Result;
        //        }
        //        return null;
        //    }
        //}
        //public string ApiCreateUser(string login, string pass, string TID, string regIP)
        //{
        //    string strMdD5 = hashMD5.Fundist_Hash(login, pass, TID);
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(URL_API_FUNDIST);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        //        HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/User/Add/?Login=" + login + "&Password=" + pass + "&Currency=" + CURRENCY + "&Hash=" + strMdD5 + "&Language=" + LANGUAGE + "&RegistrationIP=" + regIP).Result;
        //        if (response.IsSuccessStatusCode)
        //        {

        //            return response.Content.ReadAsStringAsync().Result;
        //        }
        //        return null;
        //    }
        //}
        /// <summary>
        /// Update User Infor
        /// Hash MD5: User/Update/[CASINO_SERVER_IP]/[TID]/[KEY]/[LOGIN]/[PASSWORD]/[CURRENCY]/[PWD]
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <param name="TID"></param>
        /// <param name="regIP"></param>
        /// <param name="gender"></param>
        /// <param name="country"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="nick"></param>
        /// <param name="timezone"></param>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="phone"></param>
        /// <param name="albPhone"></param>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <param name="email"></param>
        /// <param name="AffiliateID"></param>
        /// <returns></returns>
        public string ApiUpdateUserInfo(string login, string pass, string TID, string regIP, string gender, string country, DateTime dateOfBirth, string nick, string timezone, string name, string lastName, string phone, string albPhone, string city, string address, string email, string AffiliateID)
        {
            string strMdD5 = hashMD5.HashUpdateUserInfo(login, pass, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/User/Add/?&Login=" + login + "&Password=" + pass + "&Currency=" + CURRENCY + "&Hash=" + strMdD5 + "&Language=" + LANGUAGE + "&RegistrationIP=" + regIP + "&Gender=" + gender + "&Country=" + country + "&DateOfBirth=" + dateOfBirth + "&Nick=" + nick + "&Timezone=" + timezone + "&Name=" + name + "&LastName=" + lastName + "&Phone=" + phone + "&AlternativePhone=" + albPhone + "&City=" + city + "&Address=" + address + "&Email=" + email + "&AffiliateID=" + AffiliateID).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Enable/Diable user
        /// Hash MD5: HashEnableUser
        /// Amount: 3 params
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <param name="TID"></param>
        /// <returns></returns>
        public string ApiEnableUser(string login, string TID)
        {
            string strMdD5 = hashMD5.HashEnableUser(login, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/User/Enable/?&Login=" + login + "&TID=" + TID + "&Hash=" + strMdD5).Result;//có thể option reason
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        public string ApiDisableUser(string login, string TID)
        {
            string strMdD5 = hashMD5.HashEnableUser(login, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/User/Disable/?&Login=" + login + "&TID=" + TID + "&Hash=" + strMdD5).Result;//có thể option reason
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="amount">số lượng transaction</param>
        /// <param name="TID"></param>
        /// <param name="type">có 2 type: Deposit/Widthdraw</param>
        /// <param name="system">có 2 system: EVO/EZUI</param>
        /// <returns></returns>
        public string ApiSetBalance_Transfer(string login, string amount, string TID, string type, string system)
        {
            string strMdD5 = hashMD5.Hash_SetBalanceUser(login, TID, amount, system);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/Balance/Set/?&Login=" + login + "&System=" + system + "&Amount=" + amount + "&TID=" + TID + "&Currency=" + CURRENCY + "&Hash=" + strMdD5 + "&Type=" + type).Result;//có thể option reason
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Get user balance/widthDraw được chỉ định cụ thể
        /// </summary>
        /// <param name="login"></param>
        /// <param name="TID"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public string ApiGetUser_BalanceSpecified(string login, string TID, string system)
        {
            string strMdD5 = hashMD5.Hash_getbalance(login, TID, system);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/Balance/Get/?&Login=" + login + "&System=" + system + "&TID=" + TID + "&Hash=" + strMdD5).Result;//có thể option reason
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        public string ApiGetUser_WidthDrawSpecified(string login, string TID, string system)
        {
            string strMdD5 = hashMD5.Hash_getbalance(login, TID, system);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/Balance/Withdraw/?&Login=" + login + "&System=" + system + "&TID=" + TID + "&Hash=" + strMdD5).Result;//có thể option reason
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Direct authorization
        /// option
        /// {&Language=[LANGUAGE]}
        /// {&Nick=[NICK]}
        /// {&Timezone=[TIMEZONE]}
        /// { &Demo =[DEMO]}
        /// { &IsMobile =[ISMOBILE]}
        /// { &ExtParam =[EXTPARAM]}
        /// { &UserAutoCreate =[USERAUTOCREATE]}
        /// { &Currency =[CURRENCY]}
        /// { &Country =[COUNTRY]}
        /// </summary>
        /// <param name="login"></param>
        /// <param name="TID"></param>
        /// <param name="system"></param>
        /// <param name="pass"></param>
        /// <param name="page">mã page code</param>
        /// <param name="UserId">IP được uỷ quyền</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiDirect_AuthorizationAsync(string login, string TID, string system, string pass, string page, string UserIP)
        {
           
            string strMdD5 = hashMD5.Hash_Direct_Authorization(login, TID, pass, system);
            log.Info("DirectAuth-Long");
            log.Info("System/Api/" + API_KEY + "/User/DirectAuth/?&Login=" + login
                                                + "&Password=" + pass + "&System=" + system +
                                                "&TID=" + TID + "&Hash=" + strMdD5 + "&Page=" + page + "&UserIP=" + UserIP + "&Language=en&Country=VN&Currency=USD");
            using (var client = new HttpClient())
            {
                var b = "System/Api/" + API_KEY + "/User/DirectAuth/?&Login=" + login
                                                + "&Password=" + pass + "&System=" + system +
                                                "&TID=" + TID + "&Hash=" + strMdD5 + "&Page=" + page + "&UserIP=" + UserIP + "&Language=en&Country=VN&Currency=USD";
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/User/DirectAuth/?&Login=" + login
                                                + "&Password=" + pass + "&System=" + system +
                                                "&TID=" + TID + "&Hash=" + strMdD5 + "&Page=" + page + "&UserIP=" + UserIP+ "&Language=en&Timezone=+7&Country=VN&Currency=USD", null));//có thể option reason
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Kill_Authorization
        /// option param: EXTPARAM chỉ xoá liên kết đến giá trị
        /// </summary>
        /// <param name="login"></param>
        /// <param name="TID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiKill_Authorization(string login, string TID)
        {
            string strMdD5 = hashMD5.Hash_Kill_Authorization(login, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync("System/Api/" + API_KEY + "/User/KillAuth/?&Login=" + login + "&TID=" + TID + "&Hash=" + strMdD5);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        public string ApiAuthorization_with_HTML(string login, string TID, string pass, string page, string userIP, string system)
        {
            string strMdD5 = hashMD5.HashAuthorization_with_HTML(login, TID, pass, system);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = client.GetAsync("System/Api/" + API_KEY + "/User/AuthHTML/?&Login=" + login + "&Password=" + pass + "&System=" + system + "&TID=" + TID + "&Hash=" + strMdD5 + "&Page=" + page + "&UserIP=" + userIP).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        #region Get game
        /// <summary>
        /// Get all game in fundist
        /// Hash: Game/FullList/[IP]/[TID]/[KEY]/[PWD]
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiGetFullList(string TID)
        {
            string strMdD5 = hashMD5.HashGameFullList(TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Game/FullList/?&TID=" + TID + "&Hash=" + strMdD5, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        public async System.Threading.Tasks.Task<string> ApiGetcategoriesGame(string TID)
        {
            string strMdD5 = hashMD5.HashGameCategories(TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Game/Categories/?&TID=" + TID + "&Hash=" + strMdD5, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        public async System.Threading.Tasks.Task<string> ApiGetAvailableGame(string TID)
        {
            string strMdD5 = hashMD5.HashGameAvailable(TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Game/List/?&TID=" + TID + "&Hash=" + strMdD5, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        public async System.Threading.Tasks.Task<string> ApiGetLobbyState(string TID, string table)
        {
            string strMdD5 = hashMD5.HashLobbyState(TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Tables/LobbyState/?&Table="+table+"&TID=" + TID + "&Hash=" + strMdD5, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        #endregion
        /// <summary>
        /// Lấy lịch sử tiền cược của người chơi
        /// Hash: Stats/BetsSummary/[IP]/[TID]/[KEY]/[DATE]/[PWD]
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="TID">follow format(YYYY-MM-DD)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiBetsSummaryFullList(string Date, string TID, string AffiliateID)
        {
            string strMdD5 = hashMD5.Hash_HashUniversalBetsAuth(Date, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "Stats/BetsSummary/?&Date=" + Date + "&TID=" + TID + "&Hash=" + strMdD5 + "&AffiliateID=" + AffiliateID, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// State bets
        /// Hash: Stats/Bets/[IP]/[TID]/[KEY]/[DATE]/[PWD]
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="TID"></param>
        /// <param name="system"></param>
        /// <param name="AffiliateID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiStatsBetsyFullList(string Date, string TID, string system, string login)
        {
            string strMdD5 = hashMD5.Hash_HashStatsBetssAuth(TID, Date);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = new HttpResponseMessage();
                if (string.IsNullOrEmpty(login))
                {
                    response = (await client.GetAsync("System/Api/" + API_KEY + "/Stats/Bets/?&Date=" + Date + "&TID=" + TID + "&Hash=" + strMdD5 + "&System=" + system + "&Login=" + login));
                }
                else
                {
                    response = (await client.GetAsync("System/Api/" + API_KEY + "/Stats/Bets/?&Date=" + Date + "&TID=" + TID + "&Hash=" + strMdD5 + "&System=" + system));
                }
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Lấy chi tiết 
        /// Hash: Stats/Bets/[IP]/[TID]/[KEY]/[DATE]/[PWD]
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="TID"></param>
        /// <param name="system"></param>
        /// <param name="AffiliateID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiStatsDetailyFullList(string Date, string TID, string system, string AffiliateID)
        {
            string strMdD5 = hashMD5.Hash_HashDetailStatssAuth(TID, Date);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Stats/Detailed/?Date=" + Date + "&TID=" + TID + "&Hash=" + strMdD5 + "&System=" + system + "&AffiliateID=" + AffiliateID, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Lấy chi tiết về game
        /// Hash: Stats/GameDetails/[IP]/[TID]/ /[KEY]/[GAMEID]/[PWD]
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="TID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiGameDetail(string gameID, string TID)
        {
            string strMdD5 = hashMD5.Hash_HashGameDetail(TID, gameID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Stats/GameDetails/?&GameID=" + gameID + "&TID=" + TID + "&Hash=" + strMdD5, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Thanh toan khach hang
        /// Hash: Loyalty/Payment/[IP]/[TID]/[KEY]/[SYSTEM]/[AMOUNT]/[LOGIN]/[PWD]
        /// </summary>
        /// <param name="login"></param>
        /// <param name="system">998/993</param>
        /// <param name="amount"></param>
        /// <param name="TID"></param>
        /// <param name="userIP"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiLoyaltyPayments(string login, string system, string amount, string TID, string userIP)
        {
            string strMdD5 = hashMD5.HashLoyaltypayments(system, amount, login, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Loyalty/Payment?&Login=" + login+"&System="+system+"&Amount="+amount + "&TID=" + TID + "&Hash=" + strMdD5+"&UserIP="+userIP, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// [STATUS] – Status of bonus:
        //0 - Disable bonus,
        //1 - Select bonus,
        //2 - Take inventory bonus
        /// </summary>
        /// <param name="bonusID"></param>
        /// <param name="status"></param>
        /// <param name="login"></param>
        /// <param name="TID"></param>
        /// <param name="userIP"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiBonusSelect(string bonusID, string status, string login, string TID, string userIP)
        {
            string strMdD5 = hashMD5.HashBonusSelect_BonusCancel(login, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/WLCInfo/Bonus/Select?&IDBonus=" + bonusID + "&Status=" + status + "&Login=" + login + "&TID=" + TID + "&Hash=" + strMdD5 + "&UserIP=" + userIP, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Bonus Cancel
        /// dung chung hash MD5 voi bonus select
        /// </summary>
        /// <param name="bonusID"></param>
        /// <param name="login"></param>
        /// <param name="TID"></param>
        /// <param name="userIP">IP address of user</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiBonusCancel(string bonusID, string login, string TID, string userIP)
        {
            string strMdD5 = hashMD5.HashBonusSelect_BonusCancel(login, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/WLCInfo/Bonus/Select?&IDBonus=" + bonusID + "&Login=" + login + "&TID=" + TID + "&Hash=" + strMdD5 + "&UserIP=" + userIP, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        #region Error handling
        /// <summary>
        /// Trả về kết quả được yêu cầu thực thi trước đó
        /// Server respone
        /// The original response, if found
        ///NOT_FOUND, if not found
        ///KEY_ERROR, if [KEY] is invalid
        ///IP_ERROR, if requesting IP is not allowed for [KEY]
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiRetrieveResponse(string TID)
        {
            //string strMdD5 = hashMD5.HashBonusSelect_BonusCancel(login, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Response/Get?&TID=" + TID, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// Trả về thông tin được thực thi trước đó
        /// Server respone
        /// 1. Request (Balance/Get, Balance/Set, etc.)
        /// 2. Arguments(?&=....)
        /// 3. Response code
        /// 4. Full response
        /// 5. Date and time of request
        /// 6. IP of request
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> ApiRetrieveInfor(string TID)
        {
            //string strMdD5 = hashMD5.HashBonusSelect_BonusCancel(login, TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Response/Info?&TID=" + TID, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        #endregion
        #region Providers additional info
        public async System.Threading.Tasks.Task<string> ApiLive_providers_tableinfo(string TID, string provider)
        {
            string strMdD5 = hashMD5.HashTableInfo(TID);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API_FUNDIST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = (await client.PostAsync("System/Api/" + API_KEY + "/Providers/TablesInfo/?&Provider="+provider+"&TID=" + TID+"&Hash="+strMdD5, null));
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        #endregion
    }

    public static class CommonCalculate
    {
        public static string GetTimeStamp()
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");//{"Bangkok", "SE Asia Standard Time"}
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, cstZone).ToString("yyyyMMddHHmmssffff");
        }
        public static DateTime ConvertUTC7()
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");//{"Bangkok", "SE Asia Standard Time"}
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, cstZone);
        }
    }
    public static class ConvertTimestampToDate
    {
        public static DateTime ConvertCustomDate(string timestamp)
        {
            int year = int.Parse(timestamp.Substring(0, 4));
            int month = int.Parse(timestamp.Substring(4, 2));
            int day = int.Parse(timestamp.Substring(6, 2));
            int hour = int.Parse(timestamp.Substring(8, 2));
            int minute = int.Parse(timestamp.Substring(10, 2));
            int second = int.Parse(timestamp.Substring(12, 2));
            DateTime dt = new DateTime(year, month, day, hour, minute, second, 0);
            return dt;
        }
    }
    #region get Ip website
    public static class IPClient
    {
        public static string getIP()
        {
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            else
                ip = ip.Split(',')[0];

            return ip;
        }
    }
    #endregion
}