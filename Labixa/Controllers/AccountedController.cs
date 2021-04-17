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
    public class AccountedController : Controller
    {
        private UserManager<User> _userManager;
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public AccountedController(UserManager<User> userManager,ITwoFactAuthAdmin twoFact, IConfirmEmail confirmEmail)
        {
            _userManager = userManager;
        }
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Signin(string returnUrl)
        {
            //AuthenticationManager.SignOut();
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //ViewBag.ReturnUrl = returnUrl;
            //Response.Cookies["mimosa"].Expires = DateTime.Now.AddDays(-1);
            //Response.Cookies["mamosi"].Expires = DateTime.Now.AddDays(-1);
            //Request.Cookies.Remove("mimosa");//QRIMage
            //Request.Cookies.Remove("mamosi");//QRText
            //                                 //Response.Cookies.Clear()
            return View();
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
                                    _userManager.Update(user);
                                    return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult SignOut()
        {
            Response.Cookies["mimosa"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["mamosi"].Expires = DateTime.Now.AddDays(-1);
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}