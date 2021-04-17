using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.Common
{
    public interface ITwoFactAuthAdmin
    {
        string GetQrCodeImage(string userName);
        string GetQrCodeText(string userName);
        bool ValidateTwoFactorPIN_2FA(string userName, string pinCode);
    }
    public class TwoFactAuthAdmin : ITwoFactAuthAdmin
    {
        private string key_key = System.Configuration.ConfigurationManager.AppSettings["key_2fa"];
        private string nameweb_2fa = System.Configuration.ConfigurationManager.AppSettings["name_web_2fa"];

        /// <summary>
        /// Generate mã QR bằng hình ảnh
        /// </summary>
        /// <param name="userName">tên player khi đăng ký xong</param>
        /// <returns></returns>
        public string GetQrCodeImage(string userName)
        {
            TwoFactorAuthenticator _tfa = new TwoFactorAuthenticator();
            var key = _tfa.GeneratePINAtInterval(userName, 360, 6);
            var result = _tfa.GenerateSetupCode(userName, userName, userName, 300, 300);
            var urlImag = result.QrCodeSetupImageUrl;
            return urlImag;
        }
        /// <summary>
        /// Generate max QR bằng code
        /// </summary>
        /// <param name="userName">tên player khi đăng ký xong</param>
        /// <returns></returns>
        public string GetQrCodeText(string userName)
        {
            TwoFactorAuthenticator _tfa = new TwoFactorAuthenticator();
            var key = _tfa.GeneratePINAtInterval(userName, 360, 6);
            var result = _tfa.GenerateSetupCode(userName, userName, userName, 300, 300);
            var QRtext = result.ManualEntryKey;
            return QRtext;
        }
        public bool ValidateTwoFactorPIN_2FA(string userName, string pinCode)
        {
            TwoFactorAuthenticator _tfa = new TwoFactorAuthenticator();
            var result = _tfa.ValidateTwoFactorPIN(userName, pinCode, TimeSpan.FromSeconds(30));
            return result;
        }
    }
}