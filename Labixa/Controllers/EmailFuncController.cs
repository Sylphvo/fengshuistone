using Labixa.Common;
using Labixa.Models;
using log4net;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Controllers
{
    public class EmailFuncController : BaseHomeController
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private UserManager<User> _userManager;
        private readonly ITwoFactAuthAdmin _twoAuthFact;
        private readonly IConfirmEmail _confirmMail;
        private string DOMAIN_API_ADMIN = System.Configuration.ConfigurationManager.AppSettings["domain_api"];

        public EmailFuncController(UserManager<User> userManager, ITwoFactAuthAdmin twoAuthFact, IConfirmEmail confirmEmail)
        {
            _userManager = userManager;
            _twoAuthFact = twoAuthFact;
            _confirmMail = confirmEmail;
            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        //
        // GET: /EmailFunc/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult sendMailConfirm(string message)
        {

            MessageResponeApi mess = JsonConvert.DeserializeObject<MessageResponeApi>(message);
           
            return View(model: mess);
        }
        //dùng để xác nhận email transaction gọi về action này
        #region để link confirm gọi đến thực hiện giao dịch withdrawETH
        public async System.Threading.Tasks.Task<ActionResult> ConfirmTransactinMail(string token)//mail xác nhận giao dịch cho ETH
        {
            string message = "Transaction timeout. Please contact Admin via mail to supported";
            MessageResponeApiAdmin messageRespone = new MessageResponeApiAdmin()
            {
                Message = "Transaction requested timeout. Please contact Admin via mail to supported",
                Code = 1
            };
            string[] arrStr = token.Split('_');//tách chuỗi token thành timestamp và userId
            var user = _userManager.FindById(arrStr[1].ToString());
            var resultDuration = DateTime.Now - ConvertTimestampToDate.ConvertCustomDate(arrStr[0]);
            if (resultDuration.TotalMinutes <= 30)
            {
                message = "Transaction ETH performed successful";
                var arrString = await PerformedTransactionAsync(user.UserName, arrStr[0]);//gọi type 6 để thực hiện giao dịch
                var resApi = JsonConvert.DeserializeObject<MessageResponeApiAdmin>(arrString);
                if (resApi.Code == 1)
                {
                    messageRespone.Message = resApi.Message;
                    messageRespone.Code = resApi.Code;
                    return View(messageRespone);
                }
                else
                {
                    messageRespone.Message = resApi.Message;
                    messageRespone.Code = resApi.Code;
                    return View(messageRespone);
                }
            }
            return View(messageRespone);
        }
        //khi click mail dùng action này để đón nhận và thực hiện giao dịch
        public async System.Threading.Tasks.Task<string> PerformedTransactionAsync(string Username, string Timestamp)
        {
            //var obj = new { type = Type, username = userName, timestamp = timeStamp };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DOMAIN_API_ADMIN+"api/transaction/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                //end truyển tham số vào body tại đây
                var jsonObject = new { type = 6, username = Username, timestamp = Timestamp, password = "", amount = 0, toaddress = "", speed = 0 };//gọi api type 6 để thực hiện giao dịch
                //end truyển tham số vào body tại đây
                var myContent = JsonConvert.SerializeObject(jsonObject);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(DOMAIN_API_ADMIN+"api/transaction/", byteContent);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            return null;
        }
        #endregion
        #region để link confirm gọi đến thực hiện giao dịch withdrawVIP
        public async System.Threading.Tasks.Task<ActionResult> ConfirmTransactionMail_VIP(string token)//mail xác nhận giao dịch cho VIP
        {
            MessageResponeApiAdmin messageRespone = new MessageResponeApiAdmin()
            {
                Message = "Transaction requested timeout. Please contact Admin via mail to supported",
                Code = 1
            };
            string[] arrStr = token.Split('_');//tách chuỗi token thành timestamp và userId
            var user = _userManager.FindById(arrStr[1].ToString());
            var resultDuration = DateTime.Now - ConvertTimestampToDate.ConvertCustomDate(arrStr[0]);
            if (resultDuration.TotalMinutes <= 30)
            {
                var arrString = await PerformedTransactionVIPAsync(user.UserName, arrStr[0]);//gọi type 7 để thực hiện giao dịch
                var resApi = JsonConvert.DeserializeObject<MessageResponeApiAdmin>(arrString);
                if (resApi.Code == 1)
                {
                    messageRespone.Message = resApi.Message;
                    messageRespone.Code = resApi.Code;
                    return View(messageRespone);
                }
                else
                {
                    messageRespone.Message = resApi.Message;
                    messageRespone.Code = resApi.Code;
                    return View(messageRespone);
                }
            }
            return View(model: messageRespone);
        }
        //khi click mail dùng action này để đón nhận và thực hiện giao dịch
        public async System.Threading.Tasks.Task<string> PerformedTransactionVIPAsync(string Username, string Timestamp)
        {
            //var obj = new { type = Type, username = userName, timestamp = timeStamp };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DOMAIN_API_ADMIN+"transaction/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                //end truyển tham số vào body tại đây
                var jsonObject = new { type = 7, username = Username, timestamp = Timestamp, password="", amount=0, toaddress="", speed=0 };//gọi api type 7 để thực hiện giao dịch VIP to VIP
                //end truyển tham số vào body tại đây
                var myContent = JsonConvert.SerializeObject(jsonObject);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(DOMAIN_API_ADMIN+"api/transaction/", byteContent);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            return null;
        }
        #endregion
        //dùng để xác nhận email gọi về action này
        public ActionResult ConfirmMail(string token)
        {
            try
            {
                log.Info("Confirm Respone Email Success");
                var user = _userManager.FindById(token);
                user.EmailConfirmed = true;
                _userManager.Update(user);
                //return View(user);
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                RedirectToAction("Index", "Error Message");
                throw;
            }

        }
        public ActionResult Confirm2FA(string username)
        {
            var _user = _userManager.FindByName(username);
            return View(_user);
        }
        #region forgot password
        public ActionResult CheckMailTimeout(string tokenId, string timestamp)
        {
            var resultDuration = CommonCalculate.ConvertUTC7() - ConvertTimestampToDate.ConvertCustomDate(timestamp);//lấy ngày hiện tại qua hàm ConverUTC7
            if (resultDuration.TotalMinutes <= 30)//kiểm tra thời gian timeout của mail
            {
                return RedirectToAction("FormInputForgotPassword", "EmailFunc", new { tokenId = tokenId });
            }
            return View(model:"Password verification time out !");
        }
        public ActionResult FormInputForgotPassword(string tokenId)
        {
            ViewBag.tokenId = tokenId;
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId">chính là userId</param>
        /// <param name="tokenCode">đoạn mã code do .net auto generate ra</param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ConfirmForgotPassAsync(FormCollection frmCollect)
        {
            if (frmCollect["tokenId"] == null)
            {
                return View(model:"Email reset password invalided !!!");
            }
            var user = _userManager.FindById(frmCollect["tokenId"]);
            if(user != null)
            {
                var pass = frmCollect["password"];
                string compare = user.Temp2;
                if (user.Temp2 == null)
                {
                    compare = user.PasswordNotHash;
                }
                var result = await _userManager.ChangePasswordAsync(user.Id, compare, pass);
                user.Temp2 = pass;
                _userManager.Update(user);
                //var result = await _userManager.ConfirmEmailAsync(frmCollect["tokenId"], frmCollect["tokenCode"]);
                if (result.Succeeded)
                {
                    return View(model: "Password updated successfull!!.");
                }
                return View(model: result.Errors);
            }
            else
            {
                return View(model: "Player not exists. Please check, thank you !!!");
            }
        }
        #endregion
        #region email confirm transaction deposit
        //public ActionResult ConfirmTrasactionDeposit(string username, string amount, int type)
        //{
        //    var user = _userManager.FindByName(username);
        //    //var resMail = _confirmMail.ConfirmEmailDeposit("nghiiatran1998@gmail.com", username, amount, type, Server);// user.Email
        //    var resMail = _confirmMail.ConfirmEmailDeposit(user.Email, username, amount, type, Server);// user.Email
        //    return View(model:resMail);
        //}
        #endregion
        #region get ip client
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> GetIPClient(string username)
        {
            var ip = IPClient.getIP();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DOMAIN_API_ADMIN);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpResponseMessage response = await client.GetAsync(DOMAIN_API_ADMIN +"api/Account?UserIP="+ip+"&username="+username);
                if (response.IsSuccessStatusCode)
                {
                    return Json(response.Content.ReadAsStringAsync().Result);
                }
            }
            return Json(ip);
        }
        #endregion
        [HttpPost]
        public ActionResult Submit2FA(string username, string pinCode)
        {
            var resultValidate = _twoAuthFact.ValidateTwoFactorPIN_2FA(username, pinCode);
            if (resultValidate == false)
            {
                ModelState.AddModelError("", "Validate invalid. Please confirm again");
                return RedirectToAction("Confirm2FA", "EmailFunc", new { username=username});
            }
            else
            {
                var _user = _userManager.FindByName(username);
                //cập nhật ip vào db web
                _user.IP_login = IPClient.getIP();
                _user.TwoFactorEnabled = true;
                _userManager.Update(_user);
                ModelState.AddModelError("", "Validate success. Please login");
                return RedirectToAction("LoginAferConfrim_2faAsync", "Account", new { username = _user.UserName});
            }
        }
	}
}