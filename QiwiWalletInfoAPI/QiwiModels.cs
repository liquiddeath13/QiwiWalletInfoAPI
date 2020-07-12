using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QiwiModels
{
    public enum PaymentOperationType
    {
        ALL,
        IN,
        OUT,
        QIWI_CARD
    }
    public enum ChequeImageFormat
    {
        JPEG,
        PDF
    }
    public class Money
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
    public class IncomingTotal : Money { }
    public class OutgoingTotal : Money { }
    public class Sum : Money { }
    public class Commission : Money { }
    public class Total : Money { }
    public class Balance : Money { }
    public class Type
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
    public class Account
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }
        [JsonProperty("fsAlias")]
        public string FsAlias { get; set; }
        [JsonProperty("bankAlias")]
        public string BankAlias { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("type")]
        public Type Type { get; set; }
        [JsonProperty("hasBalance")]
        public bool HasBalance { get; set; }
        [JsonProperty("balance")]
        public Balance Balance { get; set; }
        [JsonProperty("currency")]
        public long Currency { get; set; }
    }
    public class WalletBalance
    {
        [JsonProperty("accounts")]
        public List<Account> Accounts { get; set; }
    }
    public class MailForCheque
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
    public class ChequeImage
    {
        public List<string> MyArray { get; set; }
    }
    public class PaymentsStats
    {
        [JsonProperty("incomingTotal")]
        public List<IncomingTotal> IncomingTotal { get; set; }
        [JsonProperty("outgoingTotal")]
        public List<OutgoingTotal> OutgoingTotal { get; set; }
    }
    public class Provider
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("shortName")]
        public string ShortName { get; set; }
        [JsonProperty("longName")]
        public string LongName { get; set; }
        [JsonProperty("logoUrl")]
        public object LogoUrl { get; set; }
        [JsonProperty("description")]
        public object Description { get; set; }
        [JsonProperty("keys")]
        public object Keys { get; set; }
        [JsonProperty("siteUrl")]
        public object SiteUrl { get; set; }
        [JsonProperty("extras")]
        public List<object> Extras { get; set; }
    }
    public class Source
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("shortName")]
        public string ShortName { get; set; }
        [JsonProperty("longName")]
        public string LongName { get; set; }
        [JsonProperty("logoUrl")]
        public object LogoUrl { get; set; }
        [JsonProperty("description")]
        public object Description { get; set; }
        [JsonProperty("keys")]
        public string Keys { get; set; }
        [JsonProperty("siteUrl")]
        public object SiteUrl { get; set; }
        [JsonProperty("extras")]
        public List<object> Extras { get; set; }
    }
    public class Transaction
    {
        [JsonProperty("txnId")]
        public long TxnId { get; set; }
        [JsonProperty("personId")]
        public long PersonId { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("errorCode")]
        public long ErrorCode { get; set; }
        [JsonProperty("error")]
        public object Error { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("statusText")]
        public string StatusText { get; set; }
        [JsonProperty("trmTxnId")]
        public string TrmTxnId { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("sum")]
        public Sum Sum { get; set; }
        [JsonProperty("commission")]
        public Commission Commission { get; set; }
        [JsonProperty("total")]
        public Total Total { get; set; }
        [JsonProperty("provider")]
        public Provider Provider { get; set; }
        [JsonProperty("source")]
        public Source Source { get; set; }
        [JsonProperty("comment")]
        public object Comment { get; set; }
        [JsonProperty("currencyRate")]
        public long CurrencyRate { get; set; }
        [JsonProperty("extras")]
        public object Extras { get; set; }
        [JsonProperty("chequeReady")]
        public bool ChequeReady { get; set; }
        [JsonProperty("bankDocumentAvailable")]
        public bool BankDocumentAvailable { get; set; }
        [JsonProperty("bankDocumentReady")]
        public bool BankDocumentReady { get; set; }
        [JsonProperty("repeatPaymentEnabled")]
        public bool RepeatPaymentEnabled { get; set; }
        [JsonProperty("favoritePaymentEnabled")]
        public bool FavoritePaymentEnabled { get; set; }
        [JsonProperty("regularPaymentEnabled")]
        public bool RegularPaymentEnabled { get; set; }
    }
    public class Payments
    {
        [JsonProperty("data")]
        public List<Transaction> Data { get; set; }

        [JsonProperty("nextTxnId")]
        public long NextTxnId { get; set; }

        [JsonProperty("nextTxnDate")]
        public DateTime NextTxnDate { get; set; }
    }
    public class IdentificationData
    {
        [JsonProperty("birthDate")]
        public string BirthDate { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("inn")]
        public string Inn { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("middleName")]
        public string MiddleName { get; set; }
        [JsonProperty("oms")]
        public string Oms { get; set; }
        [JsonProperty("passport")]
        public string Passport { get; set; }
        [JsonProperty("snils")]
        public string Snils { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class MobilePinInfo
    {
        [JsonProperty("lastMobilePinChange")]
        public DateTime LastMobilePinChange { get; set; }
        [JsonProperty("mobilePinUsed")]
        public bool MobilePinUsed { get; set; }
        [JsonProperty("nextMobilePinChange")]
        public DateTime NextMobilePinChange { get; set; }
    }
    public class PassInfo
    {
        [JsonProperty("lastPassChange")]
        public DateTime LastPassChange { get; set; }
        [JsonProperty("nextPassChange")]
        public DateTime NextPassChange { get; set; }
        [JsonProperty("passwordUsed")]
        public bool PasswordUsed { get; set; }
    }
    public class PinInfo
    {
        [JsonProperty("pinUsed")]
        public bool PinUsed { get; set; }
    }
    public class AuthInfo
    {
        [JsonProperty("boundEmail")]
        public string BoundEmail { get; set; }
        [JsonProperty("ip")]
        public string Ip { get; set; }
        [JsonProperty("lastLoginDate")]
        public DateTime LastLoginDate { get; set; }
        [JsonProperty("mobilePinInfo")]
        public MobilePinInfo MobilePinInfo { get; set; }
        [JsonProperty("passInfo")]
        public PassInfo PassInfo { get; set; }
        [JsonProperty("personId")]
        public long PersonId { get; set; }
        [JsonProperty("pinInfo")]
        public PinInfo PinInfo { get; set; }
        [JsonProperty("registrationDate")]
        public DateTime RegistrationDate { get; set; }
    }
    public class IdentificationInfo
    {
        [JsonProperty("bankAlias")]
        public string BankAlias { get; set; }
        [JsonProperty("identificationLevel")]
        public string IdentificationLevel { get; set; }
    }
    public class ContractInfo
    {
        [JsonProperty("blocked")]
        public bool Blocked { get; set; }
        [JsonProperty("contractId")]
        public long ContractId { get; set; }
        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }
        [JsonProperty("features")]
        public List<object> Features { get; set; }
        [JsonProperty("identificationInfo")]
        public List<IdentificationInfo> IdentificationInfo { get; set; }
    }
    public class UserInfo
    {
        [JsonProperty("defaultPayCurrency")]
        public long DefaultPayCurrency { get; set; }
        [JsonProperty("defaultPaySource")]
        public long DefaultPaySource { get; set; }
        [JsonProperty("email")]
        public object Email { get; set; }
        [JsonProperty("firstTxnId")]
        public long FirstTxnId { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("operator")]
        public string Operator { get; set; }
        [JsonProperty("phoneHash")]
        public string PhoneHash { get; set; }
        [JsonProperty("promoEnabled")]
        public object PromoEnabled { get; set; }
    }
    public class QiwiWalletProfile
    {
        [JsonProperty("authInfo")]
        public AuthInfo AuthInfo { get; set; }
        [JsonProperty("contractInfo")]
        public ContractInfo ContractInfo { get; set; }
        [JsonProperty("userInfo")]
        public UserInfo UserInfo { get; set; }
    }
}
