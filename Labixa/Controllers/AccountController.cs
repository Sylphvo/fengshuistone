using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Labixa.Models;
using Outsourcing.Data.Models;
using Outsourcing.Data;
using System.Net;
using System.Net.Configuration;
using Labixa.Common;
using log4net;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using log4net.Config;
using System.IO;
using System.Windows.Forms;

namespace Labixa.Controllers
{
    //[Authorize]
    public class AccountController : BaseHomeController
    {
        private string SERVER_IP = System.Configuration.ConfigurationManager.AppSettings["server_ip"];
        private string PARENT_ID = System.Configuration.ConfigurationManager.AppSettings["parent_id"];
        private string DOMAIN_API = System.Configuration.ConfigurationManager.AppSettings["domain_api"];

        private UserManager<User> _userManager;
        private readonly ISDKApiFundist _sdkApiFundits;
        private readonly ISDKApiAdmin _sdkApiAdmin;
        private readonly ITwoFactAuthAdmin _twoFact;
        private readonly IConfirmEmail _confirmEmail;
        private readonly IValidateCapcha _captcha;
        //private IUserRoleStore<User> _userRoleManager;

        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AccountController(UserManager<User> userManager, ISDKApiFundist sDKApiFundist, ISDKApiAdmin sDKApiAdmin, ITwoFactAuthAdmin twoFact, IConfirmEmail confirmEmail, IValidateCapcha captcha)
        {
            _userManager = userManager;
            _sdkApiFundits = sDKApiFundist;
            _sdkApiAdmin = sDKApiAdmin;
            _twoFact = twoFact;
            _confirmEmail = confirmEmail;
            _confirmEmail = confirmEmail;
            _captcha = captcha;
            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        //public ActionResult AddRole()
        //{
        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new OutsourcingEntities()));
        //    var result = roleManager.Create(new IdentityRole("Customer"));
        //    if (result.Succeeded)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        //    }
        //}
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var ip = IPClient.getIP();//lấy ip local
            AuthenticationManager.SignOut();
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AccountDashboard", "Home");
            }
            log.Info("tui là tui");
            //_userManager.ChangePassword("c21a1169-1e5d-4aa7-9604-1595514608bc", "123456", "labixa@123");
            ViewBag.ReturnUrl = returnUrl;
            //HttpCookie cookie1 = Request.Cookies["mimosa"];
            Response.Cookies["mimosa"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["mamosi"].Expires = DateTime.Now.AddDays(-1);
            //if (cookie1 != null)
            //{
            //    cookie1.Value = null;
            //}
            //HttpCookie cookie2 = Request.Cookies["mamosi"];
            //if(cookie2 != null)
            //{
            //    cookie2.Value = null;
            //}
            Request.Cookies.Remove("mimosa");//QRIMage
            Request.Cookies.Remove("mamosi");//QRText
                                             //Response.Cookies.Clear()
            return View();
            //return RedirectToAction("Index", "ErrorMessage");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip = IPClient.getIP();//lấy ip local

                    //if (ip == "115.73.214.11")
                    //{

                    //}
                    //else
                    //{
                    //    return RedirectToAction("Index", "ErrorMessage");
                    //}
                    ModelState.AddModelError("", message);
                    var user = await _userManager.FindAsync(model.UserName.ToLower(), model.Password);
                    if (user != null)
                    {
                        if (user.Activated == true)
                        {
                            //kiểm tra xác thực email
                            if (user.EmailConfirmed == true)
                            {
                                //Nếu chứa xác thực thì login bình thường
                                if (user.TwoFactorEnabled == false)
                                {
                                    Session["User"] = user;
                                    Session["UserName"] = user.UserName.ToLower();
                                    Session["QRImage"] = user.Temp;
                                    Session["QRText"] = user.Temp1;
                                    HttpCookie qrImage = Request.Cookies["mimosa"];
                                    HttpCookie qrText = Request.Cookies["mamosi"];
                                    if (qrImage != null && qrText != null)
                                    {
                                        //Request.Cookies.Remove("mimosa");
                                        //Request.Cookies.Remove("mamosi");
                                        qrImage.Value = null;
                                        qrText.Value = null;

                                        qrImage.Value = user.Temp;
                                        qrImage.Expires = DateTime.Now.AddDays(30);
                                        qrText.Value = user.Temp1;
                                        qrText.Expires = DateTime.Now.AddDays(30);
                                        Request.Cookies.Add(qrImage);
                                        Request.Cookies.Add(qrText);
                                    }
                                    else
                                    {
                                        qrImage = new HttpCookie("mimosa", user.Temp);
                                        qrImage.Expires = DateTime.Now.AddDays(1);
                                        Response.SetCookie(qrImage);

                                        qrText = new HttpCookie("mamosi", user.Temp1);
                                        qrText.Expires = DateTime.Now.AddDays(1);
                                        Response.SetCookie(qrText);
                                    }
                                    log.Info(Session["QRText"]);

                                    await SignInAsync(user, model.RememberMe);
                                    //lưu ip login vào db Website kiểm tra login
                                    user.IP_login = ip;
                                    _userManager.Update(user);
                                    return RedirectToAction("AccountDashboard", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("Confirm2FA", "EmailFunc", new { username = user.UserName.ToLower() });
                                }
                            }
                            else ModelState.AddModelError("", "Please confirm email before login");
                        }
                        else
                        {
                            ModelState.AddModelError("", "User name or Password incorrect");
                        }
                    }
                    else ModelState.AddModelError("", "User name or Password incorrect");
                }

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "ErrorMessage");
            }

        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string affiliate)
        {
            Session["User"] = null;
            Session["UserName"] = null;
            Request.Cookies.Remove("mimosa");//QRIMage
            Request.Cookies.Remove("mamosi");//QRText
            Response.Cookies["mimosa"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["mamosi"].Expires = DateTime.Now.AddDays(-1);
            AuthenticationManager.SignOut();
            if (affiliate != null && affiliate != "")
            {
                ViewBag.Affiliate = affiliate;
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            CaptchaResponse response = _captcha.ValidateCaptcha(Request["g-recaptcha-response"]);
            try
            {
                if (ModelState.IsValid)
                {
                    //kiểm tra user name khi được nhập vào
                    var checkUser = _userManager.FindByName(model.UserName.ToLower());
                    //check mail
                    var checkEmailAddress = _userManager.FindByEmail(model.Email);
                    if (checkEmailAddress != null)// mail tồn tại không cho tạo tk
                    {
                        ModelState.AddModelError("", "Email existed");
                        //return RedirectToAction("Register", "Account");
                    }
                    else
                    {
                        //kiểm tra username có tồn tại trong db hay chưa, null là username chưa có -> có thể đăng ký được
                        if (checkUser == null)
                        {
                            //kiểm tra parent id
                            if (!string.IsNullOrEmpty(model.Affilate_ID))
                            {
                                var checkParentId = _userManager.FindByName(model.Affilate_ID);//chính là user name
                                if (checkParentId != null && checkParentId.EmailConfirmed == true)// tồn tại affilliate cho tạo tk + trùng mail thì không cho tạo tk
                                {
                                    //var checkParentId = _userManager.FindById(findUserParent.Id);
                                    //Neu parent exist thi them parent vao
                                    User userParent = new User()
                                    {
                                        UserName = model.UserName.ToLower().Replace(" ", string.Empty),
                                        Email = model.Email,
                                        ParentId = checkParentId.Id,
                                        PasswordNotHash = model.Password,
                                        Temp2 = model.Password,// để lưu khi đổi pass
                                        Activated = true//tạo mặc định để có thể lock hoặc unclock trong admin
                                    };
                                    var ip = IPClient.getIP();//chỉ dùng cho web golive
                                    if (ip.Equals("::1"))
                                    {
                                        log.Info("Ip localhost: " + ip + "_" + DateTime.Now);
                                        ip = SERVER_IP;
                                    }
                                    var CallApiFundist = await _sdkApiFundits.ApiCreateUser(model.UserName.ToLower().Replace(" ", string.Empty), model.Password, CommonCalculate.GetTimeStamp(), ip, model.UserName);//tạo account fundist
                                    if (CallApiFundist.Equals("1"))
                                    {
                                        var qrImage = _twoFact.GetQrCodeImage(model.UserName.ToLower().Replace(" ", string.Empty));
                                        var qrText = _twoFact.GetQrCodeText(model.UserName.ToLower().Replace(" ", string.Empty));
                                        userParent.Temp = qrImage;
                                        userParent.Temp1 = qrText;
                                        var resultParent = await _userManager.CreateAsync(userParent, model.Password);//tạo account trong db web
                                        var userId = _userManager.FindByName(model.UserName.ToLower().Replace(" ", string.Empty));//tìm username trong db web để lấy ra id của user
                                        log.Info("Start mail register");
                                        var resultSendMail = await _confirmEmail.CallMailRegister(CommonCalculate.GetTimeStamp(), model.UserName.ToLower().Replace(" ", string.Empty), 4);//gửi mail xác nhận đăng ký
                                                                                                                                                                                          //gọi api admin để tạo ví
                                        var jsonObject = new { username = userId.UserName, password = userId.PasswordNotHash, usernameParent = model.Affilate_ID };
                                        var CallApiAdmin = await _sdkApiAdmin.SDKApiAdminPostAsync(DOMAIN_API + "api/Account/", jsonObject);//gọi api tạo account admin
                                        JObject ApiAdminCall = JObject.Parse(CallApiAdmin);
                                        //tim username upate address, balance ETH, balance VIP
                                        var a = ApiAdminCall["UserName"].ToString();
                                        var findUseName = _userManager.FindByName(a);
                                        if (findUseName != null)
                                        {
                                            findUseName.Address_Crypto = ApiAdminCall["AdressKey"].ToString();
                                            findUseName.Balance_ETH = ApiAdminCall["Balance"].ToString();
                                            findUseName.Balance_VIP = ApiAdminCall["BalanceVip"].ToString();
                                            _userManager.Update(findUseName);
                                        }
                                        /////

                                        if (resultParent.Succeeded)
                                        {
                                            // await SignInAsync(userParent, isPersistent: false);//tắt auto đăng nhập khi đăng ký thành công
                                            return RedirectToAction("sendMailConfirm", "EmailFunc", new { message = resultSendMail });
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "Invalid: " + resultParent);
                                        }
                                    }
                                    else
                                    {
                                        log.Info("Error when call api fundist: " + CallApiFundist);
                                        ModelState.AddModelError("Invalid: ", CallApiFundist);
                                    }
                                }
                                else
                                {
                                    log.Info("Error invalid Affilate");
                                    //Thông báo lỗi parent
                                    ModelState.AddModelError("Parent invalid or email existed", "Affiliate ID does not exis !");
                                }
                            }
                            else
                            {
                                User userParent = new User()
                                {
                                    UserName = model.UserName.ToLower().Replace(" ", string.Empty),
                                    Email = model.Email,
                                    PasswordNotHash = model.Password,
                                    ParentId = PARENT_ID,
                                    Temp2 = model.Password,// để lưu khi đổi pass
                                    Activated = true//tạo mặc định để có thể lock hoặc unclock trong admin
                                };
                                var ip = IPClient.getIP();//chỉ dùng cho web golive
                                if (ip.Equals("::1"))
                                {
                                    ip = SERVER_IP;
                                }
                                var CallApiFundist = await _sdkApiFundits.ApiCreateUser(model.UserName.ToLower().Replace(" ", string.Empty), model.Password, CommonCalculate.GetTimeStamp(), ip, model.UserName);//gọi api fundist
                                                                                                                                                                                                                 //chưa check
                                if (CallApiFundist.Equals("1"))
                                {
                                    userParent.Temp = _twoFact.GetQrCodeImage(model.UserName.ToLower().Replace(" ", string.Empty));
                                    userParent.Temp1 = _twoFact.GetQrCodeText(model.UserName.ToLower().Replace(" ", string.Empty));
                                    var resultParent = await _userManager.CreateAsync(userParent, model.Password);//tạo account ở db web
                                    var userId = _userManager.FindByName(model.UserName.ToLower().Replace(" ", string.Empty));//tìm username trong db web để lấy ra id của user
                                    log.Info("Send mail");                                                     //Gửi mail xác nhận đăng ký
                                    var resultSendMail = await _confirmEmail.CallMailRegister(CommonCalculate.GetTimeStamp(), model.UserName.ToLower().Replace(" ", string.Empty), 4);//gửi mail xác nhận đăng ký

                                    var jsonObject = new { username = userId.UserName, password = userId.PasswordNotHash, usernameParent = "avataclub" };
                                    var CallApiAdmin = await _sdkApiAdmin.SDKApiAdminPostAsync(DOMAIN_API + "api/Account/", jsonObject);//tạo account trên admin
                                    log.Info("Account create api admin: " + CallApiAdmin);
                                    if (!string.IsNullOrEmpty(CallApiAdmin))
                                    {
                                        JObject ApiAdminCall = JObject.Parse(CallApiAdmin);
                                        //tim username upate address, balance ETH, balance VIP
                                        var a = ApiAdminCall["UserName"].ToString();
                                        var findUseName = _userManager.FindByName(a);
                                        if (findUseName != null)
                                        {
                                            findUseName.Address_Crypto = ApiAdminCall["AdressKey"].ToString();
                                            findUseName.Balance_ETH = ApiAdminCall["Balance"].ToString();
                                            findUseName.Balance_VIP = ApiAdminCall["BalanceVip"].ToString();
                                            _userManager.Update(findUseName);
                                        }
                                        if (resultParent.Succeeded)
                                        {
                                            //await SignInAsync(userParent, isPersistent: false);
                                            return RedirectToAction("sendMailConfirm", "EmailFunc", new { message = resultSendMail });
                                        }
                                        else
                                        {
                                            AddErrors(resultParent);
                                        }
                                    }
                                    else
                                    {
                                        log.Info("Can't create account from api admin - End");
                                        ModelState.AddModelError("", CallApiAdmin);
                                    }
                                }
                                else
                                {
                                    log.Info("Error when call api fundist: " + CallApiFundist);
                                    ModelState.AddModelError("Invalid: ", CallApiFundist);
                                }
                                //var resultParent = await _userManager.CreateAsync(userParent, model.Password);
                                //if (resultParent.Succeeded)
                                //{
                                //    await SignInAsync(userParent, isPersistent: false);
                                //    return RedirectToAction("Index", "Home");
                                //}
                                //else
                                //{
                                //    AddErrors(resultParent);
                                //}
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Same name existed");
                        }
                    }
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                log.Info("Error exception function register accont_" + DateTime.Now + ": " + ex);
                ModelState.AddModelError("", "**Error register");
                return RedirectToAction("Index", "ErrorMessage");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Offtwofa()
        {
            User user = new User
            {
                TwoFactorEnabled = false
            };
            _userManager.Update(user);
            return View();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await _userManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //Change Password
        // GET: /Account/Manage
        [Authorize]
        public ActionResult Manage(ManageMessageId? message, string messagechange)
        {
            var userId = User.Identity.GetUserId();
            var user = _userManager.FindById(userId);

            //tạo mới lại 2fa
            var resultImage = _twoFact.GetQrCodeImage(User.Identity.Name);
            var resultText = _twoFact.GetQrCodeText(User.Identity.Name);
            //xóa cookie và lấy cookie mới;
            HttpCookie qrImage = Request.Cookies["mimosa"];
            HttpCookie qrText = Request.Cookies["mamosi"];
            //Response.Cookies["mimosa"].Expires = DateTime.Now.AddDays(-1);
            //Response.Cookies["mamosi"].Expires = DateTime.Now.AddDays(-1);
            ///
            //qrImage.Value = null;
            //qrText.Value = null;
            user.Temp = resultImage;
            user.Temp1 = resultText;
            _userManager.Update(user);
            ViewBag.QRImage = user.Temp;
            ViewBag.QRText = user.Temp1;
            ViewBag.UserName = user.UserName;
            ViewBag.Cofirm2fa = user.TwoFactorEnabled;
            //gán lại giá trị sua khi cập nhật cho cookie
            qrImage.Value = user.Temp;
            qrText.Value = user.Temp1;
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("SignOut");
            if (messagechange != null)
            {
                ViewBag.messagechange = messagechange;
            }
            return View();
        }

        //Change Password
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = _userManager.FindById(userId);

            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            //string message = string.Empty;
            ManageMessageId? message = null;


            if (hasPassword)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        IdentityResult result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.CurrentPassword, model.Password);
                        //var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);//quên mật khẩu
                        //string body = "Change Password Success";
                        //string title = "Successful";
                        if (result.Succeeded)
                        {
                            var findUserName = _userManager.FindByName(User.Identity.Name);
                            findUserName.Temp2 = model.Password;
                            //findUserName.PasswordNotHash = model.NewPassword;
                            _userManager.Update(findUserName);
                            //message = ManageMessageId.RemoveLoginSuccess;
                            //message = "Change Password Success";
                            //message = (ManageMessageId?)MessageBox.Show(body, title);
                            //ViewBag.body = "Change Password Success";
                            return RedirectToAction("Manage", new { message = ManageMessageId.ChangePasswordSuccess, messagechange = "Change Password Success" }); ;
                        }
                        else
                        {
                            //message = "Change Password Failed";
                            AddErrors(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //message = "Change Password Failed";
                    log.Info("Function change password account_" + DateTime.Now + ": " + ex);
                    return RedirectToAction("Index", "ErrorMessage");
                }

            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }
                try
                {
                    if (ModelState.IsValid)
                    {
                        IdentityResult result = await _userManager.AddPasswordAsync(User.Identity.GetUserId(), model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Info("Function change password account: " + ex);
                    return RedirectToAction("Index", "ErrorMessage");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await _userManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new User() { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        //[HttpGet]
        //public ActionResult AddRole()
        //{
        //    _userRoleManager.cre
        //}
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["User"] = null;
            Session["UserName"] = null;
            Request.Cookies.Remove("mimosa");//QRIMage
            Request.Cookies.Remove("mamosi");//QRText
            Response.Cookies["mimosa"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["mamosi"].Expires = DateTime.Now.AddDays(-1);
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SignOut()
        {
            Response.Cookies["mimosa"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["mamosi"].Expires = DateTime.Now.AddDays(-1);
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = _userManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }
        [Authorize]
        public ActionResult GetSession(string username)
        {
            var userId = User.Identity.GetUserId();
            Session["User"] = _userManager.FindById(userId);
            return null;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
        #region load QR code
        public async Task<ActionResult> LoadQRCode()
        {
            var aa = Session["User"];
            return PartialView("_LoadQRCode");
        }
        #endregion
        #region Enable 2FA
        [HttpPost]
        public JsonResult Enable_2FA(string username, string pinCode)
        {
            try
            {
                var resultEnable = _twoFact.ValidateTwoFactorPIN_2FA(username, pinCode);
                if (resultEnable == true)
                {
                    var user = _userManager.FindByName(username);
                    user.TwoFactorEnabled = true;
                    _userManager.Update(user);
                    log.Info(DateTime.Now + "_Start Validate success 2FA");
                    return Json(new { Message = "Enable success", Status = 1 });
                }
                else return Json(new { Message = "Enable unsuccessful", Status = -1 });

            }
            catch (Exception ex)
            {
                log.Info("ERROR Exception_" + DateTime.Now + ":" + ex);
                return Json(new { Message = "ERROR Validate 2FA", Status = -1 });
            }
        }
        [AllowAnonymous]
        public async Task<ActionResult> LoginAferConfrim_2faAsync(string username)
        {
            var user = _userManager.FindByName(username);
            Session["User"] = user;
            Session["UserName"] = user.UserName;
            HttpCookie qrImage = Request.Cookies["mimosa"];
            HttpCookie qrText = Request.Cookies["mamosi"];
            if (qrImage != null && qrText != null)
            {
                //Request.Cookies.Remove("mimosa");
                //Request.Cookies.Remove("mamosi");
                qrImage.Value = null;
                qrText.Value = null;

                qrImage.Value = user.Temp;
                qrImage.Expires = DateTime.Now.AddDays(1);
                qrText.Value = user.Temp1;
                qrText.Expires = DateTime.Now.AddDays(1);
                Request.Cookies.Add(qrImage);
                Request.Cookies.Add(qrText);
            }
            else
            {
                qrImage = new HttpCookie("mimosa", user.Temp);
                qrImage.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(qrImage);

                qrText = new HttpCookie("mamosi", user.Temp1);
                qrText.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(qrText);
            }
            await SignInAsync(user, true);
            return Redirect("/home/AccountDashboard");
        }
        #endregion
        #region reset password
        /// <summary>
        /// Form nhập email để reset password
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(InputEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.EmailReset);
                    if (user == null || !(user.EmailConfirmed))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return View(model: "Email is not registered. Please check again!!!");
                    }

                    //var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                    //gọi đến function email gửi mail xác thực lấy lại mật khẩu
                    log.Info(DateTime.Now + "_Start call api mail");
                    var resForgot = await _confirmEmail.CallMailResetPass(CommonCalculate.GetTimeStamp().ToString(), user.UserName, 5);//5 là type để lưu 
                    log.Info(DateTime.Now + "_End call api mail");
                    MessageResponeApi mess = JsonConvert.DeserializeObject<MessageResponeApi>(resForgot);
                    if (mess.StatusCode.Equals("1"))
                    {
                        return View(model: "A verification code is sent to your mail. Please check mail to reset your password (**Note: Effective 30 minutes)");
                    }
                    return View(model: "A verification code isn't sent to your mail");
                }
                catch (Exception ex)
                {
                    log.Info("Error exception function forgot password_" + DateTime.Now + ": " + ex);
                    return View(model: "Reset mail failed. Please try again !!!");
                }
            }
            return View(model: "Reset mail failed. Please try again !!!");
            // If we got this far, something failed, redisplay form
        }
        #endregion
    }

}