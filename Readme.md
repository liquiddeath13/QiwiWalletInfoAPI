### Реализованные возможности ###
1. Получение информации о кошельке QIWI
1. Получение информации о владельце кошелька QIWI
1. Получение информации о балансе кошелька QIWI
1. Получение информации об истории проведения операций по указанному кошельку QIWI
1. Получение информации об уровне идентификации кошелька и идентификационных данных персоны
1. Идентификация кошелька
1. Получение информации о проведённой транзакции
1. Получение чека проведённой транзакции [нуждается в правках, не использовать!!!]
### Пример работы ###
Вывод на экран информации о последних 50 проведённых операциях:
```C#
QiwiAPI qa = new QiwiAPI("токен");
List<QiwiModels.Transaction> paymentsList = qa.GetPayments(long.Parse("номер телефона"), 50).Data;
foreach (var payment in paymentsList)
{
    Console.WriteLine($"ID транзакции: {payment.TxnId}.");
    Console.WriteLine($"Дата транзакции: {payment.Date}.");
    Console.WriteLine($"Тип транзакции: {payment.Type}.");
    Console.WriteLine($"Источник транзакции: {payment.Account}.");
    Console.WriteLine($"Транзакция затронула кошелёк #{payment.PersonId}.");
    Console.WriteLine($"Сумма: {payment.Sum.Amount}. Комиссия: {payment.Commission.Amount}.");
    Console.WriteLine($"Комментарий: {payment.Comment}.");
}
```
### TODO ###
1. Добавление возможности работы с p2p API
1. Исправление ошибок работы
1. Рефакторинг
