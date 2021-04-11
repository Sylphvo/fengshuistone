using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Net;
using log4net;
using System.Reflection;
using System.Net.Http;

namespace Labixa.Common
{
    public interface IConfirmEmail
    {
       // string ConfirmEmailRegisterAccount(string userName, string emailTo, HttpServerUtilityBase server, string id);
        //string TransactionEmailAccount(string id, string emailTo, string timestamp, HttpServerUtilityBase server, string amount, string addressTo, string feeTransaction, string username);
        //string TransactionEmailAccountVIP(string id, string emailTo, string timestamp, HttpServerUtilityBase server, string amount, string addressTo, string feeTransaction, string userName);
        //string ConfirmEmailForgotPassword(string id, string username, HttpServerUtilityBase server, string emailTo, string timestamp);
        //string ConfirmEmailDeposit(string addressTo, string username, string amount, int type, HttpServerUtilityBase server);
        System.Threading.Tasks.Task<string> CallMailRegister(string timestamp, string username, int type);//gọi api mail để gửi mail xác nhận đăng ký
        System.Threading.Tasks.Task<string> CallMailResetPass(string timestamp,string username, int type);//gọi api mail để gửi mail Reset pass


    }
    public class ConfirmEmail : IConfirmEmail
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string EMAIL_SENDER = ConfigurationManager.AppSettings["Email_sender"].ToString();
        private string EMAIL_SENDER_PASSWORD = ConfigurationManager.AppSettings["password"].ToString();
        private string EMAIL_SENDER_HOST = ConfigurationManager.AppSettings["Host_smtp"].ToString();
        private int EMAIL_SENDER_PORT = Convert.ToInt16(ConfigurationManager.AppSettings["portnumber"]);
        private Boolean EMAIL_IsSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
        private string _DOMAIN = ConfigurationManager.AppSettings["domain"].ToString();
        private string domain_mail = ConfigurationManager.AppSettings["domain_mail"].ToString();
        //public string ConfirmEmailRegisterAccount(string userName, string emailTo, HttpServerUtilityBase server, string id)
        //{
        //    try
        //    {
        //        log.Info("Start Send Mail Confirm Account Register");
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(server.MapPath("~/Views/EmailFunc/Template_one.html")))
        //        {
        //            body = reader.ReadToEnd();
        //        }
        //        //Repalce [newusername] = signup user name   
        //        body = body.Replace("[newusername]", userName);
        //        body = body.Replace("[token-key]", id);
        //        body = body.Replace("[domain]", _DOMAIN);
        //        MailMessage _mailmsg = new MailMessage();

        //        //Make TRUE because our body text is html  
        //        _mailmsg.IsBodyHtml = true;

        //        //Set From Email ID  
        //        _mailmsg.From = new MailAddress(EMAIL_SENDER);

        //        //Set To Email ID  
        //        _mailmsg.To.Add(emailTo);

        //        //Set Subject  
        //        _mailmsg.Subject = "Welcome to Big Player Club";

        //        //Set Body Text of Email   
        //        _mailmsg.Body = body;


        //        //Now set your SMTP   
        //        SmtpClient _smtp = new SmtpClient();

        //        //Set HOST server SMTP detail  
        //        _smtp.Host = EMAIL_SENDER_HOST;

        //        //Set PORT number of SMTP  
        //        _smtp.Port = EMAIL_SENDER_PORT;

        //        //Set SSL --> True / False  
        //        _smtp.EnableSsl = EMAIL_IsSSL;

        //        //Set Sender UserEmailID, Password  
        //        NetworkCredential _network = new NetworkCredential(EMAIL_SENDER, EMAIL_SENDER_PASSWORD);
        //        _smtp.Credentials = _network;

        //        //Send Method will send your MailMessage create above.  
        //        _smtp.Send(_mailmsg);
        //        log.Info("Send Success Mail Confirm Account Register");
        //        return "Send Success Mail Confirm Account Register";
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info("Send Unsucces Mail Confirm Account Register - ERROR Exception Send Mail " + ex);
        //        return "Send mail unsuccessul !.";
        //        throw;
        //    }
        //}
        //public string TransactionEmailAccount(string id, string emailTo, string timestamp, HttpServerUtilityBase server, string Amount, string AddressTo, string FeeTransaction, string username)
        //{
        //    try
        //    {
        //        log.Info("Start Send Mail Confirm Account Register");
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(server.MapPath("~/Views/EmailFunc/Template_transaction.html")))
        //        {
        //            body = reader.ReadToEnd();
        //        }
        //        //Repalce [newusername] = signup user name  
        //        body = body.Replace("[timestamp]", timestamp + "_" + id);
        //        body = body.Replace("[domain]", _DOMAIN);
        //        body = body.Replace("[Address]", AddressTo);
        //        body = body.Replace("[Amount]", Amount);
        //        body = body.Replace("[Fee]", FeeTransaction);
        //        body = body.Replace("[Status]", "New");
        //        body = body.Replace("[username]", username);

        //        MailMessage _mailmsg = new MailMessage();

        //        //Make TRUE because our body text is html  
        //        _mailmsg.IsBodyHtml = true;

        //        //Set From Email ID  
        //        _mailmsg.From = new MailAddress(EMAIL_SENDER);

        //        //Set To Email ID  
        //        _mailmsg.To.Add(emailTo);

        //        //Set Subject  
        //        _mailmsg.Subject = "Big Player Club Message";

        //        //Set Body Text of Email   
        //        _mailmsg.Body = body;


        //        //Now set your SMTP   
        //        SmtpClient _smtp = new SmtpClient();

        //        //Set HOST server SMTP detail  
        //        _smtp.Host = EMAIL_SENDER_HOST;

        //        //Set PORT number of SMTP  
        //        _smtp.Port = EMAIL_SENDER_PORT;

        //        //Set SSL --> True / False  
        //        _smtp.EnableSsl = EMAIL_IsSSL;

        //        //Set Sender UserEmailID, Password  
        //        NetworkCredential _network = new NetworkCredential(EMAIL_SENDER, EMAIL_SENDER_PASSWORD);
        //        _smtp.Credentials = _network;

        //        //Send Method will send your MailMessage create above.  
        //        _smtp.Send(_mailmsg);
        //        log.Info("Send Success Mail Confirm Account Register");
        //        return "Send Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info("Send Unsucces Mail Transaction - ERROR Exception Send Mail " + ex);
        //        return "Send mail unsuccessul !.";
        //        throw;
        //    }
        //}
        //public string TransactionEmailAccountVIP(string id, string emailTo, string timestamp, HttpServerUtilityBase server, string amount, string addressTo, string feeTransaction, string userName)
        //{
        //    try
        //    {
        //        log.Info("Start Send Mail Confirm Account Register");
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(server.MapPath("~/Views/EmailFunc/Template_transactionVIP.html")))
        //        {
        //            body = reader.ReadToEnd();
        //        }
        //        //Repalce [newusername] = signup user name  
        //        body = body.Replace("[timestamp]", timestamp + "_" + id);
        //        body = body.Replace("[domain]", _DOMAIN);
        //        body = body.Replace("[Address]", addressTo);
        //        body = body.Replace("[Amount]", amount);
        //        body = body.Replace("[Fee]", feeTransaction);
        //        body = body.Replace("[Status]", "New");
        //        body = body.Replace("[username]", userName);

        //        MailMessage _mailmsg = new MailMessage();

        //        //Make TRUE because our body text is html  
        //        _mailmsg.IsBodyHtml = true;

        //        //Set From Email ID  
        //        _mailmsg.From = new MailAddress(EMAIL_SENDER);

        //        //Set To Email ID  
        //        _mailmsg.To.Add(emailTo);

        //        //Set Subject  
        //        _mailmsg.Subject = "Big Player Club Message";

        //        //Set Body Text of Email   
        //        _mailmsg.Body = body;


        //        //Now set your SMTP   
        //        SmtpClient _smtp = new SmtpClient();

        //        //Set HOST server SMTP detail  
        //        _smtp.Host = EMAIL_SENDER_HOST;

        //        //Set PORT number of SMTP  
        //        _smtp.Port = EMAIL_SENDER_PORT;

        //        //Set SSL --> True / False  
        //        _smtp.EnableSsl = EMAIL_IsSSL;

        //        //Set Sender UserEmailID, Password  
        //        NetworkCredential _network = new NetworkCredential(EMAIL_SENDER, EMAIL_SENDER_PASSWORD);
        //        _smtp.Credentials = _network;

        //        //Send Method will send your MailMessage create above.  
        //        _smtp.Send(_mailmsg);
        //        log.Info("Send Success Mail Confirm Account Register");
        //        return "Send Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info("Send Unsucces Mail Transaction - ERROR Exception Send Mail " + ex);
        //        return "Send mail unsuccessul !.";
        //        throw;
        //    }
        //}/// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code">.net tự động generate ra</param>
        /// <param name="username"></param>
        /// <param name="server"></param>
        /// <param name="emailTo"></param>
        /// <param name="timestamp">để xác thực thời gian timeout của mail này</param>
        /// <returns></returns>
        //public string ConfirmEmailForgotPassword(string id, string username, HttpServerUtilityBase server, string emailTo, string timestamp)
        //{
        //    try
        //    {
        //        log.Info("Start Send Mail Confirm Account Register");
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(server.MapPath("~/Views/EmailFunc/template_mailResetPass.html")))
        //        {
        //            body = reader.ReadToEnd();
        //        }
        //        //Repalce [newusername] = signup user name  
        //        body = body.Replace("[username]", username);
        //        body = body.Replace("[domain]", _DOMAIN);
        //        body = body.Replace("[userID]", id);
        //        body = body.Replace("[timestamp]", timestamp);// link để từ chối nhận mail này (chưa làm chức năng này)


        //        MailMessage _mailmsg = new MailMessage();

        //        //Make TRUE because our body text is html  
        //        _mailmsg.IsBodyHtml = true;

        //        //Set From Email ID  
        //        _mailmsg.From = new MailAddress(EMAIL_SENDER);

        //        //Set To Email ID  
        //        _mailmsg.To.Add(emailTo);

        //        //Set Subject  
        //        _mailmsg.Subject = "Big Player Club Message";

        //        //Set Body Text of Email   
        //        _mailmsg.Body = body;


        //        //Now set your SMTP   
        //        SmtpClient _smtp = new SmtpClient();

        //        //Set HOST server SMTP detail  
        //        _smtp.Host = EMAIL_SENDER_HOST;

        //        //Set PORT number of SMTP  
        //        _smtp.Port = EMAIL_SENDER_PORT;

        //        //Set SSL --> True / False  
        //        _smtp.EnableSsl = EMAIL_IsSSL;

        //        //Set Sender UserEmailID, Password  
        //        NetworkCredential _network = new NetworkCredential(EMAIL_SENDER, EMAIL_SENDER_PASSWORD);
        //        _smtp.Credentials = _network;

        //        //Send Method will send your MailMessage create above.  
        //        _smtp.Send(_mailmsg);
        //        log.Info("Send Success Mail Confirm Account Forgot password");
        //        return "1";//gửi thành công
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info("Send Unsucces Mail Forgot Password - ERROR Exception Send Mail " + ex);
        //        return "Send mail unsuccessul !.";
        //        throw;
        //    }
        //}
        //public string ConfirmEmailDeposit(string addressTo,string username, string amount, int type, HttpServerUtilityBase server)
        //{
        //    try
        //    {
        //        log.Info("Start Send Mail Confirm Account Register");
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(server.MapPath("~/Views/EmailFunc/Template_transactionDesposit.html")))
        //        {
        //            body = reader.ReadToEnd();
        //        }
        //        //Repalce [newusername] = signup user name   
        //        body = body.Replace("[username]", username);
        //        body = body.Replace("[Amount]", amount);
        //        body = body.Replace("[Datetime]", DateTime.Now.ToString());
        //        body = body.Replace("[Total]", amount);
        //        body = body.Replace("[domain]", _DOMAIN);

        //        if (type == 1)
        //        {
        //            body = body.Replace("[typeTransaction]", "Ethereum");
        //        }
        //        else
        //        {
        //            body = body.Replace("[typeTransaction]", "VIP");
        //        }
        //        //body = body.Replace("[domain]", _DOMAIN);
        //        MailMessage _mailmsg = new MailMessage();

        //        //Make TRUE because our body text is html  
        //        _mailmsg.IsBodyHtml = true;

        //        //Set From Email ID  
        //        _mailmsg.From = new MailAddress(EMAIL_SENDER);

        //        //Set To Email ID  
        //        _mailmsg.To.Add(addressTo);

        //        //Set Subject  
        //        _mailmsg.Subject = "Big Player Club Message";

        //        //Set Body Text of Email   
        //        _mailmsg.Body = body;


        //        //Now set your SMTP   
        //        SmtpClient _smtp = new SmtpClient();

        //        //Set HOST server SMTP detail  
        //        _smtp.Host = EMAIL_SENDER_HOST;

        //        //Set PORT number of SMTP  
        //        _smtp.Port = EMAIL_SENDER_PORT;

        //        //Set SSL --> True / False  
        //        _smtp.EnableSsl = EMAIL_IsSSL;

        //        //Set Sender UserEmailID, Password  
        //        NetworkCredential _network = new NetworkCredential(EMAIL_SENDER, EMAIL_SENDER_PASSWORD);
        //        _smtp.Credentials = _network;

        //        //Send Method will send your MailMessage create above.  
        //        _smtp.Send(_mailmsg);
        //        log.Info("Send Success Mail Confirm Account Register");
        //        return "Send mail successful";
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info("Send Unsucces Mail Confirm Account Register - ERROR Exception Send Mail " + ex);
        //        return "Send mail unsuccessul !.";
        //        throw;
        //    }
        //}
        public async System.Threading.Tasks.Task<string> CallMailRegister(string timestamp, string username, int type)//gọi api mail để gửi mail xác nhận đăng ký
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domain_mail);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var _url = domain_mail + "WebCallMail/MailRegister?timeStamp="+timestamp+"&username=" + username+"&type="+type;
                HttpResponseMessage response = await client.GetAsync(_url);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }
        public async System.Threading.Tasks.Task<string> CallMailResetPass(string timestamp, string username, int type) {
            log.Info("Call action to call api mail");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domain_mail);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var _url = domain_mail + "WebCallMail/MailResetPassword?timestamp=" + timestamp+"&username="+username+"&type="+type;
                HttpResponseMessage response = await client.GetAsync(_url);
                if (response.IsSuccessStatusCode)
                {
                    log.Info("Send mail success. Respone success");
                    return response.Content.ReadAsStringAsync().Result;
                }
                return "";
            }
        }//gọi api mail để gửi mail Reset pass

    }
    public class InheritanceHttpServerUtilityBase: HttpServerUtilityBase
    {
        public override string MapPath(string path)
        {
            return base.MapPath(path);
        }
    }
}