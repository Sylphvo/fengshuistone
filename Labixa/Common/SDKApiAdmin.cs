using Labixa.HashMD;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Labixa.Common
{
    public interface ISDKApiAdmin
    {
        System.Threading.Tasks.Task<string> SDKApiAdminPostAsync(string UriServer, object jsonObject);
        System.Threading.Tasks.Task<string> SDKApiTransactionGetAsync(string UriServer, string userName);//type 1
        System.Threading.Tasks.Task<string> SDKApiSystemBetGetAsync(string UriServer, string userName);
        System.Threading.Tasks.Task<string> SDKApiAffiliateGet(string UriServer, string userName);
        System.Threading.Tasks.Task<string> SDKApiTransactionPostAsync(string UriServer, object userName);
        System.Threading.Tasks.Task<string> SDKApiSendMailWidthdrawETHAsync(string id, string email, string amount, string addressTo, string fee, string username, string timestamp, string idTransaction, int type);//send mail widthdraw ETH
        System.Threading.Tasks.Task<string> SDKApiSendMailWidthdrawVIPAsync(string id, string email, string amount, string userTo, string fee, string username, string timestamp, string idTransaction, int type);//send mail widthdraw ETH
        System.Threading.Tasks.Task<string> TransactionPostAsync(string UriServer, string username, string timestamp);
        System.Threading.Tasks.Task<double> GetTotalBet(string UriServer, string username);
        System.Threading.Tasks.Task<double> GetSytemBet (string UriServer, string username);
        #region History Deposit 
        System.Threading.Tasks.Task<string> History_Deposit_Currency_ETH(string username);
        System.Threading.Tasks.Task<string> History_Deposit_Currency_BIT(string username);
        System.Threading.Tasks.Task<string> History_Deposit_New_ETH(string username);
        System.Threading.Tasks.Task<string> History_Deposit_New_BIT(string username);

        #endregion
        #region History Exchange 
        System.Threading.Tasks.Task<string> History_Exchange_Currency_ETH(string username);
        System.Threading.Tasks.Task<string> History_Exchange_Currency_BIT(string username);
        System.Threading.Tasks.Task<string> History_Exchange_New_ETH(string username);
        System.Threading.Tasks.Task<string> History_Exchange_New_BIT(string username);

        #endregion
        #region History Withdraw 
        System.Threading.Tasks.Task<string> History_Withdraw_Currency_ETH(string username);
        System.Threading.Tasks.Task<string> History_Withdraw_Currency_BIT(string username);
        System.Threading.Tasks.Task<string> History_Withdraw_New_ETH(string username);
        System.Threading.Tasks.Task<string> History_Withdraw_New_BIT(string username);

        #endregion
        Task<double> SDKApiMaxWidthdrawETH(string UriServer, string username);
        Task<double> SDKApiMaxWidthdrawVIP(string UriServer, string username);
        System.Threading.Tasks.Task<string> GetCheckPlayerLimited(string UriServer, string username);
    }
    public class SDKApiAdmin:ISDKApiAdmin
    {
        private string url_mail = ConfigurationManager.AppSettings["domain_mail"].ToString();
        private string url_prod = ConfigurationManager.AppSettings["domain_api"].ToString();
        private string url_cloud = ConfigurationManager.AppSettings["domain_cloud"].ToString();

        #region History Deposit ETH , BIT [Currency,New] 
        public async Task<string> History_Deposit_Currency_ETH(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_prod);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/TransactionHistory?username=" + username + "&type=1&transaction=deposit";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        public async Task<string> History_Deposit_Currency_BIT(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_prod);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/TransactionHistory?username=" + username + "&type=2&transaction=deposit";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        public async Task<string> History_Deposit_New_ETH(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_cloud);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url ="api/History-Trans/get?username=" + username + "&type=1&transaction=deposit";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        
        public async Task<string> History_Deposit_New_BIT(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_cloud);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/History-Trans/get?username=" + username + "&type=2&transaction=deposit";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        #endregion
        #region History Exchange ETH , BIT [Currency,New] 
        public async Task<string> History_Exchange_Currency_ETH(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_prod);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/TransactionHistory?username=" + username + "&type=1&transaction=exchance";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        public async Task<string> History_Exchange_Currency_BIT(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_prod);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/TransactionHistory?username=" + username + "&type=2&transaction=exchance";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        public async Task<string> History_Exchange_New_ETH(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_cloud);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/History-Trans/get?username=" + username + "&type=1&transaction=exchange";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }

        public async Task<string> History_Exchange_New_BIT(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_cloud);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/History-Trans/get?username=" + username + "&type=2&transaction=exchange";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        #endregion
        #region History Withdraw ETH , BIT [Currency,New] 
        public async Task<string> History_Withdraw_Currency_ETH(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_prod);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/TransactionHistory?username=" + username + "&type=1&transaction=withdraw";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        public async Task<string> History_Withdraw_Currency_BIT(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_prod);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/TransactionHistory?username=" + username + "&type=2&transaction=withdraw";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        public async Task<string> History_Withdraw_New_ETH(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_cloud);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/History-Trans/get?username=" + username + "&type=1&transaction=withdraw";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }

        public async Task<string> History_Withdraw_New_BIT(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_cloud);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var url = "api/History-Trans/get?username=" + username + "&type=2&transaction=withdraw";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        #endregion
        public async Task<double> GetSytemBet(string UriServer, string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var _url = UriServer + "username=" + username + "&type=3";
                HttpResponseMessage response = await client.GetAsync(_url);
                if (response.IsSuccessStatusCode)
                {
                    return double.Parse(response.Content.ReadAsStringAsync().Result);
                }
                return 0.00;
            }
        }

        public async Task<double> GetTotalBet(string UriServer, string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var _url = UriServer + "username=" + username + "&type=" + 2;
                HttpResponseMessage response = await client.GetAsync(_url);
                if (response.IsSuccessStatusCode)
                {
                    return double.Parse(response.Content.ReadAsStringAsync().Result);
                }
                return 0.00;
            }
        }

        /// <summary>
        /// Post lên api của admin
        /// </summary>
        /// <param name="UriServer"></param>
        /// <param name="JsonObject"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> SDKApiAdminPostAsync(string UriServer, object jsonObject)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                //end truyển tham số vào body tại đây
                //var jsonObject = new { username = username, password=pass, usernameParent=affiliate };
                //end truyển tham số vào body tại đây
                var myContent = JsonConvert.SerializeObject(jsonObject);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(UriServer, byteContent);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }

        public async Task<string> SDKApiAffiliateGet(string UriServer, string userName)
        {
            var timeStamp = CommonCalculate.GetTimeStamp();
            string str = timeStamp + "" + userName + "MLKEf6r6xQVTDK";//TID+userName+MLKEf6r6xQVTDK
            HashMD5 md5 = new HashMD5();
            string strHash = md5.MD5Hash(str);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(UriServer + userName + "&type=" + 1);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        /// <summary>
        /// hàm tính systembet có thật
        /// </summary>
        /// <param name="UriServer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> SDKApiSystemBetGetAsync(string UriServer, string userName)
        {
            var timeStamp = CommonCalculate.GetTimeStamp();
            string str = timeStamp + "" + userName + "MLKEf6r6xQVTDK";//TID+userName+MLKEf6r6xQVTDK
            HashMD5 md5 = new HashMD5();
            string strHash = md5.MD5Hash(str);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(UriServer + "?type=3&username=" + userName + "&tid=" + timeStamp + "&hash=" + strHash);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }

        /// <summary>
        /// Get transaction từ api admin
        /// Default: type = 1
        /// </summary>
        /// <param name="UriServer"></param>
        /// <param name="userName"></param>
        /// <param name="tid">timeStamp</param>
        /// <param name="hash">tid+userName</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> SDKApiTransactionGetAsync(string UriServer, string userName)
        {
            var timeStamp = CommonCalculate.GetTimeStamp();
            string str = timeStamp + "" + userName + "MLKEf6r6xQVTDK";//TID+userName+MLKEf6r6xQVTDK
            HashMD5 md5 = new HashMD5();
            string strHash = md5.MD5Hash(str);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(UriServer + "?type=1&username=" + userName);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        //public async System.Threading.Tasks.Task<string> SDKApiTransactionGetAsyncType3(string UriServer, string userName)
        //{
        //    var timeStamp = CommonCalculate.GetTimeStamp();
        //    string str = timeStamp + "" + userName + "MLKEf6r6xQVTDK";//TID+userName+MLKEf6r6xQVTDK
        //    HashMD5 md5 = new HashMD5();
        //    string strHash = md5.MD5Hash(str);
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(UriServer);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        //        HttpResponseMessage response = await client.GetAsync(UriServer + "?type=3&username=" + userName + "&tid=" + timeStamp + "&hash=" + strHash);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return response.Content.ReadAsStringAsync().Result;
        //        }
        //        return null;
        //    }
        //}
        public async System.Threading.Tasks.Task<string> SDKApiTransactionPostAsync(string UriServer, object obj)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                //end truyển tham số vào body tại đây
                //var jsonObject = new { username = username, password=pass, usernameParent=affiliate };
                //end truyển tham số vào body tại đây
                var myContent = JsonConvert.SerializeObject(obj);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(UriServer, byteContent);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        public async Task<string> TransactionPostAsync(string UriServer, string Username, string Timestamp)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                //end truyển tham số vào body tại đây
                var jsonObject = new { type = 6, username = Username, timestamp= Timestamp };
                //end truyển tham số vào body tại đây
                var myContent = JsonConvert.SerializeObject(jsonObject);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(UriServer, byteContent);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        #region send mail widthdraw ETH
        public async System.Threading.Tasks.Task<string> SDKApiSendMailWidthdrawETHAsync(string id, string email, string amount, string addressTo, string fee, string username, string timestamp, string idTransaction, int type)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_mail);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(url_mail+ "WebCallMail/MailTransactionETH" + "?id="+id+"&email="+email+"&timestamp="+timestamp+"&Amount="+amount+"&AddressTo="+addressTo+"&FeeTransaction="+fee+"&username="+username+"&idTransaction="+idTransaction+"&type="+type);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }

        #endregion
        #region send mail widthdraw VIP
        public async System.Threading.Tasks.Task<string> SDKApiSendMailWidthdrawVIPAsync(string id, string email, string amount, string userTo, string fee, string username, string timestamp, string idTransaction, int type)//send mail widthdraw VIP
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url_mail);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(url_mail+ "WebCallMail/MailTransactionVIP" + "?id=" + id + "&email=" + email + "&timestamp=" + timestamp + "&Amount=" + amount + "&AddressTo=" + userTo + "&FeeTransaction=" + fee + "&username=" + username+"&idTransaction="+idTransaction+"&type="+type);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }

        #endregion
        #region get call api transaction max widthdraw
        public async Task<double> SDKApiMaxWidthdrawETH(string UriServer, string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(UriServer + "api/transaction?username=" + username + "&curency=eth");
                if (response.IsSuccessStatusCode)
                {
                    return double.Parse(response.Content.ReadAsStringAsync().Result);
                }
                return 0;
            }
        }
        public async Task<double> SDKApiMaxWidthdrawVIP(string UriServer, string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(UriServer + "api/transaction?username=" + username + "&curency=vip");
                if (response.IsSuccessStatusCode)
                {
                    return double.Parse(response.Content.ReadAsStringAsync().Result);
                }
                return 0;
            }
        }
        #endregion
        #region check player limited
        public async Task<string> GetCheckPlayerLimited(string UriServer, string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(UriServer);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(UriServer + "api/transaction/CheckUserIsLimit?username=" + username);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }
        #endregion
    }

}