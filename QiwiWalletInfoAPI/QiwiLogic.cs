using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace QiwiLogic
{
    public class QiwiApi
    {
        string qiwiToken = "";
        string apiPath = "https://edge.qiwi.com";
        public QiwiApi(string token) => qiwiToken = token;
        public QiwiModels.QiwiWalletProfile GetProfile()
                                            => JsonConvert
                                            .DeserializeObject<QiwiModels.QiwiWalletProfile>
                                            (
                                                NetUtils.GetStringFromResponse
                                                (
                                                    NetUtils.Get
                                                    (
                                                        $"{apiPath}/person-profile/v1/profile/current",
                                                        "application/json",
                                                        "application/json",
                                                        new NameValueCollection
                                                        {
                                                                { "Authorization", $"Bearer {qiwiToken}" }
                                                        }
                                                    )
                                                ),
                                                new JsonSerializerSettings
                                                {
                                                    NullValueHandling = NullValueHandling.Ignore
                                                }
                                            );
        public bool IdentifyMe(QiwiModels.IdentificationData id, long wallet)
        {
            QiwiModels.IdentificationData response
                                          = JsonConvert
                                          .DeserializeObject<QiwiModels.IdentificationData>
                                          (
                                              NetUtils.GetStringFromResponse
                                              (
                                                  NetUtils.JsonPost
                                                  (
                                                      $"{apiPath}/identification/v1/persons/{wallet}/identification",
                                                      id,
                                                      "application/json",
                                                      "application/json",
                                                      new NameValueCollection
                                                      {
                                                              { "Authorization", $"Bearer {qiwiToken}" }
                                                      },
                                                      propsToRemove: new List<string> { "type" }
                                                  )
                                              ),
                                              new JsonSerializerSettings
                                              {
                                                  NullValueHandling = NullValueHandling.Ignore
                                              }
                                          );
            return response.Inn != "" && (response.Type == "VERIFIED" || response.Type == "FULL");
        }
        public QiwiModels.IdentificationData GetIdentity(long wallet)
                                             => JsonConvert
                                             .DeserializeObject<QiwiModels.IdentificationData>
                                             (
                                                 NetUtils.GetStringFromResponse
                                                 (
                                                     NetUtils.Get
                                                     (
                                                         $"{apiPath}/identification/v1/persons/{wallet}/identification",
                                                         "application/json",
                                                         "",
                                                         new NameValueCollection
                                                         {
                                                                 { "Authorization", $"Bearer {qiwiToken}" }
                                                         }
                                                     )
                                                 ),
                                                 new JsonSerializerSettings
                                                 {
                                                     NullValueHandling = NullValueHandling.Ignore
                                                 }
                                             );
        public QiwiModels.Payments GetPayments
        (
            long wallet,
            int rows,
            QiwiModels.PaymentOperationType operation = QiwiModels.PaymentOperationType.ALL
        )
        => JsonConvert
        .DeserializeObject<QiwiModels.Payments>
        (
            NetUtils.GetStringFromResponse
            (
                NetUtils.Get
                (
                    $"{apiPath}/payment-history/v2/persons/{wallet}/payments",
                    "application/json",
                    "",
                    new NameValueCollection
                    {
                            { "Authorization", $"Bearer {qiwiToken}" }
                    },
                    new NameValueCollection
                    {
                            { "rows", $"{rows}" },
                            { "operation", Enum.GetName(typeof(QiwiModels.PaymentOperationType), operation) },
                    }
                )
            ),
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }
        );
        public QiwiModels.PaymentsStats GetPaymentsStats
        (
            long wallet,
            DateTime startDate,
            DateTime endDate,
            QiwiModels.PaymentOperationType operation = QiwiModels.PaymentOperationType.ALL
        )
        => JsonConvert
        .DeserializeObject<QiwiModels.PaymentsStats>
        (
            NetUtils.GetStringFromResponse
            (
                NetUtils.Get
                (
                    $"{apiPath}/payment-history/v2/persons/{wallet}/payments/total",
                    "application/json",
                    "",
                    new NameValueCollection
                    {
                            { "Authorization", $"Bearer {qiwiToken}" }
                    },
                    new NameValueCollection
                    {
                            { "startDate", HttpUtility.UrlEncode(startDate.ToString("o")) },
                            { "endDate", HttpUtility.UrlEncode(endDate.ToString("o")) },
                            { "operation", Enum.GetName(typeof(QiwiModels.PaymentOperationType), operation) }
                    }
                )
            ),
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }
        );
        public QiwiModels.Transaction GetTransactionDetails(long transactionID, QiwiModels.PaymentOperationType operation)
        => JsonConvert
        .DeserializeObject<QiwiModels.Transaction>
        (
            NetUtils.GetStringFromResponse
            (
                NetUtils.Get
                (
                    $"{apiPath}/payment-history/v2/transactions/{transactionID}",
                    "application/json",
                    "",
                    new NameValueCollection
                    {
                            { "Authorization", $"Bearer {qiwiToken}" }
                    },
                    new NameValueCollection
                    {
                            { "type", Enum.GetName(typeof(QiwiModels.PaymentOperationType), operation) }
                    }
                )
            ),
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }
        );
        public string GetCheque(long transactionID, string operation, QiwiModels.ChequeImageFormat imageFormat)
        => NetUtils.GetStringFromResponse
            (
                NetUtils.Get
                (
                    $"{apiPath}/payment-history/v1/transactions/{transactionID}/cheque/file",
                    "application/json",
                    "",
                    new NameValueCollection
                    {
                            { "Authorization", $"Bearer {qiwiToken}" }
                    },
                    new NameValueCollection
                    {
                            { "type", operation },
                            { "format", Enum.GetName(typeof(QiwiModels.ChequeImageFormat), imageFormat) }
                    }
                )
            );
        public bool SendCheque(long transactionID, QiwiModels.PaymentOperationType operation, string email)
        => NetUtils.JsonPost
           (
               $"{apiPath}/payment-history/v1/transactions/{transactionID}/cheque/send",
               new QiwiModels.MailForCheque { Email = email },
               "application/json",
               "application/json",
               new NameValueCollection
               {
                       { "Authorization", $"Bearer {qiwiToken}" }
               },
               new NameValueCollection
               {
                       { "type", Enum.GetName(typeof(QiwiModels.PaymentOperationType), operation) }
               }
           ).StatusCode == HttpStatusCode.Created;
        public QiwiModels.WalletBalance GetBalances(long personID)
        => JsonConvert
        .DeserializeObject<QiwiModels.WalletBalance>
        (
            NetUtils.GetStringFromResponse
            (
                NetUtils.Get
                (
                    $"{apiPath}/funding-sources/v2/persons/{personID}/accounts",
                    "application/json",
                    "",
                    new NameValueCollection
                    {
                            { "Authorization", $"Bearer {qiwiToken}" }
                    }
                )
            ),
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }
        );
    }
}
