using Labixa.Common;
using log4net;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace Labixa.Controllers
{
    public class TransactionController : Controller
    {
        //
        // GET: /Transaction/
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        private UserManager<User> _userManager;
        private readonly ISDKApiFundist _sdkApiFundist;
        private readonly ISDKApiAdmin _sdkApiAdmin;
        public TransactionController(ISDKApiFundist sDKApiFundist, ISDKApiAdmin sDKApiAdmin, UserManager<User> userManager)
        {
            _userManager = userManager;
            this._sdkApiFundist = sDKApiFundist;
            this._sdkApiAdmin = sDKApiAdmin;
        }
        public ActionResult Index()
        {
            return View();
        }
	}
}