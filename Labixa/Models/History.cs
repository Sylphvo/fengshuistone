using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.Models
{
    #region Deposit Currency
    public class DepositETHCurrency
    {
        public int id { get; set; }
        public DateTime Time { get; set; }
        public double Amount { get; set; }
        public string TxHash { get; set; }
        public string Status { get; set; }
    }
    public class DepositBITCurrency
    {
        public int id { get; set; }
        public DateTime Time { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
    }
    #endregion
    #region Deposit New
    public class DepositETHNew
    {
        public string id { get; set; }
        public DateTime Time { get; set; }
        public double amount { get; set; }
        public string txHash { get; set; }
        public string status { get; set; }
        public string fromAddress { get; set; }
        public double Fee { get; set; }
    }
    public class DepositBITNew
    {
        public string id { get; set; }
        public DateTime Time { get; set; }
        public double amount { get; set; }
        public string status { get; set; }
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public double Fee { get; set; }
    }
    #endregion
    #region Exchange Currency
    public class ExchangeBITCurrency
    {
        public int id { get; set; }
        public DateTime Time { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public int Fee { get; set; }
        public string Status { get; set; }
    }
    public class ExchangeETHCurrency
    {
        public int id { get; set; }
        public DateTime Time { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public int Fee { get; set; }
        public string Status { get; set; }
    }
    #endregion
    #region Exchange New
    public class ExchangeBITNew
    {
        public string id { get; set; }
        public DateTime Time { get; set; }
        public double from { get; set; }
        public double to { get; set; }
        public string status { get; set; }
        public double fee { get; set; }
    }
    public class ExchangeETHNew
    {
        public string id { get; set; }
        public DateTime Time { get; set; }
        public double from { get; set; }
        public double to { get; set; }
        public string status { get; set; }
        public double fee { get; set; }
    }
    #endregion
    #region Withdraw Currency
    public class WithdrawETHCurrency
    {
        public int id { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public string TxnHash { get; set; }
        public string Status { get; set; }
    }
    public class WithdrawBITCurrency
    {
        public int id { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public string Status { get; set; }
    }
    #endregion
    #region Withdraw New
    public class WithdrawETHNew
    {
        public string id { get; set; }
        public DateTime Time { get; set; }
        public double amount { get; set; }
        public string txHash { get; set; }
        public string status { get; set; }
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public double Fee { get; set; }
    }
    public class WithdrawBITNew
    {
        public string id { get; set; }
        public DateTime Time { get; set; }
        public double amount { get; set; }
        public string status { get; set; }
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public double Fee { get; set; }
    }
    #endregion


    






}