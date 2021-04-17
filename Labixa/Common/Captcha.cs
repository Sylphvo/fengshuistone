//using Labixa.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web;

//namespace Labixa.Common
//{
//    public interface IValidateCapcha
//    {
//        CaptchaResponse ValidateCaptcha(string response);
//    }
//    public class Captcha: IValidateCapcha
//    {
//        public CaptchaResponse ValidateCaptcha(string response)
//        {
//            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["6LfbCIAaAAAAAMvBQYGig_wHWsnBb_f67W1ec3x5"];
//            var client = new WebClient();
//            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
//            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
//        }
//    }
//}