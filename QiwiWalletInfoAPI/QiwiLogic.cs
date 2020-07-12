using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace QiwiLogic
{
    /// <summary>
    /// Класс, содержащий логику работы с QIWI WALLET API
    /// </summary>
    public class QiwiAPI
    {
        /// <summary>
        /// Пользовательский токен QIWI. Необходимо получить на https://qiwi.com/api.
        /// При выпуске рекомендуется указать все разрешения, КРОМЕ проведения платежей.
        /// </summary>
        string qiwiToken = "";
        /// <summary>
        /// Путь до хоста QIWI WALLET APi
        /// </summary>
        string apiPath = "https://edge.qiwi.com";
        /// <summary>
        /// Конструктор класса QiwiAPI
        /// </summary>
        /// <param name="token">Пользовательский токен для работы с QIWI WALLET API</param>
        public QiwiAPI(string token) => qiwiToken = token;
        /// <summary>
        /// Получение информации о профиле QIWI WALLET
        /// </summary>
        /// <returns>QiwiWalletProfile, содержащего информацию о профиле QIWI WALLET</returns>
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
        /// <summary>
        /// Идентификация QIWI WALLET
        /// </summary>
        /// <param name="id">Идентификационные данные персоны</param>
        /// <param name="wallet">Номер кошелька QIWI</param>
        /// <returns>Булево значение "верно", если идентификация пройдена успешно, иначе "ложь"</returns>
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
        /// <summary>
        /// Получение идентификационных данных персоны
        /// </summary>
        /// <param name="wallet">Номер кошелька QIWI</param>
        /// <returns>IdentificationData, содержащая идентификационные данные персоны</returns>
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
        /// <summary>
        /// Получение информации о проведённых платежах
        /// </summary>
        /// <param name="wallet">Номер кошелька QIWI</param>
        /// <param name="rows">Количество последних N операций</param>
        /// <param name="operation">Тип операции (приход, расход, все)</param>
        /// <returns>Payments, содержащая информацию о проведённых операциях с указанным кошельком QIWI</returns>
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
        /// <summary>
        /// Получение статистики проведения операций по кошельку QIWI
        /// </summary>
        /// <param name="wallet">Номер кошелька QIWI</param>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <param name="operation">Тип операции (приход, расход, все)</param>
        /// <returns>PaymentsStats, содержащая статистику проведения операций</returns>
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
        /// <summary>
        /// Получение деталей транзакции
        /// </summary>
        /// <param name="transactionID">Идентификационный номер транзакции</param>
        /// <param name="operation">Тип операции (приход, расход, все)</param>
        /// <returns>Transaction, содержащая детали проведённой транзакции</returns>
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
        /// <summary>
        /// [НЕ ИСПОЛЬЗОВАТЬ ДО ИЗМЕНЕНИЙ]
        /// Получение чека проведённой транзакции
        /// </summary>
        /// <param name="transactionID">Идентификационный номер транзакции</param>
        /// <param name="operation">Тип операции (приход, расход, все)</param>
        /// <param name="imageFormat">Формат чека (JPEG, PDF)</param>
        /// <returns>Строку, содержащую ответ сервера на запрос</returns>
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
        /// <summary>
        /// [НЕ ИСПОЛЬЗОВАТЬ ДО ИЗМЕНЕНИЙ]
        /// Отправка чека проведённой транзакции на указанный электронный адрес
        /// </summary>
        /// <param name="transactionID">Идентификационный номер транзакции</param>
        /// <param name="operation">Тип операции (приход, расход, все)</param>
        /// <param name="email">Электронный адрес</param>
        /// <returns>Булево значение "верно", если чек был отправлен на указанную почту, иначе "ложь"</returns>
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
        /// <summary>
        /// Получение баланса
        /// </summary>
        /// <param name="personID">Идентификатор персоны (кошелька киви)</param>
        /// <returns>WalletBalance, содержащий информацию о балансе указанного кошелька QIWI</returns>
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
