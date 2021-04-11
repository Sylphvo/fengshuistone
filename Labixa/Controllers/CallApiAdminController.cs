using Labixa.Common;
using log4net;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Controllers
{
    public class CallApiAdminController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(CallApiAdminController));
        private string DOMAIN_API_ADMIN = System.Configuration.ConfigurationManager.AppSettings["domain_api"];
        private UserManager<User> _userManager;
        private readonly ISDKApiFundist _sdkApiFundist;
        private readonly ISDKApiAdmin _sdkApiAdmin;
        private readonly ITwoFactAuthAdmin _twoFactAuth;
        public CallApiAdminController(ISDKApiFundist sDKApiFundist, ISDKApiAdmin sDKApiAdmin, UserManager<User> userManager, ITwoFactAuthAdmin twoFactAuth)
        {
            _userManager = userManager;
            this._sdkApiFundist = sDKApiFundist;
            this._sdkApiAdmin = sDKApiAdmin;
            _twoFactAuth = twoFactAuth;
        }

        public CallApiAdminController()
        {
        }

        //
        // GET: /CallApiAdmin/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Withdraw ETH
        /// </summary>
        /// <param name="type">3</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="toAddress"></param>
        /// <param name="amount"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> SendWithdrawETH(int type, string username, string password, string toAddress, string amount, int speed, string fee)
        {
            try
            {
                var timeStamp = CommonCalculate.GetTimeStamp();
                var killFundist = _sdkApiFundist.ApiKill_Authorization(username, timeStamp);
                var _user = _userManager.FindByName(username);
                var obj = new { type = type, username = username, password = password, toaddress = toAddress, amount = amount, speed = speed, timestamp = timeStamp };
                var CalcApiTransaction = await _sdkApiAdmin.SDKApiTransactionPostAsync(DOMAIN_API_ADMIN+"api/transaction/", obj);//gọi đến api type = 3 đê
                var res = JsonConvert.DeserializeObject<MessageResponeApiAdmin>(CalcApiTransaction.ToString());
                string[] arrStr = res.Message.Split('_');//arrstr[2]: balance giao dịch, arrstr[1]:id transaction, arrstr[3]: phí transaction, arrstr[4]: ngày tạo
                if(res.Code == 1){
                    var sendMailRes = await _sdkApiAdmin.SDKApiSendMailWidthdrawETHAsync(_user.Id, _user.Email, arrStr[2], toAddress, arrStr[3], _user.UserName, timeStamp, arrStr[1], 2);
                }
                log.Info("Success withdraw");
                return Json(arrStr[0], JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info("Error exception withdraw" + ex);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> SendExchange(int type, string username, string password, string amount)
        {
            try
            {
                var timeStamp = CommonCalculate.GetTimeStamp();
                var killFundist = _sdkApiFundist.ApiKill_Authorization(username, timeStamp);
                var obj = new { type = type, username = username, password = password, amount = amount, toaddress = "", timestamp = "", speed = 0 };//type 1
                var CalcApiTransaction = await _sdkApiAdmin.SDKApiTransactionPostAsync(DOMAIN_API_ADMIN+"api/transaction/", obj);
                var converRes = JsonConvert.DeserializeObject<MessageResponeApiAdmin>(CalcApiTransaction);
                log.Info("Success exchange" + CalcApiTransaction);
                return Json(converRes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info("Error exception SendExchange" + ex);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
           
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> SendWidthdrawVIP(int type, string usernameFrom, string password,string UsernameToAddress ,string amount, string fee, string email, string id)
        {
            try
            {
                var timeStamp = CommonCalculate.GetTimeStamp();
                var killFundist = _sdkApiFundist.ApiKill_Authorization(usernameFrom, timeStamp);
                var obj = new { type = type, username = usernameFrom, password = password,toaddress=UsernameToAddress ,amount = amount, timestamp= timeStamp, speed = 0 };//type 5
                //var _user = _userManager.FindByName(UsernameToAddress);//kiểm tra db web có tồn tại username này không
                //if(_user == null)
                //{
                //    return Json(new { Message = "User name from invalided. Please check again", status = "Failed" });
                //}
                var CalcApiTransaction = await _sdkApiAdmin.SDKApiTransactionPostAsync(DOMAIN_API_ADMIN + "api/transaction/", obj);
                var res = JsonConvert.DeserializeObject<MessageResponeApiAdmin>(CalcApiTransaction);
                string[] arrStr = res.Message.Split('_');
                if(res.Code == 1)
                {
                    var mailRes = await _sdkApiAdmin.SDKApiSendMailWidthdrawVIPAsync(id, email, arrStr[2], UsernameToAddress, arrStr[3],usernameFrom, timeStamp, arrStr[1], 3);
                }
                log.Info("Success exchange" + CalcApiTransaction);
                return Json(arrStr[0], JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info("Error exception SendExchange" + ex);
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult Validate_2fa(string username, string pinCode)
        {
            try
            {
                log.Info("Start validate 2FA");
                var result = _twoFactAuth.ValidateTwoFactorPIN_2FA(username, pinCode);
                if (result == true)
                {
                    log.Info("validate 2FA success");
                    return Json(new { Message = "Validate success!!", status = "Success" });

                }
                else {
                    log.Info("validate 2FA failed");
                    return Json(new { Message = "Validate unsuccessful", status = "Failed" });
                } 

            }
            catch (Exception ex)
            {
                log.Info("Error exception: " + ex);
                return Json(new { Message = "Validate unsuccessful", status = "Failed" });
            }
        }
        #region call api get transaction max balance can widthdraw
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> WidthdrawMaxETH(string username)
        {
            var checkMaxETH = await _sdkApiAdmin.SDKApiMaxWidthdrawETH(DOMAIN_API_ADMIN, username);
            return Json(checkMaxETH, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> WidthdrawMaxVIP(string username)
        {
            var checkMaxETH = await _sdkApiAdmin.SDKApiMaxWidthdrawVIP(DOMAIN_API_ADMIN, username);
            return Json(checkMaxETH, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region check giới hạn của player
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> CheckPlayerLimitedAsync(string username)
        {
            var ResCheck = await _sdkApiAdmin.GetCheckPlayerLimited(DOMAIN_API_ADMIN, username);
            return Json(ResCheck, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
    public class ObjAfterExchangeETH
    {

    }
}